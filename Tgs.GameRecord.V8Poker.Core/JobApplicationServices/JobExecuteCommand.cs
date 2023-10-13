namespace Tgs.GameRecord.V8Poker.Core.JobApplicationServices;

public record JobExecuteCommand(
    Uri ApiUrl,
    string Deskey,
    string Md5key,
    string AgentId);