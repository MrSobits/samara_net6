namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainModelServices;

    /// <summary>
    /// Контроллер детализации лицевых счетов
    /// </summary>
    public class PersonAccountDetalizationController : BaseController
    {
        public IPersonalAccountDetailService PersonalAccountDetailService { get; set; }

        /// <summary>
        /// Детализация по полям лицевых счетов
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        public ActionResult GetFieldDetails(BaseParams baseParams)
        {
            return this.PersonalAccountDetailService.GetFieldDetail(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Детализация операций по периоду
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        public ActionResult ListOperationDetails(BaseParams baseParams)
        {
            return this.PersonalAccountDetailService.GetPeriodOperationDetail(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Детализация по периоду
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        public ActionResult ListPeriodSummaryInfo(BaseParams baseParams)
        {
            return this.PersonalAccountDetailService.GetPeriodDetail(baseParams).ToJsonResult();
        }
    }
}