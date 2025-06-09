using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public static class GlobalKeyListener
{
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_SYSKEYDOWN = 0x0104;

    public static event EventHandler<KeyEventArgs> KeyPressed;

    static GlobalKeyListener()
    {
        HookKeyboard();
    }

    private static void HookKeyboard()
    {
        Application.AddMessageFilter(new KeyboardMessageFilter());
    }

    private class KeyboardMessageFilter : IMessageFilter
    {
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN || m.Msg == WM_SYSKEYDOWN)
            {
                var keyCode = (Keys)m.WParam.ToInt32();
                OnKeyPressed(new KeyEventArgs(keyCode));
            }
            return false;
        }
    }

    private static void OnKeyPressed(KeyEventArgs e)
    {
        KeyPressed?.Invoke(null, e);
    }
}
