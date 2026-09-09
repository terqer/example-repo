namespace ChatGPTUsageWidget.Services;

public enum WidgetDisplayMode
{
    ClassicBars = 0,
    CompactCards = 1,
    MinimalPill = 2,
    CircularRings = 3,
    Speedometers = 4
}

public sealed class WidgetSettings
{
    public bool HasPosition { get; set; }
    public double Left { get; set; }
    public double Top { get; set; }
    public bool AlwaysOnTop { get; set; } = true;
    public bool SnapToEdges { get; set; } = true;
    public double Opacity { get; set; } = 0.94;
    public double Scale { get; set; } = 1.0;
    public int RefreshSeconds { get; set; } = 60;
    public WidgetDisplayMode DisplayMode { get; set; } = WidgetDisplayMode.ClassicBars;

    public bool ShowFiveHour { get; set; } = true;
    public bool ShowWeekly { get; set; } = true;
    public bool ShowResetTime { get; set; } = true;
    public bool ShowResetDate { get; set; } = true;
    public bool ShowCurrentTime { get; set; }
    public bool ShowCurrentDate { get; set; }
    public bool ShowResetCredits { get; set; } = true;
    public bool ShowStatusDot { get; set; } = true;

    public void Normalize()
    {
        Opacity = double.IsFinite(Opacity) ? Math.Clamp(Opacity, 0.60, 1.00) : 0.94;
        Scale = double.IsFinite(Scale) ? Math.Clamp(Scale, 0.70, 1.80) : 1.0;
        RefreshSeconds = Math.Clamp(RefreshSeconds, 30, 600);

        if (!Enum.IsDefined(DisplayMode))
            DisplayMode = WidgetDisplayMode.ClassicBars;

        if (!double.IsFinite(Left) || !double.IsFinite(Top))
        {
            HasPosition = false;
            Left = 0;
            Top = 0;
        }
    }

    public WidgetSettings Clone()
    {
        var clone = new WidgetSettings
        {
            HasPosition = HasPosition,
            Left = Left,
            Top = Top,
            AlwaysOnTop = AlwaysOnTop,
            SnapToEdges = SnapToEdges,
            Opacity = Opacity,
            Scale = Scale,
            RefreshSeconds = RefreshSeconds,
            DisplayMode = DisplayMode,
            ShowFiveHour = ShowFiveHour,
            ShowWeekly = ShowWeekly,
            ShowResetTime = ShowResetTime,
            ShowResetDate = ShowResetDate,
            ShowCurrentTime = ShowCurrentTime,
            ShowCurrentDate = ShowCurrentDate,
            ShowResetCredits = ShowResetCredits,
            ShowStatusDot = ShowStatusDot
        };
        clone.Normalize();
        return clone;
    }
}
