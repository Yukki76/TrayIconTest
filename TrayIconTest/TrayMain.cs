using System.Reflection;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Shell;

namespace TrayIconTest;

public partial class TrayMain : ApplicationContext, IDisposable
{    private readonly ToolTipEx _toolTipEx = new();

    public TrayMain()
    {
        InitializeComponent();
    }

    private void NotifyIcon_MouseMove(object? sender, MouseEventArgs e)
    {
        if (_contextMainItem.Visible)
        {
            _checkMouseTimer.Stop();
            return;
        }
        _checkMouseTimer.Start();
    }

    private void CheckMouseTimer_Tick(object? sender, EventArgs e)
    {
        if (IsNotifyIconHovered())
        {
            if (_toolTipEx.Visible || _contextMainItem.Visible) return;

            Rectangle iconRect = GetNotifyIconRect();
            int iconMiddlePosition = iconRect.Left + ((iconRect.Right - iconRect.Left) / 2);
            _toolTipEx.Location = new Point(iconMiddlePosition - (_toolTipEx.Width / 2),
                iconRect.Top - _toolTipEx.Height - 20);
            _toolTipEx.Message = $"Text: {_notifyIcon.Text}\nLength: {_notifyIcon.Text.Length}";
            _toolTipEx.Show();
            _toolTipEx.Activate();
        }
        else
        {
            _toolTipEx.Hide();
            _checkMouseTimer?.Stop();
        }

    }

    private void ExitMenuItem_Click(object? sender, EventArgs e)
    {
        ExitThread();
    }

    /// <summary>
    /// タスクトレイアイコン上にアイコンがあるかの判定
    /// </summary>
    /// <returns></returns>
    private bool IsNotifyIconHovered()
    {
        // マウス位置に合わせてツールチップを表示
        Point mousePos = Cursor.Position;
        Rectangle iconRect = GetNotifyIconRect();

        if (iconRect != Rectangle.Empty && !iconRect.Contains(mousePos))
        {
            // マウスカーソルがアイコンから外れた処理
            return false;
        }
        // マウスカーソルがアイコン上にまだある
        return true;
    }

    /// <summary>
    ///  タスクトレイアイコンの位置
    /// </summary>
    /// <returns></returns>
    private Rectangle GetNotifyIconRect()
    {
        var windowField = typeof(NotifyIcon).GetField(
            "_window", BindingFlags.NonPublic | BindingFlags.Instance);
        var windowValue = windowField?.GetValue(_notifyIcon);

        var idField = typeof(NotifyIcon).GetField(
            "_id", BindingFlags.NonPublic | BindingFlags.Instance);
        var idValue = idField?.GetValue(_notifyIcon);

        if (windowValue is NativeWindow nativeWindow && idValue is uint realUID)
        {
            NOTIFYICONIDENTIFIER id = new()
            {
                cbSize = (uint)Marshal.SizeOf<NOTIFYICONIDENTIFIER>(),
                hWnd = (HWND)nativeWindow.Handle,
                uID = realUID
            };
            if (PInvoke.Shell_NotifyIconGetRect(id, out RECT rect) == 0)
            {
                return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
            }
        }
        return Rectangle.Empty;
    }
}

