using System.Drawing;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;
using ScreenQR;

const string expected = "https://example.com/screenqr-test?value=中文";
var writer = new BarcodeWriter
{
    Format = BarcodeFormat.QR_CODE,
    Options = new EncodingOptions
    {
        Width = 360,
        Height = 360,
        Margin = 2,
        Hints = { [EncodeHintType.CHARACTER_SET] = "UTF-8" }
    }
};
using Bitmap qr = writer.Write(expected);
var actual = QrDecoder.Decode(qr);

if (actual != expected)
{
    Console.Error.WriteLine($"FAIL: expected '{expected}', got '{actual ?? "<null>"}'");
    return 1;
}

using var blank = new Bitmap(100, 100);
if (QrDecoder.DecodeAll(blank).Count != 0)
{
    Console.Error.WriteLine("FAIL: blank image produced a false positive");
    return 1;
}

using var secondQr = writer.Write("plain text result");
using var combined = new Bitmap(qr.Width * 2 + 80, qr.Height);
using (var graphics = Graphics.FromImage(combined))
{
    graphics.Clear(Color.White);
    graphics.DrawImageUnscaled(qr, 0, 0);
    graphics.DrawImageUnscaled(secondQr, qr.Width + 80, 0);
}

var multiple = QrDecoder.DecodeAll(combined);
if (!multiple.Contains(expected) || !multiple.Contains("plain text result") || multiple.Count != 2)
{
    Console.Error.WriteLine($"FAIL: expected two distinct QR results, got {multiple.Count}");
    return 1;
}

var mapped = SelectionGeometry.MapToPixels(new Rectangle(100, 50, 200, 100),
    new Size(800, 450), new Size(1200, 675));
if (mapped != new Rectangle(150, 75, 300, 150))
{
    Console.Error.WriteLine($"FAIL: scaled selection mapped to {mapped}");
    return 1;
}

var clamped = SelectionGeometry.MapToPixels(new Rectangle(-10, -20, 900, 600),
    new Size(800, 450), new Size(1200, 675));
if (clamped != new Rectangle(0, 0, 1200, 675))
{
    Console.Error.WriteLine($"FAIL: out-of-bounds selection mapped to {clamped}");
    return 1;
}

if (!UrlHelper.TryGetHttpUrl("https://example.com/path?q=1", out _) ||
    UrlHelper.TryGetHttpUrl("file:///C:/Windows/System32/calc.exe", out _) ||
    UrlHelper.TryGetHttpUrl("https://user@example.com/", out _) ||
    UrlHelper.TryGetHttpUrl("javascript:alert(1)", out _) ||
    UrlHelper.TryGetHttpUrl("https://example.com/\nunsafe", out _))
{
    Console.Error.WriteLine("FAIL: URL safety checks did not match the expected policy");
    return 1;
}

using var resultForm = new ResultForm(new[] { expected, "plain text result" });
var resultList = resultForm.Controls.OfType<ListBox>().Single();
var openButton = resultForm.Controls.OfType<Button>().Single(button => button.Text == "打开链接");
var copyAllButton = resultForm.Controls.OfType<Button>().Single(button => button.Text == "复制全部");
if (resultList.Items.Count != 2 || !openButton.Enabled || copyAllButton.Text != "复制全部")
{
    Console.Error.WriteLine("FAIL: multiple-result form did not initialize correctly");
    return 1;
}

resultList.SelectedIndex = 1;
if (openButton.Enabled)
{
    Console.Error.WriteLine("FAIL: non-URL result left the open-link button enabled");
    return 1;
}

if (!HotkeyGesture.Default.IsValid || HotkeyGesture.Default.ToString() != "Alt + Win + Q")
{
    Console.Error.WriteLine("FAIL: default global hotkey is invalid or displayed unexpectedly");
    return 1;
}

Console.WriteLine("PASS: QR decoding, result UI, DPI mapping, URL policy, and hotkey checks succeeded");
return 0;
