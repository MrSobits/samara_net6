namespace Bars.Gkh.Decisions.Nso.Domain.Decisions
{
    using System.IO;
    using B4;

    public interface IDecisionNotificationService
    {
        Stream DownloadNotification(BaseParams baseParams);
    }
}