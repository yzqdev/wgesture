
namespace WGestures.App
{
    internal static class Constants
    {
        /// <summary>
        /// 软件唯一id
        /// </summary>
        public const string Identifier = "com.yingdev.WGestures";
        public const string CheckForUpdateUrlAppSettingKey = "CheckForUpdateUrl";

        public const string ProductHomePageAppSettingKey = "ProductHomePage";

#if DEBUG
        public const int AutoCheckForUpdateInterval = 1000 * 3;
#else 
        public const int AutoCheckForUpdateInterval = 1000*30;
#endif
    }
}
