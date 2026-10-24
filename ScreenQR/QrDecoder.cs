using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;

namespace ScreenQR;

public static class QrDecoder
{
    public static string? Decode(Bitmap bitmap)
        => DecodeAll(bitmap).FirstOrDefault();

    public static IReadOnlyList<string> DecodeAll(Bitmap bitmap)
    {
        var reader = new ZXing.Windows.Compatibility.BarcodeReader
        {
            AutoRotate = true,
            Options = new DecodingOptions
            {
                TryHarder = true,
                PossibleFormats = new[] { BarcodeFormat.QR_CODE }
            }
        };

        var results = reader.DecodeMultiple(bitmap);
        if (results is null || results.Length == 0)
        {
            var single = reader.Decode(bitmap)?.Text;
            return string.IsNullOrWhiteSpace(single) ? Array.Empty<string>() : new[] { single };
        }

        return results
            .Select(result => result.Text)
            .Where(text => !string.IsNullOrWhiteSpace(text))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
    }
}
