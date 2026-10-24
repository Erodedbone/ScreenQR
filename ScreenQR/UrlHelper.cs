using System.Diagnostics;
using System.ComponentModel;

namespace ScreenQR;

public static class UrlHelper
{
    public static bool TryGetHttpUrl(string text, out Uri? uri)
    {
        var candidate = text.Trim();
        if (candidate.Length <= 4096 &&
            candidate.All(character => !char.IsControl(character)) &&
            Uri.TryCreate(candidate, UriKind.Absolute, out uri) &&
            (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps) &&
            !string.IsNullOrWhiteSpace(uri.Host) &&
            string.IsNullOrEmpty(uri.UserInfo))
            return true;
        uri = null;
        return false;
    }

    internal static void ConfirmAndOpen(IWin32Window owner, Uri uri)
    {
        var message = $"即将在默认浏览器中打开：\n\n{uri.AbsoluteUri}\n\n主机：{uri.IdnHost}\n\n请确认你信任此链接。";
        if (MessageBox.Show(owner, message, "确认打开链接", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.OK)
            return;

        try
        {
            Process.Start(new ProcessStartInfo(uri.AbsoluteUri) { UseShellExecute = true });
        }
        catch (Exception exception) when (exception is Win32Exception or InvalidOperationException)
        {
            MessageBox.Show(owner, $"无法打开链接：{exception.Message}", "ScreenQR",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
