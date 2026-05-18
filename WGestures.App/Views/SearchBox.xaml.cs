using System;
using System.Windows;

namespace WGestures.App.Gui.Windows;

public partial class SearchBox : Window {
    public SearchBox()
    {
        InitializeComponent();
    }
    // 对应 WinForms 的 SearchBox_Deactivate
    private void SearchBox_Deactivated(object sender, EventArgs e)
    {
        // 你的失去焦点逻辑
    }

    // 对应 WinForms 的 SearchBox_Shown
    private void SearchBox_Activated(object sender, EventArgs e)
    {
        // 你的窗口显示时的逻辑
        // 比如自动聚焦输入框：txt_content.Focus();
    }
    private void SearchBox_Shown(object sender, System.EventArgs e)
    {
        Activate();
    }

    public void SetSearchText(string txt)
    {
        txt_content.Text = txt;
        txt_content.SelectAll();
    }

    private void SearchBox_Deactivate(object sender, System.EventArgs e)
    {
        Close();
    }
}