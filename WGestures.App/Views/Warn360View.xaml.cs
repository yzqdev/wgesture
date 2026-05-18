using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace WGestures.App.Views;

public partial class Warn360View : Window {
    public Warn360View()
    {
        InitializeComponent();
       
    }

    private void Warn360_Load(object sender, RoutedEventArgs e)
    {
        // 1. 获取 WinForms 的 Icon 对象
        System.Drawing.Icon winformsIcon = WGestures.App.Properties.Resources.icon;

        if (winformsIcon != null)
        {
            // 2. 将 Icon 转换为 WPF 的 ImageSource 并赋值
            this.Icon = Imaging.CreateBitmapSourceFromHIcon(
                winformsIcon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );
        }
        tb_wgPath.Text = System.Environment.ProcessPath;
        Activate();
    }
 

    private void button1_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void ShowDetail_OnClick(object sender, RoutedEventArgs e)
    {
        Process.Start("http://tieba.baidu.com/p/3275239932");
    }
}