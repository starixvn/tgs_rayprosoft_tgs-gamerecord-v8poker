namespace Tgs.GameRecord.V8Poker.Core.ApplicationServices;

public record PullRecordCommand(
    Uri ApiUrl,
    string Deskey,
    string Md5key,
    string AgentId,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
