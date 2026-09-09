using System.IO;
using System.Text;

namespace ChatGPTUsageWidget.Services;

public static class CrashLogger
{
    private static readonly string Folder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "ChatGPTUsageWidget");

    public static string LogPath => Path.Combine(Folder, "widget.log");

    public static void Write(Exception exception, string context)
    {
        try
        {
            Directory.CreateDirectory(Folder);
            var text = new StringBuilder()
                .AppendLine($"[{DateTimeOffset.Now:O}] {context}")
                .AppendLine(exception.ToString())
                .AppendLine(new string('-', 80))
                .ToString();
            File.AppendAllText(LogPath, text);
        }
        catch
        {
        }
    }
}
