namespace ScreenQR;

internal static class AppIcon
{
    internal static Icon Create()
    {
        var path = Environment.ProcessPath ?? Application.ExecutablePath;
        return Icon.ExtractAssociatedIcon(path) ?? (Icon)SystemIcons.Application.Clone();
    }
}
