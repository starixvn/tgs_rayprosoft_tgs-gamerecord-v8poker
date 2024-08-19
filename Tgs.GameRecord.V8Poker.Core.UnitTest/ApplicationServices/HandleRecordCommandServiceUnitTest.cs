using System.Text.Json.Nodes;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Tgs.GameInterface.V8Poker.Protos;
using Tgs.GameRecord.Handle.Client;
using Tgs.GameRecord.V8Poker.Core.ApplicationServices;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.V8Poker.Core.UnitTest.ApplicationServices;

public class HandleRecordCommandServiceUnitTest
{
	[Fact]
	public async Task Loss()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "V8",
			GameProviderConfiguration = "V8Configuration",
			HangfirePrefix = "gamerecord-v8-hangfire:",
			HangfireLocking = "gamerecord-v8-locking:",
			IntervalMinutes = 5,
			IntervalMinutesMax = 30,
			IntervalMinutesMinimum = 1
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

		var command = new HandleRecordCommand(
			Info: new GameWagerInfo
			{
				ExternalWagerId = "50-1690343466-360242382-3",
				ExternalGameId = "910",
				PlayerCode = "4ucmqplxb029",
				ExternalCreatedOn = DateTimeOffset.Parse("2023-07-26T11:51:06+08:00").ToTimestamp(),
				ExternalClosedOn = DateTimeOffset.Parse("2023-07-26T11:51:29+08:00").ToTimestamp(),
				Bet = 8.00m,
				ValidBet = 8.00m,
				Settle = 7.80m,
				Commission = 0.00m,
				WinLoss = -0.20m,
				AdditionalData = new JsonObject
				{
					["GameID"] = "50-1690343466-360242382-3",
					["Accounts"] = "72412_4ucmqplxb029",
					["ServerID"] = 9101,
					["KindID"] = 910,
					["TableID"] = 182020001,
					["ChairID"] = 0,
					["UserCount"] = 1,
					["CellScore"] = 0.00m,
					["AllBet"] = 8.00m,
					["Profit"] = -0.20m,
					["Revenue"] = 0,
					["GameStartTime"] = "2023-07-26 11:51:06",
					["GameEndTime"] = "2023-07-26 11:51:29",
					["CardValue"] = "1b3c263b07002",
					["ChannelID"] = 72412,
					["LineCode"] = "72412_GPI"
				}.ToJsonString()
			});

		var service = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await service.ExecuteAsync(command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.Info.PlayerCode,
				ExternalRoundId = command.Info.ExternalWagerId,
				ExternalBetId = command.Info.ExternalWagerId,
				ExternalGameId = command.Info.ExternalGameId,
				ExternalCreatedOn = command.Info.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = command.Info.ExternalClosedOn.ToDateTimeOffset(),
				ExternalPayoffOn = command.Info.ExternalClosedOn.ToDateTimeOffset(),
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Loss,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 8.00m,
				ValidBet = 8.00m,
				Settle = 7.80m,
				Commission = 0.0M,
				Turnover = 8.00m,
				WinLoss = -0.20m,
				Jackpot = 0.0M,
				AdditionalData = command.Info.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Win()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "V8",
			GameProviderConfiguration = "V8Configuration",
			HangfirePrefix = "gamerecord-v8-hangfire:",
			HangfireLocking = "gamerecord-v8-locking:",
			IntervalMinutes = 5,
			IntervalMinutesMax = 30,
			IntervalMinutesMinimum = 1
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

		var command = new HandleRecordCommand(
			Info: new GameWagerInfo
			{
				ExternalWagerId = "50-1690343439-360242142-4",
				ExternalGameId = "910",
				PlayerCode = "4ucmqplxb029",
				ExternalCreatedOn = DateTimeOffset.Parse("2023-07-26T11:50:39+08:00").ToTimestamp(),
				ExternalClosedOn = DateTimeOffset.Parse("2023-07-26T11:51:02+08:00").ToTimestamp(),
				Bet = 4.00m,
				ValidBet = 4.00m,
				Settle = 8.00m,
				Commission = 0.00m,
				WinLoss = 4.00m,
				AdditionalData = new JsonObject
				{
					["GameID"] = "50-1690343439-360242142-4",
					["Accounts"] = "72412_4ucmqplxb029",
					["ServerID"] = 9101,
					["KindID"] = 910,
					["TableID"] = 182020001,
					["ChairID"] = 0,
					["UserCount"] = 1,
					["CellScore"] = 4.00m,
					["AllBet"] = 4.00m,
					["Profit"] = 4.00m,
					["Revenue"] = 0,
					["GameStartTime"] = "2023-07-26 11:50:39",
					["GameEndTime"] = "2023-07-26 11:51:02",
					["CardValue"] = "330b251a17001",
					["ChannelID"] = 72412,
					["LineCode"] = "72412_GPI"
				}.ToJsonString()
			});

		var service = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await service.ExecuteAsync(command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.Info.PlayerCode,
				ExternalRoundId = command.Info.ExternalWagerId,
				ExternalBetId = command.Info.ExternalWagerId,
				ExternalGameId = command.Info.ExternalGameId,
				ExternalCreatedOn = command.Info.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = command.Info.ExternalClosedOn.ToDateTimeOffset(),
				ExternalPayoffOn = command.Info.ExternalClosedOn.ToDateTimeOffset(),
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Win,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 4.00m,
				ValidBet = 4.00m,
				Settle = 8.00m,
				Commission = 0.0M,
				Turnover = 4.00m,
				WinLoss = 4.00m,
				Jackpot = 0.0M,
				AdditionalData = command.Info.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
