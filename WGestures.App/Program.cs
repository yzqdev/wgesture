using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Versioning;
using System.Threading;
using System.Windows.Forms;
using WGestures.App.Gui.Windows;
using WGestures.App.Migrate;
using WGestures.App.Properties;
using WGestures.Common;
using WGestures.Common.Config.Impl;
using WGestures.Common.OsSpecific.Windows;
using WGestures.Common.Product;
using WGestures.Core;
using WGestures.Core.Impl.Windows;
using WGestures.Core.Persistence.Impl;
using WGestures.Core.Persistence.Impl.Windows;
using WindowsInput;
using WindowsInput.Events;
using Timer = System.Windows.Forms.Timer;

namespace WGestures.App {
    static class Program {
        static Mutex mutext;
        static GestureParser gestureParser;

        static PlistConfig config;
        static CanvasWindowGestureView gestureView;

        static readonly IList<IDisposable> componentsToDispose = new List<IDisposable>();
        static SettingsFormController settingsFormController;

        static bool isFirstRun;
        static JsonGestureIntentStore intentStore;
        static Win32GestrueIntentFinder intentFinder;

        static NotifyIcon trayIcon;
        static GlobalHotKeyManager hotkeyMgr;

        //for adding hotkey
        static ToolStripMenuItem menuItem_pause;

        [STAThread]
        [SupportedOSPlatform("windows")]
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new DetailedConsoleListener());

            if (IsDuplicateInstance())
            {
                //让主实例打开`设置`
                PostIpcCmd("ShowSettings");
                return;
            }

            AppWideInit();

