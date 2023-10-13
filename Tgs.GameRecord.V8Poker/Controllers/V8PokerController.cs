using Microsoft.AspNetCore.Mvc;
using Tgs.GameRecord.V8Poker.Core.ApplicationServices;
using Tgs.GameRecord.V8Poker.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.V8Poker.Controllers;

[ApiController]
[Route("V8")]
public class V8PokerController : ControllerBase
{
    /// <summary>
    /// Games the record asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///         {
    ///             "apiRecordUrl": "https://wc-record.xpvcd046.com/",
    ///             "deskey": "C6995933499B163B",
    ///             "md5key": "628DA8F1857A6529",
    ///             "agentId": "71033",
    ///             "startTime": "2023-07-26T11:50:00+08:00",
    ///             "endTime": "2023-07-26T12:00:00+08:00"
    ///         }
    /// </remarks>
    [HttpPost]
    public ValueTask GameRecordAsync(
        [FromServices] ICommandService<PullRecordCommand> service,
        PullRecordViewModel source)
    {
        var command = new PullRecordCommand(
            ApiUrl: source.ApiRecordUrl,
            Deskey: source.Deskey,
            Md5key: source.Md5key,
            AgentId: source.AgentId,
            StartTime: source.StartTime,
            EndTime: source.EndTime);
        return service
            .ExecuteAsync(
            command,
            HttpContext.RequestAborted);
    }
}