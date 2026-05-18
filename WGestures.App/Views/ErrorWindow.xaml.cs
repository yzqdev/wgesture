using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using Application = System.Windows.Forms.Application;
using Clipboard = System.Windows.Forms.Clipboard;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WGestures.App.Gui.Windows;

public partial class ErrorWindow : Window {
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string ErrorText
    {
        get { return tb_Detail.Text; }
        set { tb_Detail.Text = GetProductInfo() + value; }
    }

    public ErrorWindow()
    {
        InitializeComponent();


        /*tb_Detail.MouseEnter += (sender, args) =>
        {
            tb_Detail.Focus();
            tb_Detail.SelectAll();
        };

        tb_mail.MouseEnter += (sender, args) =>
        {
            tb_mail.Focus();
            tb_mail.SelectAll();
        };*/
    }

    private void btn_close_Click(object sender, System.EventArgs e)
    {
        Close();
        Environment.Exit(1);
    }



    private static string GetProductInfo()
    {
        // 1. 获取 WPF 程序的版本号 (对应原来的 ProductVersion)
        // 现代 .NET 推荐直接读取主程序集的 InformationalVersion 或 Version
        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion 
                      ?? assembly.GetName().Version?.ToString() 
                      ?? "Unknown";

        // 2. 获取程序运行的绝对路径 (对应原来的 ExecutablePath)
        // AppContext.BaseDirectory 获取程序运行目录，结合进程名还原完整路径
        string appPath = Environment.ProcessPath ?? Path.Combine(AppContext.BaseDirectory, AppDomain.CurrentDomain.FriendlyName + ".exe");

        // 3. 获取操作系统版本
        // 使用 RuntimeInformation 能比 Environment.OSVersion 更准确地在现代 Windows 上识别系统信息
        string osDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription;

        // 使用 C# 的字符串内插 ($) 让代码更干净可读
        return $"WGestures Version: {version}\r\n" +
               $"OS: {osDescription}\r\n" +
               $"AppPath: {appPath}\r\n" +
               "=======================\r\n";
    }
    private static string GetProductInfoWinform()
    {
        return "WGestures Version:" + 
               System.Windows.Forms.Application.ProductVersion + "\r\nOS:" + 
               Environment.OSVersion + "\r\nAppPath:" + 
               Application.ExecutablePath + "\r\n=======================\r\n";
    }

    private void copy_error_Click(object sender, EventArgs e)
    {
        Clipboard.SetText(tb_Detail.Text);
        MessageBox.Show("复制成功!","信息");
              
    }
}