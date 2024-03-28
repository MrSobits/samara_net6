namespace Bars.Gkh.SignalR
{
    using Microsoft.AspNetCore.SignalR;

    /// <summary>
    /// Хаб для обновление статуса задачи.
    /// </summary>
    public class ReportStatusHub : Hub<IReportStatusHubClient>
    {
    }
}
