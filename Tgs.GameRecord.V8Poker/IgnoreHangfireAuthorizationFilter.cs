using Hangfire.Dashboard;

namespace Tgs.GameRecord.V8Poker;

public class IgnoreHangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
}