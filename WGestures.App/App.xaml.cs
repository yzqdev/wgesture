using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Serilog;
using Serilog.Events;
using WGestures.App.Configuration;
using WGestures.App.Gui.Forms;
using WGestures.App.Infrastructure;
using WGestures.App.Migrate;
using WGestures.App.Properties;
using WGestures.App.Views;
using WGestures.Common;
using WGestures.Common.Config;
using WGestures.Common.OsSpecific.Windows;
using WGestures.Common.Product;
using WindowsInput;
using WindowsInput.Events;
using WindowsInput.Native;
using WGestures.Core;
using WGestures.Core.Impl.Windows;
using WGestures.Core.Persistence;
using Application = System.Windows.Application;

namespace WGestures.App;

public partial class App : Application
{
    private Mutex _mutex;
    private GestureParser _gestureParser;
    private PlistConfig _config;
    private CanvasWindowGestureView _gestureView;
    private readonly IList<IDisposable> _componentsToDispose = new List<IDisposable>();
    private SettingsFormController _settingsFormController;
    private bool _isFirstRun;
    private JsonGestureIntentStore _defaultIntentStore;
    private Win32GestureIntentFinder _intentFinder;
    private TrayIconController _trayIcon;
    private GlobalHotKeyManager _hotkeyMgr;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ConfigureLogging();
        Log.Information("WGestures {Version} 启动", typeof(App).Assembly.GetName().Version);

        Trace.Listeners.Add(new DetailedConsoleListener());

        if (IsDuplicateInstance())
        {
            Log.Information("检测到已有实例，经 IPC 唤醒后退出");
            IpcServer.PostCommand("ShowSettings");
            Shutdown();
            return;
        }

        try
        {
            AppWideInit();
            LoadFailSafeConfigFile();
            SyncAutoStartState();
            CheckAndDoFirstRunStuff();
            ConfigureComponents();
            StartParserThread();

            ShowTrayIcon();
            IpcServer.Start(cmd =>
            {
                if (cmd == "ShowSettings") ShowSettings();
            });

            Current.Dispatcher.Invoke(() => { });
        }

