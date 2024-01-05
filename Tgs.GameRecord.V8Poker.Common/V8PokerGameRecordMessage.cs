namespace Tgs.GameRecord.V8Poker.Common;

public record V8PokerGameRecordMessage
{
    public Uri ApiUrl { get; init; } = default!;
    public string Deskey { get; init; } = default!;
    public string Md5key { get; init; } = default!;
    public string AgentId { get; init; } = default!;
    public DateTimeOffset StartTime { get; init; } = default!;
    public DateTimeOffset EndTime { get; init; } = default!;
}
