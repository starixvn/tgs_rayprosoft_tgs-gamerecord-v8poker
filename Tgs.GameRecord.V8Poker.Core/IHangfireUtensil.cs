using Tgs.GameRecord.V8Poker.Core.JobApplicationServices;

namespace Tgs.GameRecord.V8Poker.Core;

public interface IHangfireUtensil
{
    public Task JobCheckAsync();

    public Task JobExecuteAsync(JobExecuteCommand source);

    public Task JobInitAsync();
}
