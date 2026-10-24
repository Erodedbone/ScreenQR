namespace ScreenQR;

internal sealed class HotkeyWindow : NativeWindow, IDisposable
{
    private const int HotkeyId = 0x5152;
    private readonly Action _onPressed;
    private HotkeyGesture? _current;
    internal bool IsRegistered => _current is not null;
    internal HotkeyGesture? Current => _current;

    internal HotkeyWindow(Action onPressed, HotkeyGesture gesture)
    {
        _onPressed = onPressed;
        CreateHandle(new CreateParams());
        TryRegister(gesture);
    }

    internal bool TryRegister(HotkeyGesture gesture)
    {
        if (!gesture.IsValid) return false;
        var previous = _current;
        if (previous is not null)
            NativeMethods.UnregisterHotKey(Handle, HotkeyId);

        if (NativeMethods.RegisterHotKey(Handle, HotkeyId,
                gesture.Modifiers | NativeMethods.ModNoRepeat, (uint)gesture.Key))
        {
            _current = gesture;
            return true;
        }

        _current = null;
        if (previous is not null && NativeMethods.RegisterHotKey(Handle, HotkeyId,
                previous.Modifiers | NativeMethods.ModNoRepeat, (uint)previous.Key))
            _current = previous;
        return false;
    }

    protected override void WndProc(ref Message message)
    {
        if (message.Msg == NativeMethods.WmHotkey && message.WParam.ToInt32() == HotkeyId)
            _onPressed();
        base.WndProc(ref message);
    }

    public void Dispose()
    {
        if (Handle != IntPtr.Zero)
        {
            if (_current is not null) NativeMethods.UnregisterHotKey(Handle, HotkeyId);
            DestroyHandle();
        }
    }
}
