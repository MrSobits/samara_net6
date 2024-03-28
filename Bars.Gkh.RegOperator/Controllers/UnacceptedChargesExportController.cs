namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh.RegOperator.DomainService;

    /// <summary>
    /// Контроллер для экспорта начислений
    /// </summary>
    public class UnacceptedChargesExportController : BaseController
    {
        /// <summary>
        /// Сервис
        /// </summary>
        public IUnacceptedChargesExportService Service { get; set; }

        /// <summary>
        /// Выгрузить начисления
        /// </summary>
        public ActionResult GetUnacceptedChargesExport(BaseParams baseParams)
        {
            return new ReportStreamResult(this.Service.UnacceptedChargesExport(baseParams), "export_unaccepted_charges.zip");
        }
    }
}