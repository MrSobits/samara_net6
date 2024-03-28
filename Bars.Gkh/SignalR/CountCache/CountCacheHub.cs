namespace Bars.Gkh.SignalR
{
    using Microsoft.AspNetCore.SignalR;

    /// <summary>
    /// Хаб для работы с кэшем счётчиков
    /// </summary>
    public class CountCacheHub : Hub<ICountCacheHubClient>
    {
    }
}