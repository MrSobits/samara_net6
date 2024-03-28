namespace Bars.Gkh.Regions.Tatarstan.Controller.ChargeSplitting
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Regions.Tatarstan.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Контроллер для сущности "Договора УО с РСО на предоставление услуги"
    /// </summary>
    public class PubServContractPeriodSummController : B4.Alt.DataController<PubServContractPeriodSumm>
    {
        /// <summary>
        /// Сервис
        /// </summary>
        public IPublicServiceOrgExportService Service { get; set; }

        /// <summary>
        /// Экспорт
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult Export(BaseParams baseParams)
        {
            var report = this.Service.ExportToCsv(baseParams);
            return report;
        }
    }
}
