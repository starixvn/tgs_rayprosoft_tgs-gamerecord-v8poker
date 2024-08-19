using Tgs.GameRecord.V8Poker.Core.ApplicationServices;

namespace Tgs.GameRecord.V8Poker.Core;

public interface IPullRecordDispatcher
{
	ValueTask PushCommandAsync(PullRecordCommand command, CancellationToken cancellationToken = default);
}
