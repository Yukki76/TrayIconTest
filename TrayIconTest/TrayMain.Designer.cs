using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;

namespace TrayIconTest;

partial class TrayMain
{
    private NotifyIcon _notifyIcon;
    private ContextMenuStrip _contextMainItem;
    private System.Windows.Forms.Timer _checkMouseTimer;


    private void InitializeComponent()
    {

        _notifyIcon = new NotifyIcon();
        _contextMainItem = new ContextMenuStrip();
        _checkMouseTimer = new System.Windows.Forms.Timer();

        //
        // NotifyIcon
        //
        _notifyIcon.Text = string.Empty;
        _notifyIcon.Icon = TrayIconTest.Properties.Resources.flac;
        _notifyIcon.ContextMenuStrip = _contextMainItem;
        _notifyIcon.Visible = true;
        _notifyIcon.MouseMove += NotifyIcon_MouseMove;
        //
        // ContextMenuStrip
        //
        _contextMainItem.Items.AddRange(
        [
            new ToolStripMenuItem("終了(&X)", null, ExitMenuItem_Click),
        ]);
        _contextMainItem.ShowImageMargin = false;
        //
        // CheckMouseTimer
        //
        _checkMouseTimer.Interval = 500;
        _checkMouseTimer.Tick += CheckMouseTimer_Tick;
    }
}
