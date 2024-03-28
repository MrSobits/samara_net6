namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.DataResult;

    /// <summary>
    /// Контроллер для Расчетный счет регоператора
    /// </summary>
    public class RegopCalcAccountController : B4.Alt.DataController<RegopCalcAccount>
    {
        /// <summary>
        /// Отображения суммы счетов реестр домов регионального оператора
        /// </summary>
        public ActionResult ListRegister(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IRegopCalcAccountService>();

            using (this.Container.Using(service))
            {
                var result = service.ListRegister(baseParams);
                return new JsonNetResult(new { success = result.Success, message = result.Message, data = result.Data, summaryData = ((ListSummaryResult)result).SummaryData });
            }
        }

        /// <summary>
        /// Экспорт
        /// </summary>
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("RegopCalcAccountExport");
            using (this.Container.Using(export))
            {
                return export?.ExportData(baseParams);
            }
        }

        /// <summary>
        /// Список расчетных счетов по регоператору
        /// </summary>
        public ActionResult ListByRegop(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IRegopCalcAccountService>();

            using (this.Container.Using(service))
            {
                return service.ListByRegop(baseParams).ToJsonResult();
            }
        }
    }
}