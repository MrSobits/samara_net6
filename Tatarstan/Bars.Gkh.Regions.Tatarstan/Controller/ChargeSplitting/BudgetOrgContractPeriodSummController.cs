namespace Bars.Gkh.Regions.Tatarstan.Controller.ChargeSplitting
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Regions.Tatarstan.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Контроллер <see cref="BudgetOrgContractPeriodSumm"/>
    /// </summary>
    public class BudgetOrgContractPeriodSummController : B4.Alt.DataController<BudgetOrgContractPeriodSumm>
    {
        /// <summary>
        /// Сервис
        /// </summary>
        public IBudgetOrgContractExportService Service { get; set; }

        /// <summary>
        /// Экспортировать данные
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult Export(BaseParams baseParams)
        {
            return this.Service.ExportToCsv(baseParams);
        }
    }
}