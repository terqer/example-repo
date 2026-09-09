using System.IO;
using System.Text.Json;

namespace ChatGPTUsageWidget.Services;

public sealed class SettingsService
{
    private readonly string _folder;

    public SettingsService(string? folder = null)
    {
        _folder = folder ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ChatGPTUsageWidget");
    }

    public string FilePath => Path.Combine(_folder, "settings.json");

    public WidgetSettings Load()
    {
        try
        {
            if (!File.Exists(FilePath))
                return new WidgetSettings();

            var json = File.ReadAllText(FilePath);
            var settings = JsonSerializer.Deserialize<WidgetSettings>(json) ?? new WidgetSettings();
            settings.Normalize();
            return settings;
        }
        catch
        {
            return new WidgetSettings();
        }
    }

    public void Save(WidgetSettings settings)
    {
        try
        {
            settings.Normalize();
            Directory.CreateDirectory(_folder);
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
        catch
        {
            // Settings persistence should never take the widget down.
        }
    }
}
