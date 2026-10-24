using System.Threading;

namespace ScreenQR;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        using var singleInstance = new Mutex(true, "Local\\ScreenQR.SingleInstance", out var createdNew);
        if (!createdNew)
        {
            MessageBox.Show("ScreenQR 已在运行。请使用已设置的快捷键或托盘图标扫描。", "ScreenQR",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new TrayApplicationContext());
    }
}
