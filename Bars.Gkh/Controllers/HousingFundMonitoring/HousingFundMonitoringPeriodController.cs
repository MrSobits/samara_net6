namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Контроллер для Период Мониторинга жилищного фонда
    /// </summary>
    public class HousingFundMonitoringPeriodController : B4.Alt.DataController<HousingFundMonitoringPeriod>
    {
        /// <summary>
        /// Сервис для работы с Мониторинг жилищного фонда
        /// </summary>
        public IHousingFundMonitoringService HousingFundMonitoringService { get; set; }

        /// <summary>
        /// Массовое создание периодов
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        public ActionResult MassCreate(BaseParams baseParams)
        {
            return this.HousingFundMonitoringService.MassCreate(baseParams).ToJsonResult();
        }
    }
}