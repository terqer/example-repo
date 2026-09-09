using ChatGPTUsageWidget.Controls;
using ChatGPTUsageWidget.Services;
using System.Windows;
using System.Windows.Threading;

namespace ChatGPTUsageWidget;

public partial class App : Application
{
    public App()
    {
        DispatcherUnhandledException += OnDispatcherUnhandledException;
        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            CrashLogger.Write(e.Exception, "Unobserved task exception");
            e.SetObserved();
        };
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            if (e.ExceptionObject is Exception ex)
                CrashLogger.Write(ex, "Unhandled AppDomain exception");
        };
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        if (e.Args.Any(a => string.Equals(a, "--smoke-test", StringComparison.OrdinalIgnoreCase)))
        {
            RunSmokeTest();
            return;
        }

        try
        {
            MainWindow = new MainWindow();
            MainWindow.Show();
        }
        catch (Exception ex)
        {
            CrashLogger.Write(ex, "Application startup");
            MessageBox.Show(
                $"ChatGPT Usage Widget could not start.\n\n{ex.Message}\n\nLog: {CrashLogger.LogPath}",
                "ChatGPT Usage Widget",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Shutdown(1);
        }
    }

    private void RunSmokeTest()
    {
        try
        {
            var settings = new WidgetSettings();
            settings.Normalize();

            _ = new MainWindow();
            _ = new SettingsWindow(settings, false);

            var view = new UsageWidgetView();
            foreach (var mode in Enum.GetValues<WidgetDisplayMode>())
            {
                var variant = settings.Clone();
                variant.DisplayMode = mode;
                view.Configure(variant);
                var size = UsageWidgetView.GetBaseSize(variant);
                if (size.Width <= 0 || size.Height <= 0)
                    throw new InvalidOperationException($"Invalid base size for {mode}.");
            }

            Shutdown(0);
        }
        catch (Exception ex)
        {
            CrashLogger.Write(ex, "Smoke test");
            Shutdown(2);
        }
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        CrashLogger.Write(e.Exception, "Dispatcher exception");
        e.Handled = true;
        MessageBox.Show(
            $"An unexpected error occurred.\n\n{e.Exception.Message}\n\nLog: {CrashLogger.LogPath}",
            "ChatGPT Usage Widget",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
        Shutdown(1);
    }
}
