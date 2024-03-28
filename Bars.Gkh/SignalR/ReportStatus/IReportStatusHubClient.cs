namespace Bars.Gkh.SignalR;

using System.Threading.Tasks;

using TaskStatus = Bars.B4.Modules.Tasks.Common.Contracts.TaskStatus;

/// <summary>
/// Интерфейс для клиента <see cref="ReportStatusHub"/>
/// </summary>
public interface IReportStatusHubClient
{
    Task UpdateStatus(long reportId, TaskStatus status, long fileId);
}