using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tgs.ServiceUtility;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.V8Poker.Core.ApplicationServices;

internal class HandleRecordCommandService(
	ILogger<HandleRecordCommandService> logger,
	IOptions<GameRecordInfoOptions> options,
	IMessageSender messageSender) : ICommandService<HandleRecordCommand>
{
	private readonly GameRecordInfoOptions _gameRecordInfo = options.Value;

	public async ValueTask ExecuteAsync(HandleRecordCommand command, CancellationToken cancellationToken = default)
	{
		var betState = (command.Info.WinLoss.ToDecimal().GetValueOrDefault(0) > 0) ? Handle.Common.Protos.BetState.Win : Handle.Common.Protos.BetState.Loss;

		var request = new Handle.Common.Protos.GameRecordHandleRequest
		{
			GameProviderCode = _gameRecordInfo.GameProviderCode,
			PlayerCode = command.Info.PlayerCode,
			ExternalRoundId = command.Info.ExternalWagerId,
			ExternalBetId = command.Info.ExternalWagerId,
			ExternalGameId = command.Info.ExternalGameId,
			ExternalCreatedOn = command.Info.ExternalCreatedOn,
			ExternalClosedOn = command.Info.ExternalClosedOn,
			ExternalPayoffOn = command.Info.ExternalClosedOn,
			ExternalResettleOn = DateTimeOffset.UnixEpoch.ToTimestamp(),
			BetState = betState,
			BetType = Handle.Common.Protos.BetType.Normal,
			FreeGameState = Handle.Common.Protos.FreeGameState.None,
			GambleState = Handle.Common.Protos.GambleState.None,
			CashoutState = Handle.Common.Protos.CashoutState.None,
			ResettleState = Handle.Common.Protos.ResettleState.None,
			JackpotState = Handle.Common.Protos.JackpotState.None,
			Bet = command.Info.Bet.ToDecimal().GetValueOrDefault(0),
			ValidBet = command.Info.ValidBet.ToDecimal().GetValueOrDefault(0),
			Settle = command.Info.Settle.ToDecimal().GetValueOrDefault(0),
			Commission = command.Info.Commission.ToDecimal().GetValueOrDefault(0),
			Turnover = command.Info.ValidBet.ToDecimal().GetValueOrDefault(0),
			WinLoss = command.Info.WinLoss.ToDecimal().GetValueOrDefault(0),
			Jackpot = 0.0M,
			AdditionalData = command.Info.AdditionalData
		};

		await messageSender.PublishAsync(
			subject: Handle.Common.GameRecordHandleNatsConst.GameRecordHandleRequest,
			data: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		logger.LogInformation("LogOn:{logOn} Subject:{subject} Mesaage:This publication requests information.({request})",
			DateTime.Now.ToString("o"),
			Handle.Common.GameRecordHandleNatsConst.GameRecordHandleRequest,
			new
			{
				request.GameProviderCode,
				request.PlayerCode,
				request.ExternalRoundId,
				request.ExternalBetId,
				request.ExternalGameId,
				ExternalCreatedOn = request.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = request.ExternalClosedOn.ToDateTimeOffset(),
				request.BetState
			});
	}
}
