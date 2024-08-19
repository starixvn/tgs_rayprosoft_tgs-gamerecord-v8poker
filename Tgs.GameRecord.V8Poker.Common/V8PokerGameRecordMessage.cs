using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tgs.GameRecord.V8Poker.Common;

public record V8PokerGameRecordMessage
{
	[Required]
	[DataType(DataType.Url)]
	[DefaultValue(typeof(Uri), "https://wc-record.xpvcd046.com/")]
	public Uri ApiUrl { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(string), "C6995933499B163B")]
	public string Deskey { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(string), "628DA8F1857A6529")]
	public string Md5key { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(string), "71033")]
	public string AgentId { get; init; } = default!;
	[Required]
	[DataType(DataType.DateTime)]
	[DefaultValue(typeof(DateTimeOffset), "2023-07-26T11:50:00+08:00")]
	public DateTimeOffset StartTime { get; init; } = default!;
	[Required]
	[DataType(DataType.DateTime)]
	[DefaultValue(typeof(DateTimeOffset), "2023-07-26T11:50:00+08:00")]
	public DateTimeOffset EndTime { get; init; } = default!;
}
