namespace Tgs.GameRecord.V8Poker.Core;

public record GameRecordInfoOptions
{
    public string GameProviderCode { get; set; } = default!;
    public string GameProviderConfiguration { get; set; } = default!;
    public string HangfirePrefix { get; set; } = "gamerecord-v8-hangfire:";
    public string HangfireLocking { get; set; } = "gamerecord-v8-locking:";
    public double IntervalMinutes { get; set; } = 5;
    public double IntervalMinutesMax { get; set; } = 30;
    public double IntervalMinutesMinimum { get; set; } = 1;
}
