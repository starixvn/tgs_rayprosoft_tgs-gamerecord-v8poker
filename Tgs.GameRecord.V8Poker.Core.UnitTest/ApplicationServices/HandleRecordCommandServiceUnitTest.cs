using System.Text.Json.Nodes;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using NSubstitute;
using Tgs.GameInterface.V8Poker.Protos;
using Tgs.GameRecord.Handle.Client;
using Tgs.GameRecord.Handle.Common.Protos;
using Tgs.GameRecord.V8Poker.Core.ApplicationServices;

namespace Tgs.GameRecord.V8Poker.Core.UnitTest.ApplicationServices;

public class HandleRecordCommandServiceUnitTest
{
    [Fact]
    public async Task Loses()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "V8",
            GameProviderConfiguration = "V8Configuration",
            GameProviderHangfire = "v8-schedule:hangfire:",
            IntervalMinutes = 5,
            IntervalMinutesMax = 30,
            IntervalMinutesMinimum = 1
        });
        var fakeClient = Substitute.For<IGameRecordHandleClient>();

        var command = new HandleRecordCommand(
            Info: new GameWagerInfo
            {
                ExternalWagerId = "50-1690343466-360242382-3",
                ExternalGameId = "910",
                PlayerCode = "4ucmqplxb029",
                ExternalCreatedOn = DateTimeOffset.Parse("2023-07-26T11:51:06+08:00").ToTimestamp(),
                ExternalClosedOn = DateTimeOffset.Parse("2023-07-26T11:51:29+08:00").ToTimestamp(),
                Bet = 8.00m,
                ValidBet = 0.00m,
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
            gameRecordInfo,
            fakeClient);

        await service.ExecuteAsync(command: command);

        _ = fakeClient
            .Received(1)
            .MessageAsync(
            gameProviderCode: Arg.Is(gameRecordInfo.Value.GameProviderCode),
            playerCode: Arg.Is(command.Info.PlayerCode),
            wagerState: Arg.Is(WagerStates.Lose),
            wagerType: Arg.Is(WagerTypes.Normal),
            freeGameState: Arg.Is(FreeGameStates.None),
            gambleState: Arg.Is(GambleStates.None),
            cashoutState: Arg.Is(CashoutStates.None),
            resettleState: Arg.Is(ResettleStates.None),
            externalRoundId: Arg.Is(command.Info.ExternalWagerId),
            externalWagerId: Arg.Is(command.Info.ExternalWagerId),
            externalGameId: Arg.Is(command.Info.ExternalGameId),
            externalCreatedOn: Arg.Is(command.Info.ExternalCreatedOn.ToDateTimeOffset()),
            externalClosedOn: Arg.Is((DateTimeOffset?)command.Info.ExternalClosedOn.ToDateTimeOffset()),
            externalPayoffOn: Arg.Is((DateTimeOffset?)command.Info.ExternalClosedOn.ToDateTimeOffset()),
            externalResettleOn: Arg.Is(DateTimeOffset.UnixEpoch),
            bet: Arg.Is(command.Info.Bet.ToDecimal().GetValueOrDefault(0)),
            validBet: Arg.Is(command.Info.ValidBet.ToDecimal().GetValueOrDefault(0)),
            settle: Arg.Is(command.Info.Settle.ToDecimal().GetValueOrDefault(0)),
            commission: Arg.Is(0.0m),
            turnover: Arg.Is(command.Info.ValidBet.ToDecimal().GetValueOrDefault(0)),
            winLoss: Arg.Is(command.Info.WinLoss.ToDecimal().GetValueOrDefault(0)),
            additionalData: Arg.Is(command.Info.AdditionalData),
            cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Wins()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "V8",
            GameProviderConfiguration = "V8Configuration",
            GameProviderHangfire = "v8-schedule:hangfire:",
            IntervalMinutes = 5,
            IntervalMinutesMax = 30,
            IntervalMinutesMinimum = 1
        });
        var fakeClient = Substitute.For<IGameRecordHandleClient>();

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
            })
        {
        };
        var service = new HandleRecordCommandService(
            gameRecordInfo,
            fakeClient);

        await service.ExecuteAsync(command: command);

        _ = fakeClient
            .Received(1)
            .MessageAsync(
            gameProviderCode: Arg.Is(gameRecordInfo.Value.GameProviderCode),
            playerCode: Arg.Is(command.Info.PlayerCode),
            wagerState: Arg.Is(WagerStates.Win),
            wagerType: Arg.Is(WagerTypes.Normal),
            freeGameState: Arg.Is(FreeGameStates.None),
            gambleState: Arg.Is(GambleStates.None),
            cashoutState: Arg.Is(CashoutStates.None),
            resettleState: Arg.Is(ResettleStates.None),
            externalRoundId: Arg.Is(command.Info.ExternalWagerId),
            externalWagerId: Arg.Is(command.Info.ExternalWagerId),
            externalGameId: Arg.Is(command.Info.ExternalGameId),
            externalCreatedOn: Arg.Is(command.Info.ExternalCreatedOn.ToDateTimeOffset()),
            externalClosedOn: Arg.Is((DateTimeOffset?)command.Info.ExternalClosedOn.ToDateTimeOffset()),
            externalPayoffOn: Arg.Is((DateTimeOffset?)command.Info.ExternalClosedOn.ToDateTimeOffset()),
            externalResettleOn: Arg.Is(DateTimeOffset.UnixEpoch),
            bet: Arg.Is(command.Info.Bet.ToDecimal().GetValueOrDefault(0)),
            validBet: Arg.Is(command.Info.ValidBet.ToDecimal().GetValueOrDefault(0)),
            settle: Arg.Is(command.Info.Settle.ToDecimal().GetValueOrDefault(0)),
            commission: Arg.Is(0.0m),
            turnover: Arg.Is(command.Info.ValidBet.ToDecimal().GetValueOrDefault(0)),
            winLoss: Arg.Is(command.Info.WinLoss.ToDecimal().GetValueOrDefault(0)),
            additionalData: Arg.Is(command.Info.AdditionalData),
            cancellationToken: Arg.Any<CancellationToken>());
    }
}