        catch (Exception ex)
        {
            ShowFatalError(ex);
        }

    }

    private static void ConfigureLogging()
    {
        System.IO.Directory.CreateDirectory(AppSettings.LogsDirectory);

        // App.config 的 LogLevel 键（Trace/Debug/Information/Warning/Error...），缺省 Information
        var level = Enum.TryParse<LogEventLevel>(ConfigurationManager.AppSettings.Get("LogLevel"), out var parsed)
            ? parsed : LogEventLevel.Information;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(level)
            .WriteTo.File(
                System.IO.Path.Combine(AppSettings.LogsDirectory, "wgestures-.log"),
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Dispose();
        Log.Information("已退出");
        Log.CloseAndFlush();
        base.OnExit(e);
    }

    private bool IsDuplicateInstance()
    {
        bool createdNew;
        _mutex = new Mutex(true, Constants.Identifier, out createdNew);
        if (!createdNew)
        {
            _mutex.ReleaseMutex();
            _mutex.Dispose();
            return true;
        }

        return false;
    }

    private void ShowFatalError(Exception e)
    {
        Log.Fatal(e, "致命错误");
        Current.Dispatcher.Invoke(() =>
        {
            var frm = new ErrorWindow { Title = typeof(App).Assembly.GetName().Name };
            frm.ErrorText = e.ToString();
            frm.ShowDialog();
            Environment.Exit(1);
        });
    }

    private void AppWideInit()
    {
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Native.SetProcessDPIAware();

        Thread.CurrentThread.IsBackground = false;
        Thread.CurrentThread.Name = "入口线程";

        using (var proc = Process.GetCurrentProcess())
        {
            proc.PriorityClass = ProcessPriorityClass.High;
        }

        _hotkeyMgr = new GlobalHotKeyManager();
    }

    private void Dispose()
    {
        try
        {
            foreach (var disposable in _componentsToDispose)
            {
                disposable?.Dispose();
            }

            _componentsToDispose.Clear();
            WGestures.App.Properties.Resources.ResourceManager.ReleaseAllResources();
        }
        finally
        {
            if (_mutex != null && _mutex.WaitOne(0))
            {
                _mutex.ReleaseMutex();
            }
            _mutex?.Dispose();
        }
    }

    private void StartParserThread()
    {
        new Thread(() =>
        {
#if DEBUG
            _gestureParser.Start();
#else
            try
            {
                _gestureParser.Start();
            }
            catch (Exception e)
            {
                ShowFatalError(e);
            }
#endif
        }, maxStackSize: 65536) { Name = "Parser线程", Priority = ThreadPriority.Highest, IsBackground = false }.Start();
    }

    private void LoadFailSafeConfigFile()
    {
        if (!System.IO.File.Exists(AppSettings.ConfigFilePath))
        {
            var sourcePath = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(GetType().Assembly.Location)!,
                "defaults", "config.json");
            System.IO.File.Copy(sourcePath, AppSettings.ConfigFilePath);
        }

        try
        {
            _config = new PlistConfig(AppSettings.ConfigFilePath);
        }
        catch (Exception)
        {
            Debug.WriteLine("App.OnStartup: config文件损坏，重新恢复默认！");
            System.IO.File.Delete(AppSettings.ConfigFilePath);
            var sourcePath = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(GetType().Assembly.Location)!,
                "defaults", "config.json");
            System.IO.File.Copy(sourcePath, AppSettings.ConfigFilePath);
            _config = new PlistConfig(AppSettings.ConfigFilePath);
        }

        LoadGestureStore();
    }

    private void LoadGestureStore()
    {
        try
        {
            if (!System.IO.File.Exists(AppSettings.DefaultGesturesFilePath))
            {
                throw new System.IO.FileNotFoundException("找不到出厂默认手势文件：" + AppSettings.DefaultGesturesFilePath);
            }

            _defaultIntentStore = new JsonGestureIntentStore(AppSettings.DefaultGesturesFilePath, AppSettings.GesturesFileVersion);

            if (System.IO.File.Exists(AppSettings.GesturesFilePath))
            {
                _defaultIntentStore = new JsonGestureIntentStore(AppSettings.GesturesFilePath, AppSettings.GesturesFileVersion);
            }
            else
            {
                var sourceGestures = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(GetType().Assembly.Location)!,
                    "defaults",
                    AppSettings.DefaultConfigGestureFileName);
                System.IO.File.Copy(sourceGestures, AppSettings.GesturesFilePath);
                _defaultIntentStore = new JsonGestureIntentStore(AppSettings.GesturesFilePath, AppSettings.GesturesFileVersion);
            }

            if (_config.Dict.FileVersion != AppSettings.ConfigFileVersion ||
                _defaultIntentStore.FileVersion != AppSettings.GesturesFileVersion)
            {
                throw new Exception("配置文件版本不正确");
            }

            _defaultIntentStore.Save();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "加载或合并手势文件出错");

            try
            {
                if (System.IO.File.Exists(AppSettings.GesturesFilePath))
                    System.IO.File.Delete(AppSettings.GesturesFilePath);
                System.IO.File.Copy(AppSettings.DefaultUserGesturesFilePath, AppSettings.GesturesFilePath);
                _defaultIntentStore = new JsonGestureIntentStore(AppSettings.GesturesFilePath, AppSettings.GesturesFileVersion);
                _defaultIntentStore.Save();
            }
            catch (Exception fatalEx)
            {
                Log.Error(fatalEx, "安全恢复手势失败");
                throw;
            }
        }
    }

    private void SyncAutoStartState()
    {
        var fact = AutoStarter.IsRegistered(Constants.Identifier, System.Reflection.Assembly.GetExecutingAssembly().Location);
        var conf = _config.Dict.AutoStart;

        if (fact == conf && !_isFirstRun) return;

        try
        {
            if (conf)
            {
                AutoStarter.Register(Constants.Identifier, System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            else
            {
                AutoStarter.Unregister(Constants.Identifier);
            }
        }
        catch (Exception)
        {
#if DEBUG
            throw;
#endif
        }
    }

    private void CheckAndDoFirstRunStuff()
    {
        _isFirstRun = _config.Dict.IsFirstRun;

        if (_isFirstRun)
        {
            _config.Dict.GestureParserEnableHotCorners = true;
            ImportPreviousVersion();
            _config.Dict.IsFirstRun = false;
            _config.Dict.AutoCheckForUpdate = true;
            _config.Dict.AutoStart = true;
            _config.Dict.PathTrackerTriggerButton =
                (int)(GestureTriggerButton.Right | GestureTriggerButton.Middle | GestureTriggerButton.X);
            _config.Save();

            ShowQuickStartGuide(isFirstRun: true);
            Warning360Safe();
        }
    }

    private void ImportPreviousVersion()
    {
        try
        {
            var prevConfigAndGestures = MigrateService.ImportPrevousVersion();
            if (prevConfigAndGestures == null) return;

            _defaultIntentStore.Import(prevConfigAndGestures.GestureIntentStore);
            _config.Import(prevConfigAndGestures.Config);
            _defaultIntentStore.Save();
        }
        catch (MigrateException)
        {
#if DEBUG
            throw;
#endif
        }
    }

    private void ShowQuickStartGuide(bool isFirstRun = false)
    {
        var t = new Thread(() =>
        {
            Current.Dispatcher.Invoke(() =>
            {
                var mut = new Mutex(true, Constants.Identifier + "QuickStartGuideWindow", out bool createdNew);
                if (!createdNew)
                {
                    mut.Close();
                    return;
                }

                try
                {
                    var frm = new QuickStartGuideWindow();
                    frm.Closed += (s, e) => mut.Close();
                    frm.Show();
                    frm.Activate();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"QuickStart 运行崩溃: {ex}");
                    mut.Close();
                }
            });
        }) { IsBackground = true };

        t.SetApartmentState(ApartmentState.STA);
        t.Start();
    }

    private void Warning360Safe()
    {
        var proc360 = Process.GetProcessesByName("360Safe");
        var proc360Tray = Process.GetProcessesByName("360Tray");

        if (proc360.Length + proc360Tray.Length > 0)
        {
            Current.Dispatcher.Invoke(() =>
            {
                var warn = new Warn360View();
                warn.ShowDialog();
            });
        }
    }

    private void ShowSettings()
    {
        if (_settingsFormController != null)
        {
            _settingsFormController.BringToFront();
            return;
        }

        using (_settingsFormController = new SettingsFormController(_config, _gestureParser,
                   (Win32MousePathTracker2)_gestureParser.PathTracker, _gestureView, _hotkeyMgr, _defaultIntentStore))
        {
            _settingsFormController.ShowDialog();
        }

        _settingsFormController = null;
    }

    private void ConfigureComponents()
    {
        _intentFinder = new Win32GestureIntentFinder(_defaultIntentStore);
        var pathTracker = new Win32MousePathTracker2();
        _gestureParser = new GestureParser(pathTracker, _intentFinder);

        _gestureView = new CanvasWindowGestureView(_gestureParser);

        _componentsToDispose.Add(_gestureParser);
        _componentsToDispose.Add(_gestureView);
        _componentsToDispose.Add(pathTracker);
        _componentsToDispose.Add(_hotkeyMgr);

        ConfigurePathTracker(pathTracker);
        ConfigureGestureView();
        ConfigureGestureParser();
        ConfigureHotkeys();
    }

    private void ConfigurePathTracker(Win32MousePathTracker2 pathTracker)
    {
        pathTracker.DisableInFullscreen = _config.Dict.PathTrackerDisableInFullScreen;
        pathTracker.PreferWindowUnderCursorAsTarget = _config.Dict.PathTrackerPreferCursorWindow;
        pathTracker.TriggerButton = (GestureTriggerButton)_config.Dict.PathTrackerTriggerButton;
        pathTracker.InitialValidMove = _config.Dict.PathTrackerInitialValidMove;
        pathTracker.StayTimeout = _config.Dict.PathTrackerStayTimeout;
        pathTracker.StayTimeoutMillis = _config.Dict.PathTrackerStayTimeoutMillis;
        pathTracker.InitialStayTimeout = _config.Dict.PathTrackerInitialStayTimeout;
        pathTracker.InitialStayTimeoutMillis = _config.Dict.PathTrackerInitialStayTimoutMillis;
        pathTracker.RequestPauseResume += paused => TogglePause();
        pathTracker.EnableWindowsKeyGesturing = _config.Dict.EnableWindowsKeyGesturing;
        pathTracker.RequestShowHideTray += ToggleTrayIconVisibility;
    }

    private void ConfigureGestureView()
    {
        _gestureView.ShowPath = _config.Dict.GestureViewShowPath;
        _gestureView.ShowCommandName = _config.Dict.GestureViewShowCommandName;
        _gestureView.ViewFadeOut = _config.Dict.GestureViewFadeOut;
        _gestureView.PathMainColor = System.Drawing.Color.FromArgb(_config.Dict.GestureViewMainPathColor);
        _gestureView.PathAlternativeColor = System.Drawing.Color.FromArgb(_config.Dict.GestureViewAlternativePathColor);
        _gestureView.PathMiddleBtnMainColor = System.Drawing.Color.FromArgb(_config.Dict.GestureViewMiddleBtnMainColor);
        _gestureView.PathXBtnMainColor = System.Drawing.Color.FromArgb(_config.Dict.GestureViewXBtnPathColor);
    }

    private void ConfigureGestureParser()
    {
        _gestureParser.EnableHotCorners = _config.Dict.GestureParserEnableHotCorners;
        _gestureParser.Enable8DirGesture = _config.Dict.GestureParserEnable8DirGesture;
        _gestureParser.EnableRubEdge = _config.Dict.GestureParserEnableRubEdges;
    }

    private void ConfigureHotkeys()
    {
        _hotkeyMgr.HotKeyPreview += HotkeyMgr_HotKeyPreview;
        _hotkeyMgr.HotKeyRegistered += HotkeyMgr_Updated;
        _hotkeyMgr.HotKeyUnRegistered += HotkeyMgr_Updated;

        byte[] pauseHotKey = null;
        try
        {
            pauseHotKey = System.Text.Encoding.UTF8.GetBytes(Base64Decode(_config.Dict.PauseResumeHotKey));
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Hotkey decode error: " + ex);
        }

        if (pauseHotKey != null && pauseHotKey.Length > 0)
        {
            var hotkey = GlobalHotKeyManager.HotKey.FromBytes(pauseHotKey);
            try
            {
                _hotkeyMgr.RegisterHotKey(ConfigKeys.PauseResumeHotKey, hotkey, null);
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine("Hotkey register error: " + ex);
            }
        }
    }

    private bool HotkeyMgr_HotKeyPreview(GlobalHotKeyManager mgr, string id, GlobalHotKeyManager.HotKey hk)
    {
        if (id == ConfigKeys.PauseResumeHotKey)
        {
            Debug.WriteLine("HotKey Pressed: " + hk);
            TogglePause();
            return true;
        }

        return false;
    }

    private void HotkeyMgr_Updated(string arg1, GlobalHotKeyManager.HotKey arg2)
    {
        _trayIcon?.UpdatePauseState();
    }

    private void TogglePause()
    {
        _gestureParser?.TogglePause();
    }

    private void ShowTrayIcon()
    {
        Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

        _trayIcon = new TrayIconController(_config,
            isPaused: () => _gestureParser.IsPaused,
            pauseHotkeyString: GetPauseResumeHotkeyString,
            togglePause: TogglePause,
            showSettings: ShowSettings,
            showQuickStart: () => ShowQuickStartGuide(),
            releaseKeyLock: () => Simulate.Events().Release(KeyCode.LWin).Wait(100).Invoke(),
            restart: RestartApp,
            exit: ExitApp);

        _gestureParser.StateChanged += _ => _trayIcon.UpdatePauseState();
    }

    private string GetPauseResumeHotkeyString()
    {
        var hk = _hotkeyMgr?.GetRegisteredHotKeyById(ConfigKeys.PauseResumeHotKey);
        return hk?.ToString() ?? "";
    }

    private void ToggleTrayIconVisibility()
    {
        _trayIcon?.ToggleVisibility();
    }

    private void RestartApp()
    {
        _gestureParser?.Stop();
        Application.Current.Dispatcher.Invoke(() =>
        {
            Application.Current.Shutdown();
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
        });
    }

    private void ExitApp()
    {
        _gestureParser?.Stop();
        _trayIcon?.Dispose();
        Current.Shutdown();
    }

    private static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}