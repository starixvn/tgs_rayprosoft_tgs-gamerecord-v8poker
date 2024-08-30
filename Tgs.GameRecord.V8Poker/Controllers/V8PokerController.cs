using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Tgs.GameRecord.V8Poker.Common;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.V8Poker.Controllers;

[ApiController]
[Route("V8")]
public class V8PokerController : ControllerBase
{
	/// <summary>
	/// Games the record manual asynchronous.
	/// </summary>
	/// <param name="messageSender">The message sender.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("manual")]
	public ValueTask GameRecordManualAsync(
		[FromServices] IMessageSender messageSender,
		V8PokerGameRecordMessage source)
		=> messageSender.PublishAsync(
			subject: V8PokerNatsConst.ManualPublish,
			data: JsonSerializer.SerializeToUtf8Bytes(source),
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the record schedule asynchronous.
	/// </summary>
	/// <param name="messageSender">The message sender.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("schedule")]
	public ValueTask GameRecordScheduleAsync(
		[FromServices] IMessageSender messageSender,
		V8PokerGameRecordMessage source)
		=> messageSender.PublishAsync(
			subject: V8PokerNatsConst.SchedulePublish,
			data: JsonSerializer.SerializeToUtf8Bytes(source),
			cancellationToken: HttpContext.RequestAborted);
}
