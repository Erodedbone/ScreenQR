using System.Runtime.InteropServices;

namespace ScreenQR;

internal static class ClipboardHelper
{
    internal static void SetText(string text)
    {
        for (var attempt = 0; attempt < 5; attempt++)
        {
            try
            {
                Clipboard.SetText(text);
                return;
            }
            catch (ExternalException) when (attempt < 4)
            {
                Thread.Sleep(40 * (attempt + 1));
            }
        }
    }
}
