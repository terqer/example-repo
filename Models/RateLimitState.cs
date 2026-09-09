namespace ChatGPTUsageWidget.Models;

public sealed record RateLimitWindowState(
    int UsedPercent,
    int RemainingPercent,
    long? WindowDurationMins,
    DateTimeOffset? ResetsAt);

public sealed record RateLimitState(
    RateLimitWindowState? FiveHour,
    RateLimitWindowState? Weekly,
    string? PlanType,
    long? ResetCredits,
    DateTimeOffset UpdatedAt,
    string? Error = null);
