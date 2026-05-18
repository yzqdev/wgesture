using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using WGestures.App.Gui.Windows;
using WGestures.App.Migrate;
using WGestures.App.Properties;
using WGestures.App.Views;
using WGestures.Common;
using WGestures.Common.Config.Impl;
using WGestures.Common.OsSpecific.Windows;
using WGestures.Common.Product;
using WGestures.Core;
using WGestures.Core.Impl.Windows;
using WGestures.Core.Persistence.Impl;
using WGestures.Core.Persistence.Impl.Windows;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

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
    private Win32GestrueIntentFinder _intentFinder;
    private NotifyIcon _trayIcon;
    private GlobalHotKeyManager _hotkeyMgr;
    private ToolStripMenuItem _menuItemPause;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        Trace.Listeners.Add(new DetailedConsoleListener());

        if (IsDuplicateInstance())
        {
            PostIpcCmd("ShowSettings");
            Shutdown();
            return;
        }

        AppWideInit();

        try
        {
            LoadFailSafeConfigFile();
            SyncAutoStartState();
            CheckAndDoFirstRunStuff();
            ConfigureComponents();
            StartParserThread();

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();

            ShowTrayIcon();
            StartIpcPipe();

            Current.Dispatcher.Invoke(() => { });
        }
 
        catch (Exception ex)
        {
            ShowFatalError(ex);
        }
 
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Dispose();
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

    private void PostIpcCmd(string cmd)
    {
        try
        {
            using (var pipeClient = new System.IO.Pipes.NamedPipeClientStream("WGestures_IPC_API"))
            {
                pipeClient.Connect(1000);
                using (var writer = new System.IO.StreamWriter(pipeClient) { AutoFlush = true })
                {
                    writer.WriteLine(cmd);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("PostIpcCmd Error: " + ex.Message);
        }
    }

    private void StartIpcPipe()
    {
        var synCtx = new WindowsFormsSynchronizationContext();
        var pipeThread = new Thread(() =>
        {
            while (true)
            {
                try
                {
                    using (var server = new System.IO.Pipes.NamedPipeServerStream("WGestures_IPC_API"))
                    {
                        server.WaitForConnection();

                        Debug.WriteLine("Client Connected");
                        using (var reader = new System.IO.StreamReader(server))
                        {
                            var cmd = reader.ReadLine();
                            Debug.WriteLine("Pipe CMD=" + cmd);

                            if (cmd == "ShowSettings")
                            {
                                synCtx.Post(s =>
                                {
                                    Debug.WriteLine("Thread=" + Thread.CurrentThread.ManagedThreadId);
                                    ShowSettings();
                                }, null);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("IPC Pipe Error: " + ex.Message);
                }
            }
        }, maxStackSize: 65536) { IsBackground = true };
        pipeThread.Start();
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
            Debug.WriteLine("加载或合并手势文件出错：" + ex);

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
                Debug.WriteLine("安全恢复手势失败: " + fatalEx);
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
        _intentFinder = new Win32GestrueIntentFinder(_defaultIntentStore);
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
        pathTracker.RequestPauseResume += paused => MenuItemPause_Click(null, EventArgs.Empty);
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
        UpdateTray();
    }

    private void TogglePause()
    {
        _gestureParser?.TogglePause();
    }

    private void ShowTrayIcon()
    {
        _trayIcon = CreateNotifyIcon();
        Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
    }

    private NotifyIcon CreateNotifyIcon()
    {
        var notifyIcon = new NotifyIcon();
        notifyIcon.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " " +
                         System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        var contextMenu1 = new ContextMenuStrip();

        var menuItemExit = new ToolStripMenuItem { Text = "退出" };
        menuItemExit.Click += MenuItemExit_Click;

        var menuItemRestart = new ToolStripMenuItem { Text = "重启" };
        menuItemRestart.Click += MenuItemRestart_Click;

        _menuItemPause = new ToolStripMenuItem { Text = "暂停" };
        _menuItemPause.Click += MenuItemPause_Click;

        var menuItemResume = new ToolStripMenuItem { Text = "解除按键死锁" };
        menuItemResume.Click += MenuItemResume_Click;

        var menuItemSettings = new ToolStripMenuItem { Text = "设置" };
        menuItemSettings.Click += MenuItemSettings_Click;

        var menuItemShowQuickStart = new ToolStripMenuItem { Text = "快速入门" };
        menuItemShowQuickStart.Click += (sender, args) => ShowQuickStartGuide();

        contextMenu1.Items.AddRange(new ToolStripItem[]
        {
            _menuItemPause, menuItemResume, menuItemRestart, new ToolStripSeparator(),
            menuItemSettings, menuItemShowQuickStart, new ToolStripSeparator(), menuItemExit
        });

        notifyIcon.Icon = WGestures.App.Properties.Resources.trayIcon;
        notifyIcon.ContextMenuStrip = contextMenu1;
        notifyIcon.Visible = true;

        _gestureParser.StateChanged += GestureParser_StateChanged;

        return notifyIcon;
    }

    private void UpdateTray()
    {
        if (_trayIcon == null) return;

        var hotKeyStr = GetPauseResumeHotkeyString();
        hotKeyStr = string.IsNullOrEmpty(hotKeyStr) ? "" : $"({hotKeyStr})";

        if (_gestureParser?.IsPaused ?? false)
        {
            _menuItemPause.Text = "继续 " + hotKeyStr;
            _trayIcon.Icon = WGestures.App.Properties.Resources.trayIcon_bw;
        }
        else
        {
            _menuItemPause.Text = "暂停 " + hotKeyStr;
            _trayIcon.Icon = WGestures.App.Properties.Resources.trayIcon;
        }
    }

    private string GetPauseResumeHotkeyString()
    {
        var hk = _hotkeyMgr?.GetRegisteredHotKeyById(ConfigKeys.PauseResumeHotKey);
        return hk?.ToString() ?? "";
    }

    private void ToggleTrayIconVisibility()
    {
        if (!(_trayIcon.Visible && !_config.Dict.TrayIconVisible))
        {
            _config.Dict.TrayIconVisible = !_trayIcon.Visible;
            _config.Save();
        }

        if (_trayIcon.Visible)
        {
            _trayIcon.ShowBalloonTip(10 * 1000, "WGestures图标将隐藏", "按 Shift+左键+中键 恢复显示\n再次运行程序可打开设置界面", ToolTipIcon.Info);
        }
        else
        {
            _trayIcon.Visible = true;
        }
    }

    private void MenuItemSettings_Click(object sender, EventArgs e)
    {
        ShowSettings();
    }

    private void MenuItemPause_Click(object sender, EventArgs e)
    {
        TogglePause();
    }

    private void MenuItemRestart_Click(object sender, EventArgs e)
    {
        _gestureParser?.Stop();
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            System.Windows.Application.Current.Shutdown();
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
        });
    }

    private void MenuItemExit_Click(object sender, EventArgs e)
    {
        _gestureParser?.Stop();
        _trayIcon?.Dispose();
        Current.Shutdown();
    }

    private void MenuItemResume_Click(object sender, EventArgs e)
    {
        WindowsInput.Simulate.Events().Release(WindowsInput.Events.KeyCode.LWin).Wait(100).Invoke();
    }

    private void GestureParser_StateChanged(GestureParser.State s)
    {
        UpdateTray();
    }

    private void ScheduledUpdateCheck(object sender, NotifyIcon tray)
    {
        if (!_config.Dict.AutoCheckForUpdate) return;

        var checker = new VersionChecker(AppSettings.CheckForUpdateUrl);
        checker.Finished += info =>
        {
            var whatsNew = info.WhatsNew.Length > 50 ? info.WhatsNew.Substring(0, 50) : info.WhatsNew;

            if (info.Version != System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString())
            {
                tray.BalloonTipClicked += (o, args) =>
                {
                    if (info.Version == System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()) return;
                    var frm = new UpdateInfoWindow(
                        System.Configuration.ConfigurationManager.AppSettings.Get(Constants.ProductHomePageAppSettingKey), info);

                    Current.Dispatcher.Invoke(() => frm.ShowDialog());
                    tray.Visible = _config.Dict.TrayIconVisible;
                };
                if (!tray.Visible)
                {
                    tray.Visible = true;
                }

                tray.ShowBalloonTip(1000 * 15, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "新版本可用!",
                    "版本:" + info.Version + "\n" + whatsNew, ToolTipIcon.Info);
            }

            checker.Dispose();
        };
        checker.ErrorHappened += ex =>
        {
            Debug.WriteLine("App.ScheduledUpdateCheck Error:" + ex.Message);
            checker.Dispose();
            Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(
                    $"检查更新失败，原因：\n{ex.Message}",
                    "错误",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            });
        };

        checker.CheckAsync();
    }

    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    public static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}