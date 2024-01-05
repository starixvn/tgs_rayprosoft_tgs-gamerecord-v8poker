using System.Text.Json.Nodes;
using Microsoft.Extensions.Options;
using Tgs.GameRecord.Handle.Client;
using Tgs.GameRecord.Handle.Common.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.V8Poker.Core.ApplicationServices;

internal class HandleRecordCommandService(
    IOptions<GameRecordInfoOptions> options,
    IGameRecordHandleClient client) : ICommandService<HandleRecordCommand>
{
    private readonly GameRecordInfoOptions _gameRecordInfo = options.Value;

    public ValueTask ExecuteAsync(HandleRecordCommand command, CancellationToken cancellationToken = default)
    {
        var wagerState = (command.Info.WinLoss.ToDecimal().GetValueOrDefault(0) > 0) ? WagerStates.Win : WagerStates.Lose;

        return client
        .MessageAsync(
            gameProviderCode: _gameRecordInfo.GameProviderCode,
            playerCode: command.Info.PlayerCode,
            wagerState: wagerState,
            wagerType: WagerTypes.Normal,
            freeGameState: FreeGameStates.None,
            gambleState: GambleStates.None,
            cashoutState: CashoutStates.None,
            resettleState: ResettleStates.None,
            externalRoundId: command.Info.ExternalWagerId,
            externalWagerId: command.Info.ExternalWagerId,
            externalGameId: command.Info.ExternalGameId,
            externalCreatedOn: command.Info.ExternalCreatedOn.ToDateTimeOffset(),
            externalClosedOn: command.Info.ExternalClosedOn.ToDateTimeOffset(),
            externalPayoffOn: command.Info.ExternalClosedOn.ToDateTimeOffset(),
            externalResettleOn: DateTimeOffset.UnixEpoch,
            bet: command.Info.Bet.ToDecimal().GetValueOrDefault(0),
            validBet: command.Info.ValidBet.ToDecimal().GetValueOrDefault(0),
            settle: command.Info.Settle.ToDecimal().GetValueOrDefault(0),
            commission: command.Info.Commission.ToDecimal().GetValueOrDefault(0),
            turnover: command.Info.ValidBet.ToDecimal().GetValueOrDefault(0),
            winLoss: command.Info.WinLoss.ToDecimal().GetValueOrDefault(0),
            additionalData: JsonNode.Parse(command.Info.AdditionalData)!,
            cancellationToken: cancellationToken);
    }
}
