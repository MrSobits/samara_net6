namespace Bars.GkhGji.Controllers.FuelInfo
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.DomainService.FuelInfo;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Контроллер для Период сведений о наличии и расходе топлива
    /// </summary>
    public class FuelInfoPeriodController : B4.Alt.DataController<FuelInfoPeriod>
    {
        /// <summary>
        /// Сервис для работы со сведениями о наличии и расходе топлива
        /// </summary>
        public IFuelInfoService FuelInfoService { get; set; }

        /// <summary>
        /// Массовое создание периодов
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        public ActionResult MassCreate(BaseParams baseParams)
        {
            return this.FuelInfoService.MassCreate(baseParams).ToJsonResult();
        }
    }
}