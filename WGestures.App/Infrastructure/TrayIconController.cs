using System;
using System.Reflection;
using System.Windows.Forms;
using WGestures.App.Properties;
using WGestures.Common.Config;

namespace WGestures.App.Infrastructure;

/// <summary>
/// 托盘图标与右键菜单（暂停/解除按键死锁/重启/设置/快速入门/退出）。
/// 图标与菜单文案随暂停状态刷新；刷新时机由 App 接线 GestureParser.StateChanged 等事件驱动。
/// </summary>
internal class TrayIconController : IDisposable
{
    private readonly NotifyIcon _notifyIcon = new NotifyIcon();
    private readonly ToolStripMenuItem _menuItemPause;
    private readonly PlistConfig _config;
    private readonly Func<bool> _isPaused;
    private readonly Func<string> _pauseHotkeyString;

    public TrayIconController(PlistConfig config, Func<bool> isPaused, Func<string> pauseHotkeyString,
        Action togglePause, Action showSettings, Action showQuickStart,
        Action releaseKeyLock, Action restart, Action exit)
    {
        _config = config;
        _isPaused = isPaused;
        _pauseHotkeyString = pauseHotkeyString;

        var asmName = Assembly.GetExecutingAssembly().GetName();
        _notifyIcon.Text = asmName.Name + " " + asmName.Version;

        _menuItemPause = new ToolStripMenuItem { Text = "暂停" };
        _menuItemPause.Click += (s, e) => togglePause();

        var menuItemResume = new ToolStripMenuItem { Text = "解除按键死锁" };
        menuItemResume.Click += (s, e) => releaseKeyLock();

        var menuItemRestart = new ToolStripMenuItem { Text = "重启" };
        menuItemRestart.Click += (s, e) => restart();

        var menuItemSettings = new ToolStripMenuItem { Text = "设置" };
        menuItemSettings.Click += (s, e) => showSettings();

        var menuItemShowQuickStart = new ToolStripMenuItem { Text = "快速入门" };
        menuItemShowQuickStart.Click += (s, e) => showQuickStart();

        var menuItemExit = new ToolStripMenuItem { Text = "退出" };
        menuItemExit.Click += (s, e) => exit();

        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.AddRange(new ToolStripItem[]
        {
            _menuItemPause, menuItemResume, menuItemRestart, new ToolStripSeparator(),
            menuItemSettings, menuItemShowQuickStart, new ToolStripSeparator(), menuItemExit
        });

        _notifyIcon.Icon = Resources.trayIcon;
        _notifyIcon.ContextMenuStrip = contextMenu;
        _notifyIcon.Visible = true;
    }

    /// <summary>按当前暂停状态与暂停热键刷新菜单文案与图标（彩色/灰度）。</summary>
    public void UpdatePauseState()
    {
        var hotKeyStr = _pauseHotkeyString();
        hotKeyStr = string.IsNullOrEmpty(hotKeyStr) ? "" : $"({hotKeyStr})";

        if (_isPaused())
        {
            _menuItemPause.Text = "继续 " + hotKeyStr;
            _notifyIcon.Icon = Resources.trayIcon_bw;
        }
        else
        {
            _menuItemPause.Text = "暂停 " + hotKeyStr;
            _notifyIcon.Icon = Resources.trayIcon;
        }
    }

    /// <summary>手势触发的托盘图标隐藏/恢复（Shift+左键+中键），状态写入配置。</summary>
    public void ToggleVisibility()
    {
        if (!(_notifyIcon.Visible && !_config.Dict.TrayIconVisible))
        {
            _config.Dict.TrayIconVisible = !_notifyIcon.Visible;
            _config.Save();
        }

        if (_notifyIcon.Visible)
        {
            _notifyIcon.ShowBalloonTip(10 * 1000, "WGestures图标将隐藏", "按 Shift+左键+中键 恢复显示\n再次运行程序可打开设置界面", ToolTipIcon.Info);
        }
        else
        {
            _notifyIcon.Visible = true;
        }
    }

    public void Dispose()
    {
        _notifyIcon.Dispose();
    }
}
