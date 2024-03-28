namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.SignalR;

    using Microsoft.AspNetCore.SignalR;

    public class TaskStatusController : BaseController
    {
        private readonly IHubContext<ReportStatusHub, IReportStatusHubClient> reportStatusHubContext;

        public TaskStatusController(IHubContext<ReportStatusHub, IReportStatusHubClient> reportStatusHubContext)
        {
            this.reportStatusHubContext = reportStatusHubContext;
        }

        public ActionResult UpdateStatus(BaseParams baseParams)
        {
            var reportId = baseParams.Params.GetAs<long>("reportId", ignoreCase: true);
            var status = baseParams.Params.GetAs<TaskStatus>("taskStatus", ignoreCase: true);
            var fileId = baseParams.Params.GetAs<long>("fileId", ignoreCase: true);

            this.reportStatusHubContext
                .Clients.All
                .UpdateStatus(reportId, status, fileId)
                .GetResultWithoutContext();
            
            return JsonNetResult.Success;
        }
    }
}
