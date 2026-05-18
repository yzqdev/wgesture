using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core;

namespace WGestures.App.Views;

public partial class QuickStartGuideWindow : Window
{
    public QuickStartGuideWindow()
    {
        InitializeComponent();
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string htmlPath = Path.Combine(baseDir, "QuickStartGuide", "index.html");

            Debug.WriteLine(htmlPath);

            if (File.Exists(htmlPath))
            {
                
                await web_container.EnsureCoreWebView2Async(null);

                web_container.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
                web_container.CoreWebView2.Settings.AreDevToolsEnabled = false;
                web_container.CoreWebView2.Settings.IsStatusBarEnabled = false;
                // ✅ 顺手再关一次右键（保险）
                web_container.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
                web_container.Source = new Uri("file:///" + htmlPath);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        this.Activate();
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        bool isAltPressed =
            Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);

        bool isCtrlPressed =
            Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);

        bool isShiftPressed =
            Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

        try
        {
            if ((isAltPressed && e.Key == Key.Right) ||
                (isCtrlPressed && e.Key == Key.Tab && !isShiftPressed))
            {
                _ = web_container.CoreWebView2.ExecuteScriptAsync("performNext()");
                e.Handled = true;
            }
            else if ((isAltPressed && e.Key == Key.Left) ||
                     (isCtrlPressed && e.Key == Key.Tab && isShiftPressed))
            {
                _ = web_container.CoreWebView2.ExecuteScriptAsync("performPrev()");
                e.Handled = true;
            }
            else if (isCtrlPressed && e.Key == Key.W)
            {
                this.Close();
                e.Handled = true;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("JS调用失败: " + ex.Message);
        }
    }

    private void Web_container_CoreWebView2InitializationCompleted(
        object sender,
        CoreWebView2InitializationCompletedEventArgs e)
    {
        if (!e.IsSuccess)
        {
            Debug.WriteLine("WebView2 初始化失败: " + e.InitializationException);
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        web_container?.Dispose();
        base.OnClosed(e);
    }
}