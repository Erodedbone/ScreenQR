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

if (!UrlHelper.TryGetHttpUrl("https://example.com/path?q=1", out _) ||
    UrlHelper.TryGetHttpUrl("file:///C:/Windows/System32/calc.exe", out _) ||
    UrlHelper.TryGetHttpUrl("https://user@example.com/", out _))
{
    Console.Error.WriteLine("FAIL: URL safety checks did not match the expected policy");
    return 1;
}

Console.WriteLine("PASS: single/multiple QR, blank image, DPI mapping, and URL policy tests succeeded");
return 0;
