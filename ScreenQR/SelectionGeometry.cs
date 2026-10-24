namespace ScreenQR;

public static class SelectionGeometry
{
    public static Rectangle MapToPixels(Rectangle selection, Size overlaySize, Size bitmapSize)
    {
        if (selection.IsEmpty || overlaySize.Width <= 0 || overlaySize.Height <= 0 ||
            bitmapSize.Width <= 0 || bitmapSize.Height <= 0)
            return Rectangle.Empty;

        var scaleX = (double)bitmapSize.Width / overlaySize.Width;
        var scaleY = (double)bitmapSize.Height / overlaySize.Height;
        var left = Math.Clamp((int)Math.Floor(selection.Left * scaleX), 0, bitmapSize.Width);
        var top = Math.Clamp((int)Math.Floor(selection.Top * scaleY), 0, bitmapSize.Height);
        var right = Math.Clamp((int)Math.Ceiling(selection.Right * scaleX), left, bitmapSize.Width);
        var bottom = Math.Clamp((int)Math.Ceiling(selection.Bottom * scaleY), top, bitmapSize.Height);
        return Rectangle.FromLTRB(left, top, right, bottom);
    }
}