            try
            {
                //加载配置文件，如果文件不存在或损坏，则加载默认配置文件
                LoadFailSafeConfigFile();

                SyncAutoStartState();
                CheckAndDoFirstRunStuff();

                ConfigureComponents();
                StartParserThread();

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                GC.WaitForPendingFinalizers();

                //显示托盘图标
                ShowTrayIcon();

                //监听IPC消息
                StartIpcPipe();


                Application.Run();

            }
#if !DEBUG
            catch (Exception e)
            {
                ShowFatalError(e);
            }
#endif
            finally { Dispose(); }



        }

        //TODO: refactor out
        static void StartIpcPipe()
        {
            //todo: impl a set of IPC APIs to perform some cmds. eg.Pause/Resume, Quit...
            //todo: Temporay IPC mechanism.
            var synCtx = new WindowsFormsSynchronizationContext();//note: won't work with `SynchronizationContext.Current`
            var pipeThread = new Thread(() =>
            {
                while (true)
                {
                    using (var server = new System.IO.Pipes.NamedPipeServerStream("WGestures_IPC_API"))
                    {
                        server.WaitForConnection();

                        Debug.WriteLine("Clien Connected");
                        using (var reader = new StreamReader(server))
                        {
                            var cmd = reader.ReadLine();
                            Debug.WriteLine("Pipe CMD=" + cmd);

                            if (cmd == "ShowSettings")
                            {
                                synCtx.Post((s) =>
                                {
                                    Debug.WriteLine("Thread=" + Thread.CurrentThread.ManagedThreadId);
                                    ShowSettings();
                                }, null);
                            }
                        }
                    }
                }
            }, maxStackSize: 1)
            { IsBackground = true };
            pipeThread.Start();
        }

        static void PostIpcCmd(string cmd)
        {
            using (var pipeClient = new System.IO.Pipes.NamedPipeClientStream("WGestures_IPC_API"))
            {
                pipeClient.Connect();
                using (var writer = new StreamWriter(pipeClient) { AutoFlush = true })
                {
                    writer.WriteLine(cmd);
                }
            }
        }

        static void StartParserThread()
        {
            new Thread(() =>
            {
#if DEBUG
                gestureParser.Start();
#else
                try
                {
                    gestureParser.Start();
                }
                catch (Exception e)
                {
                    ShowFatalError(e);
                }
#endif
            }, maxStackSize: 1)
            { Name = "Parser线程", Priority = ThreadPriority.Highest, IsBackground = false }.Start();
        }
        /// <summary>
        /// 再启动一个wgesture
        /// </summary>
        /// <returns></returns>
        static bool IsDuplicateInstance()
        {
            bool createdNew;
            mutext = new Mutex(true, Constants.Identifier, out createdNew);
            if (!createdNew)
            {
                mutext.Close();
                return true;
            }
            return false;
        }

        static void ShowFatalError(Exception e)
        {
            var frm = new ErrorForm() { Text = Application.ProductName };
            frm.ErrorText = e.ToString();
            frm.ShowDialog();
            Environment.Exit(1);
        }
        /// <summary>
        /// 检查是否是第一次运行
        /// </summary>
        static void CheckAndDoFirstRunStuff()
        {
            //是否是第一次运行
            bool isFirstRun = config.Dict.IsFirstRun;
             

            if (isFirstRun)
            {
                //默认值
                //config.Set(ConfigKeys.GestureParserEnableHotCorners, true);
                config.Dict.GestureParserEnableHotCorners = true;
                ImportPrevousVersion();

                //强制值
                config.Dict.IsFirstRun = false;
               
                config.Dict.AutoCheckForUpdate = true;
                
                config.Dict.AutoStart = true;
              
                config.Dict.PathTrackerTriggerButton=(int)(GestureTriggerButton.Right | GestureTriggerButton.Middle | GestureTriggerButton.X);

                config.Save();

                ShowQuickStartGuide(isFirstRun: true);
                Warning360Safe();
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        static void AppWideInit()
        {
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Native.SetProcessDPIAware();

            Thread.CurrentThread.IsBackground = false;
            Thread.CurrentThread.Name = "入口线程";

            using (var proc = Process.GetCurrentProcess())
            {
                //高优先级
                proc.PriorityClass = ProcessPriorityClass.High;
            }

            hotkeyMgr = new GlobalHotKeyManager();
        }
        /// <summary>
        /// 把defaults文件夹的配置文件复制到local文件夹
        /// </summary>
        static void LoadFailSafeConfigFile()
        {
            if (!File.Exists(AppSettings.ConfigFilePath))
            {
                File.Copy(string.Format("{0}/defaults/config.json", Path.GetDirectoryName(Application.ExecutablePath)), AppSettings.ConfigFilePath);
            }
            if (!File.Exists(AppSettings.GesturesFilePath))
            {
                File.Copy(string.Format("{0}/defaults/gestures.wg2", Path.GetDirectoryName(Application.ExecutablePath)), AppSettings.GesturesFilePath);
            }

            try
            { 
                //如果文件损坏，则替换。
                config = new PlistConfig(AppSettings.ConfigFilePath);
            }
            catch (Exception)
            {
                Debug.WriteLine("Program.Main: config文件损坏！");
                File.Delete(AppSettings.ConfigFilePath);
                File.Copy(string.Format("{0}/defaults/config.json", Path.GetDirectoryName(Application.ExecutablePath)), AppSettings.ConfigFilePath);

                config = new PlistConfig(AppSettings.ConfigFilePath);
            }

            try
            {
                intentStore = new JsonGestureIntentStore(AppSettings.GesturesFilePath, AppSettings.GesturesFileVersion);

                if (config.Dict.FileVersion != AppSettings.ConfigFileVersion ||
                intentStore.FileVersion != AppSettings.GesturesFileVersion)
                {
                    throw new Exception("配置文件版本不正确");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("加载配置文件出错：" + e);

                File.Delete(AppSettings.GesturesFilePath);
                File.Copy(string.Format("{0}/defaults/gestures.wg", Path.GetDirectoryName(Application.ExecutablePath)), AppSettings.GesturesFilePath);

                intentStore = new JsonGestureIntentStore(AppSettings.GesturesFilePath, AppSettings.GesturesFileVersion);
            }

        }

        /// <summary>
        /// 导入以前版本的配置
        /// </summary>
        static void ImportPrevousVersion()
        {
            try
            {
                //导入先前版本
                var prevConfigAndGestures = MigrateService.ImportPrevousVersion();
                if (prevConfigAndGestures == null) return;

                intentStore.Import(prevConfigAndGestures.GestureIntentStore);
                config.Import(prevConfigAndGestures.Config);

                intentStore.Save();
            }
            catch (MigrateException e)
            {
                //ignore
#if DEBUG
                throw;
#endif
            }
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
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        static void ConfigureComponents()
        {
            #region Create Components
            intentFinder = new Win32GestrueIntentFinder(intentStore);
            var pathTracker = new Win32MousePathTracker2();
            gestureParser = new GestureParser(pathTracker, intentFinder);

            gestureView = new CanvasWindowGestureView(gestureParser);

            componentsToDispose.Add(gestureParser);
            componentsToDispose.Add(gestureView);
            componentsToDispose.Add(pathTracker);
            componentsToDispose.Add(hotkeyMgr);
            #endregion

            #region pathTracker
            pathTracker.DisableInFullscreen = config.Dict.PathTrackerDisableInFullScreen ;
            pathTracker.PreferWindowUnderCursorAsTarget = config.Dict.PathTrackerPreferCursorWindow ;
            pathTracker.TriggerButton = (GestureTriggerButton)config.Dict.PathTrackerTriggerButton ;
            pathTracker.InitialValidMove = config.Dict.PathTrackerInitialValidMove;
            pathTracker.StayTimeout = config.Dict.PathTrackerStayTimeout ;
            pathTracker.StayTimeoutMillis = config.Dict.PathTrackerStayTimeoutMillis ;
            pathTracker.InitialStayTimeout = config.Dict.PathTrackerInitialStayTimeout ;
            pathTracker.InitialStayTimeoutMillis = config.Dict.PathTrackerInitialStayTimoutMillis ;
            pathTracker.RequestPauseResume += paused => menuItem_pause_Click(null, EventArgs.Empty);
            pathTracker.EnableWindowsKeyGesturing = config.Dict.EnableWindowsKeyGesturing ;
            pathTracker.RequestShowHideTray += ToggleTrayIconVisibility;

            #endregion

            #region gestureView
            gestureView.ShowPath = config.Dict.GestureViewShowPath ;
            gestureView.ShowCommandName = config.Dict.GestureViewShowCommandName ;
            gestureView.ViewFadeOut = config.Dict.GestureViewFadeOut ;
            gestureView.PathMainColor = Color.FromArgb(config.Dict.GestureViewMainPathColor );
            gestureView.PathAlternativeColor = Color.FromArgb(config.Dict.GestureViewAlternativePathColor );
            gestureView.PathMiddleBtnMainColor = Color.FromArgb(config.Dict.GestureViewMiddleBtnMainColor  );
            gestureView.PathXBtnMainColor = Color.FromArgb(config.Dict.GestureViewXBtnPathColor );
            #endregion

            #region GestureParser
            gestureParser.EnableHotCorners = config.Dict.GestureParserEnableHotCorners ;
            gestureParser.Enable8DirGesture = config.Dict.GestureParserEnable8DirGesture ;
            gestureParser.EnableRubEdge = config.Dict.GestureParserEnableRubEdges ;

            #endregion
            //HOt key
            hotkeyMgr.HotKeyPreview += HotkeyMgr_HotKeyPreview;
            hotkeyMgr.HotKeyRegistered += HotkeyMgr_Updated;
            hotkeyMgr.HotKeyUnRegistered += HotkeyMgr_Updated;
            byte[] pauseHotKey = null;

            //workaround for bug introduced last version
            try {
                var t = Base64Decode(config.Dict.PauseResumeHotKey);
                Console.WriteLine(t);
                pauseHotKey = System.Text.Encoding.Default.GetBytes(Base64Decode(config.Dict.PauseResumeHotKey)) ;
                Console.WriteLine(config.Dict.PauseResumeHotKey);
            }
            catch (InvalidCastException e)
            {
                Debug.WriteLine(e);
            }

            if (pauseHotKey != null && pauseHotKey.Length > 0)
            {
                var hotkey = GlobalHotKeyManager.HotKey.FromBytes(pauseHotKey);

                try
                {
                    hotkeyMgr.RegisterHotKey(ConfigKeys.PauseResumeHotKey, hotkey, null);
                }
                catch (InvalidOperationException e)
                {
                    Debug.WriteLine(e);

                    //ignore for now ?
                }

            }
        }

        private static void HotkeyMgr_Updated(string arg1, GlobalHotKeyManager.HotKey arg2)
        {
            UpdateTray();
        }

        static bool HotkeyMgr_HotKeyPreview(GlobalHotKeyManager mgr, string id, GlobalHotKeyManager.HotKey hk)
        {
            if (id == ConfigKeys.PauseResumeHotKey)
            {
                Debug.WriteLine("HotKey Pressed: " + hk);
                TogglePause();
                //menuItem_pause.Text = string.Format("{0} ({1})", gestureParser.IsPaused ? "继续" : "暂停" ,hk.ToString());

                return true; //Handled
            }

            return false;
        }

        static void TogglePause()
        {
            gestureParser.TogglePause();
        }

         /// <summary>
         /// 显示任务栏小图标
         /// </summary>
        static void ShowTrayIcon()
        {
            trayIcon = CreateNotifyIcon();
            EventHandler handleBalloon = (sender, args) =>
            {
                var timer = new Timer { Interval = 1000 };
                timer.Tick += (sender_1, args_1) =>
                {
                    timer.Stop();
                    trayIcon.Visible = config.Dict.TrayIconVisible ;
                };
                timer.Start();
            };

            trayIcon.BalloonTipClosed += handleBalloon;
            trayIcon.BalloonTipClicked += handleBalloon;
            trayIcon.DoubleClick += (sender, args) => ShowSettings();

            if (isFirstRun)
            {
                trayIcon.ShowBalloonTip(1000 * 10, "WGstures在这里", "双击图标打开设置，右击查看菜单", ToolTipIcon.Info);
            }
            else
            {
                var showIcon = config.Dict.TrayIconVisible ;
                if (!showIcon) //隐藏
                {
                    ToggleTrayIconVisibility();
                    //trayIcon.ShowBalloonTip(10* 1000, "WGestures图标将隐藏", "按 Shift+左键+中键 恢复\n再次运行WGestures可打开设置界面", ToolTipIcon.Info);
                }
            }
            //是否检查更新
            if ( config.Dict.AutoCheckForUpdate)
            {
                var checkForUpdateTimer = new Timer { Interval = Constants.AutoCheckForUpdateInterval };

                checkForUpdateTimer.Tick += (sender, args) =>
                {
                    checkForUpdateTimer.Stop();
                    ScheduledUpdateCheck(sender, trayIcon);

                };
                checkForUpdateTimer.Start();
            }

            UpdateTray();
        }

        #region event handlers
        static void menuItem_settings_Click(object sender, EventArgs eventArgs)
        {
            ShowSettings();
        }

        static void menuItem_pause_Click(object sender, EventArgs eventArgs)
        {
            TogglePause();
        }
        static void menuItem_restart_Click(object sender, EventArgs eventArgs)
        {
            Application.Restart();
        }
        static void menuItem_exit_Click(object sender, EventArgs e)
        {
            gestureParser.Stop();
            Application.ExitThread();
            trayIcon.Dispose();
        }


        #endregion

        /// <summary>
        /// 仅在启动一段时间后检查一次更新，
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tray"></param>
        static void ScheduledUpdateCheck(object sender, NotifyIcon tray)
        {
            if (!config.Dict.AutoCheckForUpdate ) return;

            var checker = new VersionChecker(AppSettings.CheckForUpdateUrl);
            checker.Finished += info =>
            {
                var whatsNew = info.WhatsNew.Length > 50 ? info.WhatsNew.Substring(0, 50) : info.WhatsNew;

                if (info.Version != Application.ProductVersion)
                {
                    tray.BalloonTipClicked += (o, args) =>
                    {
                        if (info.Version == Application.ProductVersion) return;
                        using (var frm = new UpdateInfoForm(ConfigurationManager.AppSettings.Get(Constants.ProductHomePageAppSettingKey), info))
                        {
                            frm.ShowDialog();
                            tray.Visible = config.Dict.TrayIconVisible ;
                        }
                    };
                    if (!tray.Visible)
                    {
                        tray.Visible = true;
                    }

                    tray.ShowBalloonTip(1000 * 15, Application.ProductName + "新版本可用!", "版本:" + info.Version + "\n" + whatsNew, ToolTipIcon.Info);
                }

                checker.Dispose();
                checker = null;

                GC.Collect();
            };
            checker.ErrorHappened += e =>
            {
                Debug.WriteLine("Program.ScheduledUpdateCheck Error:" + e.Message);
                checker.Dispose();
                checker = null;

                GC.Collect();
            };

            checker.CheckAsync();
        }


        static void ToggleTrayIconVisibility()
        {
            //如果图标当前可见， 而config中设置的值是不可见， 则说明是临时显示; 如果不是临时显示， 才需要修改config
            if (!(trayIcon.Visible && !config.Dict.TrayIconVisible ))
            {
                config.Dict.TrayIconVisible= !trayIcon.Visible ;
                config.Save();
            }

            if (trayIcon.Visible)
            {
                trayIcon.ShowBalloonTip(10 * 1000, "WGestures图标将隐藏", "按 Shift+左键+中键 恢复显示\n再次运行程序可打开设置界面", ToolTipIcon.Info);
            }
            else
            {
                trayIcon.Visible = true;
            }
        }

        static void ShowSettings()
        {
            if (settingsFormController != null)
            {
                settingsFormController.BringToFront();
                return;
            }
            using (settingsFormController = new SettingsFormController(config, gestureParser,
                (Win32MousePathTracker2)gestureParser.PathTracker, intentStore, gestureView, hotkeyMgr))
            {
                //进程如果优先为Hight，设置窗口上执行手势会响应非常迟钝（原因不明）
                //using (var proc = Process.GetCurrentProcess()) proc.PriorityClass = ProcessPriorityClass.Normal;
                settingsFormController.ShowDialog();
                //using (var proc = Process.GetCurrentProcess()) proc.PriorityClass = ProcessPriorityClass.High;
            }
            settingsFormController = null;
        }

        /// <summary>
        /// 用配置信息去同步自启动
        /// </summary>
          [SupportedOSPlatform("Windows")]
        static void SyncAutoStartState()
        {
            var fact = AutoStarter.IsRegistered(Constants.Identifier, Application.ExecutablePath);
            var conf = config.Dict.AutoStart;

            if (fact == conf && !isFirstRun) return;

            try
            {
                //可能被杀毒软件阻止
                if (conf)
                {
                    AutoStarter.Register(Constants.Identifier, Application.ExecutablePath);
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
        /// <summary>
        /// 是否在第一次展示quickstart
        /// </summary>
        /// <param name="isFirstRun"></param>
        static void ShowQuickStartGuide(bool isFirstRun = false)
        {
            var t = new Thread(() =>
            {
                var mut = new Mutex(true, Constants.Identifier + "QuickStartGuideWindow", out bool createdNew);
                if (!createdNew) return;

                using (var frm = new QuickStartGuideForm())
                {
                    Application.Run(frm);
                    mut.Close();
                }

                if (isFirstRun)
                {
                    //Open again to show settings
                    //Process.Start(Application.ExecutablePath);
                }

                GC.Collect();
            })
            { IsBackground = true };

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        static string GetPauseResumeHotkeyString()
        {
            var hk = hotkeyMgr.GetRegisteredHotKeyById(ConfigKeys.PauseResumeHotKey);

            if (hk != null) return hk.Value.ToString();

            return "";
        }

        static NotifyIcon CreateNotifyIcon()
        {
            var notifyIcon = new NotifyIcon();
            notifyIcon.Text = Application.ProductName + " " + Application.ProductVersion + " by YingDev.com";

            var contextMenu1 = new ContextMenuStrip();

            var menuItem_exit = new ToolStripMenuItem() { Text = "退出" };
            menuItem_exit.Click += menuItem_exit_Click;
            var menuItem_restart = new ToolStripMenuItem() { Text = "重启" };
            menuItem_restart.Click += menuItem_restart_Click;
            menuItem_pause = new ToolStripMenuItem() { Text = "暂停" };
            menuItem_pause.Click += menuItem_pause_Click;
          var  menuItem_resume = new ToolStripMenuItem() { Text = "解除按键死锁" };
            menuItem_resume.Click += menuItem_resume_Click;

            var menuItem_settings = new ToolStripMenuItem() { Text = "设置" };
            menuItem_settings.Click += menuItem_settings_Click;

            var menuItem_showQuickStart = new ToolStripMenuItem() { Text = "快速入门" };
            menuItem_showQuickStart.Click += (sender, args) => ShowQuickStartGuide();

            /*var menuItem_toggleTray = new MenuItem() { Text = "隐藏 (Shift + 左键 + 中键)" };
            menuItem_toggleTray.Click += (sender, args) =>
            {
               ToggleTrayIconVisibility();
            };*/

            contextMenu1.Items.AddRange(new ToolStripItem[] { /*menuItem_toggleTray, */menuItem_pause,menuItem_resume,menuItem_restart, new ToolStripSeparator(), menuItem_settings, menuItem_showQuickStart, new ToolStripSeparator(), menuItem_exit });
            notifyIcon.Icon = Resources.trayIcon;
            //notifyIcon.Text = Application.ProductName;
            notifyIcon.ContextMenuStrip = contextMenu1;
            notifyIcon.Visible = true;

            //todo: move out
            gestureParser.StateChanged += GestureParser_StateChanged;


            return notifyIcon;
        }

        private static void menuItem_resume_Click(object sender, EventArgs e)
        {
            Simulate.Events().Release( KeyCode.LWin).Wait(100).Invoke();
             
        }

        private static void GestureParser_StateChanged(GestureParser.State s)
        {
            UpdateTray();
        }

        private static void UpdateTray()
        {
            if (trayIcon == null) return;

            var hotKeyStr = GetPauseResumeHotkeyString();
            hotKeyStr = string.IsNullOrEmpty(hotKeyStr) ? "" : string.Format("({0})", hotKeyStr);
            if (gestureParser.IsPaused)
            {
                menuItem_pause.Text = "继续 " + hotKeyStr;
                trayIcon.Icon = Resources.trayIcon_bw;
            }
            else
            {
                menuItem_pause.Text = "暂停 " + hotKeyStr;
                trayIcon.Icon = Resources.trayIcon;
            }
        }

        static void Warning360Safe()
        {
            var proc360 = Process.GetProcessesByName("360Safe");
            var proc360Tray = Process.GetProcessesByName("360Tray");

            if (proc360.Length + proc360Tray.Length > 0)
            {
                using (var warn = new Warn360())
                {
                    warn.ShowDialog();
                }
            }
        }

        static void Dispose()
        {
            try
            {
                foreach (var disposable in componentsToDispose)
                {
                    if (disposable != null) disposable.Dispose();
                }

                componentsToDispose.Clear();
                Resources.ResourceManager.ReleaseAllResources();
            }
            finally
            {
                mutext.ReleaseMutex();
                Environment.Exit(1);
            }
        }
    }
}
