using System.ComponentModel;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;
using Windows.Win32.UI.WindowsAndMessaging;


namespace TrayIconTest;

public partial class ToolTipEx : Form
{
    [DefaultValue(null)]
    public string? Message
    {
        set => label1.Text = value;
    }

    public ToolTipEx()
    {
        InitializeComponent();
        SetSystemBackDrop(Handle);
        SetWindowCorner(Handle);
    }

    protected override CreateParams CreateParams
    {
        get
        {
            int WS_EX_TOPMOST = 0x00000008;
            int WS_EX_NOACTIVATE = 0x08000000;
            CreateParams cp = base.CreateParams;
            cp.ExStyle |= WS_EX_TOPMOST | WS_EX_NOACTIVATE;
            return cp;
        }
    }

    private void ToolTipEx_VisibleChanged(object sender, EventArgs e)
    {
        SetTopMost(Handle);
    }

    private static void SetSystemBackDrop(IntPtr hwnd)
    {
        unsafe
        {
            DWMWINDOWATTRIBUTE windowAttr = DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE;
            DWM_SYSTEMBACKDROP_TYPE backDropType = DWM_SYSTEMBACKDROP_TYPE.DWMSBT_TRANSIENTWINDOW;

            PInvoke.DwmSetWindowAttribute((HWND)hwnd, windowAttr, &backDropType, sizeof(int));
        }
    }

    private static void SetWindowCorner(IntPtr hwnd)
    {
        unsafe
        {
            DWMWINDOWATTRIBUTE windowAttr = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
            DWM_WINDOW_CORNER_PREFERENCE pref = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;

            PInvoke.DwmSetWindowAttribute((HWND)hwnd, windowAttr, &pref, sizeof(int));
        }
    }


    /// <summary>
    /// 
    /// </summary>
    private static void SetTopMost(IntPtr hwnd)
    {
        IntPtr HWND_TOPMOST = -1;
        SET_WINDOW_POS_FLAGS SWP_NOSIZE = SET_WINDOW_POS_FLAGS.SWP_NOSIZE;
        SET_WINDOW_POS_FLAGS SWP_NOMOVE = SET_WINDOW_POS_FLAGS.SWP_NOMOVE;
        SET_WINDOW_POS_FLAGS SWP_NOACTIVATE = SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE;

        PInvoke.SetWindowPos((HWND)hwnd, (HWND)HWND_TOPMOST, 0, 0, 0, 0,
            SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);

    }

}
