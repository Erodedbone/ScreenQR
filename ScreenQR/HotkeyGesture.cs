namespace ScreenQR;

internal sealed record HotkeyGesture(uint Modifiers, Keys Key)
{
    internal static HotkeyGesture Default { get; } =
        new(NativeMethods.ModWin | NativeMethods.ModAlt, Keys.Q);

    internal bool IsValid => Key != Keys.None &&
        (Modifiers & (NativeMethods.ModControl | NativeMethods.ModAlt |
                      NativeMethods.ModShift | NativeMethods.ModWin)) != 0;

    public override string ToString()
    {
        var parts = new List<string>();
        if ((Modifiers & NativeMethods.ModControl) != 0) parts.Add("Ctrl");
        if ((Modifiers & NativeMethods.ModAlt) != 0) parts.Add("Alt");
        if ((Modifiers & NativeMethods.ModShift) != 0) parts.Add("Shift");
        if ((Modifiers & NativeMethods.ModWin) != 0) parts.Add("Win");
        parts.Add(Key.ToString());
        return string.Join(" + ", parts);
    }
}
