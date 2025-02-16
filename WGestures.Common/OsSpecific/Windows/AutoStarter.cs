using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
 
using System.Runtime.Versioning;
 
using WindowsShortcutFactory;
namespace WGestures.Common.OsSpecific.Windows
{
    /// <summary>
    /// 注册开机自启
    /// </summary>
    public static class AutoStarter
    {
        //private const string RunLocation = @"Software\Microsoft\Windows\CurrentVersion\Run";
        /// <summary>
        /// 创建启动的快捷方式
        /// </summary>
        /// <param name="identifier">应用id</param>
        /// <returns>启动的快捷方式路径</returns>
        static string MakeShortcutPath(string identifier)
        {
            //C:\Users\yanni\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup
            return Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\" + identifier + ".lnk";
        }
        [SupportedOSPlatform("Windows")]
        public static void Register(string identifier, string appPath)
        {
            Unregister(identifier);
            CreateShortcut(MakeShortcutPath(identifier), appPath);

            //var key = Registry.CurrentUser.CreateSubKey(RunLocation);
            //key.SetValue(identifier, appPath);
            /*using (var ts = new Microsoft.Win32.TaskScheduler.TaskService())
            {
                var userId = WindowsIdentity.GetCurrent().Name;

                var task = ts.NewTask();
                task.RegistrationInfo.Description = identifier;
                task.Settings.DisallowStartIfOnBatteries = false;
                task.Settings.ExecutionTimeLimit = TimeSpan.Zero;
                task.Settings.Hidden = false;

                task.Principal.LogonType = TaskLogonType.InteractiveToken;
                task.Principal.UserId = userId;

                task.Principal.RunLevel = TaskRunLevel.Highest;
                task.Settings.Priority = ProcessPriorityClass.High;

                task.Triggers.Add(new LogonTrigger());
                task.Actions.Add(new ExecAction(appPath, "",workingDirectory));

                ts.RootFolder.RegisterTaskDefinition(identifier, task, 
                    TaskCreation.CreateOrUpdate, userId, 
                    LogonType: TaskLogonType.InteractiveToken);
            }*/
        }
        [SupportedOSPlatform("Windows")]
        public static void Unregister(string identifier)
        {
            System.IO.File.Delete(MakeShortcutPath(identifier));

            //ensure removing registry item added in older versions
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");

            key.DeleteValue(identifier,throwOnMissingValue: false);
            /* using (var ts = new TaskService())
             {
                 ts.RootFolder.DeleteTask(identifier);
             }*/
        }

        public static bool IsRegistered(string identifier,string appPath)
        {
            return System.IO.File.Exists(MakeShortcutPath(identifier));
            /*var key = Registry.CurrentUser.OpenSubKey(RunLocation);
            if (key == null)
                return false;

            var value = (string)key.GetValue(identifier);
            if (value == null)
                return false;

            return (value == appPath);*/
            /*
            using (var ts = new TaskService())
            {
                return ts.RootFolder.Tasks.Exists(identifier);
            }*/
        }
        /// <summary>
        /// 创建快捷方式
        /// </summary>
        /// <param name="shortcutPath">快捷方式路径</param>
        /// <param name="targetFileLocation">文件路径</param>
       [SupportedOSPlatform("Windows")]
        public static void CreateShortcut(string shortcutPath, string targetFileLocation)
        {
            try
            {
//                var exePath = Environment.ProcessPath;
//                var script = @$" 
//               $WshShell = New-Object -COMObject WScript.Shell;$Shortcut = $WshShell.CreateShortcut('Wgestures.lnk')
//$Shortcut.TargetPath =  '{exePath}'
//$Shortcut.Save()
//            ";

//                var powerShell = PowerShell.Create();
//                powerShell.AddScript(script);

//                foreach (var className in powerShell.Invoke())
//                {
//                    Trace.WriteLine(className);
//                }
                //var ps1File = @".\QuickStartGuide\createShort.ps1";
                //var startInfo = new ProcessStartInfo()
                //{
                //    FileName = "pwsh.exe",
                //    Arguments = $"-ExecutionPolicy Bypass -WindowStyle Hidden -NoProfile -file \"{ps1File}\"",
                //    UseShellExecute = false
                //};
                //var proc = Process.Start(startInfo);
                //在用户启动文件夹创建快捷方式C:\Users\yanni\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup
                //WshShell shell = new WshShell();
                //IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

                //shortcut.TargetPath = targetFileLocation;
                //shortcut.Save();
                using var shortcut = new WindowsShortcut
                {
                    Path = targetFileLocation,
                    Description = "wgestures eexe"
                };

                shortcut.Save(shortcutPath);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                
            }

                                
        }
    }
}
