using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WGestures.Common.Product;

namespace WGestures.App;

internal static class AppSettings
{
    public static string CheckForUpdateUrl
    {
        get
        {

#if DEBUG
            return ConfigurationManager.AppSettings.Get(Constants.CheckForUpdateUrlAppSettingKey);// "http://localhost:1226/projects/latestVersion?product=WGestures";

#else
                return ConfigurationManager.AppSettings.Get(Constants.CheckForUpdateUrlAppSettingKey);

#endif
        }
    }

    public static string ProductHomePage
    {
        get { return ConfigurationManager.AppSettings.Get(Constants.ProductHomePageAppSettingKey); }
    }

    public static string UserDataDirectory
    {
        //C:\Users\UserName\AppData\Local\CompanyName\ProductName\ProductVersion
        //C:\Users\yanni\AppData\Local\YingDev.com\WGestures\1.8.4.0
        get { return Application.LocalUserAppDataPath; }
    }


    /// <summary>
    /// cd "$env:USERPROFILE\AppData\Local\YingDev.com\WGestures\1.8.4.0"
    /// </summary>
    public static string ConfigFilePath
    {
        get { return UserDataDirectory + @"\config.json"; }
    }
    /// <summary>
    /// cd "$env:USERPROFILE\AppData\Local\YingDev.com\WGestures\1.8.4.0"
    /// </summary>
    public static string GesturesFilePath
    {
        get { return UserDataDirectory + @"\gestures.wg2"; }
    }

    public static string DefaultUserGesturesFilePath
    {
        get { return Application.StartupPath + $@"\defaults\{UserConfigGestureFileName}"; }
    }    public static string DefaultGesturesFilePath
    {
        get { return Application.StartupPath + $@"\defaults\{DefaultConfigGestureFileName}"; }
    }

    public static string DefaultConfigGestureFileName = "default_gestures.wg2";
    public static string UserConfigGestureFileName = "gestures.wg2";

    public static string ConfigFileVersion
    {
        get { return "1"; }
    }

    public static string GesturesFileVersion
    {
        get { return "3"; }
    }
        
}