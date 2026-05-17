using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using WGestures.Common.Product;

// 彻底移除对 System.Windows.Forms 的依赖，转为纯 WPF 命名空间

namespace WGestures.App.Views
{
    public partial class UpdateInfoWindow : Window
    {
        private string _gotoUrl;

        // 1. 保留一个无参构造方法，防止 XAML 设计器或自动化工具报错
        public UpdateInfoWindow()
        {
            InitializeComponent();
        }

        // 2. 正确的带参构造方法（注意：方法名必须与类名 UpdateInfoWindow 严格一致）
        public UpdateInfoWindow(string gotoUrl, VersionInfo versionInfo) : this()
        {
            _gotoUrl = gotoUrl;
            
            // 注意：WPF 的 Label 使用 Content 属性，TextBlock/TextBox 使用 Text 属性
            lb_newVersion.Text = versionInfo.Version; 
            tb_whatsNew.Text = versionInfo.WhatsNew;

            // WPF 中获取当前程序版本号的正确标准写法
            lb_currentVersion.Text= Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0.0";
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void lnk_gotoUrl_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_gotoUrl))
            {
                var startInfo = new ProcessStartInfo("explorer.exe", _gotoUrl);
                using (Process.Start(startInfo)) { }
            }
            Close();
        }

        // 3. 用 WPF 专用的快捷键重写，替代 WinForms 的 ProcessCmdKey
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            // 监听 Ctrl + W 快捷键关闭窗口
            if (e.Key == Key.W && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                Close();
                e.Handled = true; // 标记事件已处理
            }
        }
    }
}