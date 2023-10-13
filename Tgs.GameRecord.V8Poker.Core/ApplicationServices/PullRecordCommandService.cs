using Microsoft.Extensions.Logging;
using Tgs.GameInterface.V8Poker.Client;
using Tgs.GameInterface.V8Poker.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.V8Poker.Core.ApplicationServices;

internal class PullRecordCommandService : ICommandService<PullRecordCommand>
{
    private readonly IGameInterfaceV8PokerClient _client;
    private readonly ILogger<PullRecordCommandService> _logger;
    private readonly ICommandService<HandleRecordCommand> _service;

    public PullRecordCommandService(
        ILogger<PullRecordCommandService> logger,
        IGameInterfaceV8PokerClient client,
        ICommandService<HandleRecordCommand> service)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async ValueTask ExecuteAsync(PullRecordCommand command, CancellationToken cancellationToken = default)
    {
        var infos = await _client.GameWagerAsync(
            apiUrl: command.ApiUrl,
            deskey: command.Deskey,
            md5key: command.Md5key,
            agentId: command.AgentId,
            startTime: command.StartTime,
            endTime: command.EndTime,
            cancellationToken: cancellationToken)
            .ToArrayAsync(
            cancellationToken: cancellationToken)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameWagerInfo>();

        foreach (var info in infos)
        {
            try
            {
                await _service
                .ExecuteAsync(
                     command: new HandleRecordCommand(info),
                     cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogOn:{logOn} Message: An exception occurred in this game record.({info})", DateTime.Now.ToString("o"), info);
            }
        }
    }
}
