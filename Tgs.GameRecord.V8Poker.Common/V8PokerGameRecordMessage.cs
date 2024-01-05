using System.ComponentModel.DataAnnotations;

namespace Tgs.GameRecord.V8Poker.Common;

public record V8PokerGameRecordMessage
{
    [Required]
    [DataType(DataType.Url)]
    public Uri ApiUrl { get; init; } = default!;
    [Required]
    public string Deskey { get; init; } = default!;
    [Required]
    public string Md5key { get; init; } = default!;
    [Required]
    public string AgentId { get; init; } = default!;
    [Required]
    [DataType(DataType.DateTime)]
    public DateTimeOffset StartTime { get; init; } = default!;
    [Required]
    [DataType(DataType.DateTime)]
    public DateTimeOffset EndTime { get; init; } = default!;
}
