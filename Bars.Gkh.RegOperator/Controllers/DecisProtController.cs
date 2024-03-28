namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Domain;

    public class DecisProtController : BaseController
    {
        private readonly IDecisionService _decisionService;

        public DecisProtController(IDecisionService decisionService)
        {
            _decisionService = decisionService;
        }

        public ActionResult DownloadContract(BaseParams baseParams)
        {
            return new ReportStreamResult(_decisionService.DownloadContract(baseParams), "Договор.docx");
        }
    }
}