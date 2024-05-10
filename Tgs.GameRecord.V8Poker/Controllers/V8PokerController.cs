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
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///         {
	///             "apiUrl": "https://wc-record.xpvcd046.com/",
	///             "deskey": "C6995933499B163B",
	///             "md5key": "628DA8F1857A6529",
	///             "agentId": "71033",
	///             "startTime": "2023-07-26T11:50:00+08:00",
	///             "endTime": "2023-07-26T12:00:00+08:00"
	///         }
	/// </remarks>
	[HttpPost("manual")]
	public ValueTask GameRecordManualAsync(
		[FromServices] IMessageSender messageSender,
		V8PokerGameRecordMessage source)
		=> messageSender.PublishAsync(
			subject: V8PokerNatsConst.ManualPublish,
			data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source)),
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the record schedule asynchronous.
	/// </summary>
	/// <param name="messageSender">The message sender.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///         {
	///             "apiUrl": "https://wc-record.xpvcd046.com/",
	///             "deskey": "C6995933499B163B",
	///             "md5key": "628DA8F1857A6529",
	///             "agentId": "71033",
	///             "startTime": "2023-07-26T11:50:00+08:00",
	///             "endTime": "2023-07-26T12:00:00+08:00"
	///         }
	/// </remarks>
	[HttpPost("schedule")]
	public ValueTask GameRecordScheduleAsync(
		[FromServices] IMessageSender messageSender,
		V8PokerGameRecordMessage source)
		=> messageSender.PublishAsync(
			subject: V8PokerNatsConst.SchedulePublish,
			data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source)),
			cancellationToken: HttpContext.RequestAborted);
}
