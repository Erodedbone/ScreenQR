using System.Text.Json;

namespace ScreenQR;

internal sealed class AppSettings
{
    public uint HotkeyModifiers { get; set; } = HotkeyGesture.Default.Modifiers;
    public Keys HotkeyKey { get; set; } = HotkeyGesture.Default.Key;

    internal HotkeyGesture Hotkey => new(HotkeyModifiers, HotkeyKey);
}

internal static class SettingsStore
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };
    private static readonly string DirectoryPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ScreenQR");
    private static readonly string FilePath = Path.Combine(DirectoryPath, "settings.json");

    internal static AppSettings Load()
    {
        try
        {
            if (!File.Exists(FilePath)) return new AppSettings();
            var settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(FilePath));
            return settings is not null && settings.Hotkey.IsValid ? settings : new AppSettings();
        }
        catch
        {
            return new AppSettings();
        }
    }

    internal static void Save(AppSettings settings)
    {
        Directory.CreateDirectory(DirectoryPath);
        File.WriteAllText(FilePath, JsonSerializer.Serialize(settings, JsonOptions));
    }
}
