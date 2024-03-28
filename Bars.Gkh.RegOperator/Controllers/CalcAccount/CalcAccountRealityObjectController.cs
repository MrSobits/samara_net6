namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Контроллер расчетных счетов домов
    /// </summary>
    public class CalcAccountRealityObjectController : B4.Alt.DataController<CalcAccountRealityObject>
    {
        public ICalcAccountRealityObjectService Service { get; set; }

        /// <summary>
        /// Список домов на добавление
        /// </summary>
        public ActionResult ListRobjectToAdd(BaseParams baseParams)
        {
            return this.Service.ListRobjectToAdd(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Массовое создание
        /// </summary>
        public ActionResult MassCreate(BaseParams baseParams)
        {
            return this.Service.MassCreate(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Список для регопа
        /// </summary>
        public ActionResult ListForRegop(BaseParams baseParams)
        {
            return this.Service.ListForRegop(baseParams).ToJsonResult();
        }
    }
}