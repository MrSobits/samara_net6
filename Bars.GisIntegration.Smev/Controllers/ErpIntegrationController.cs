namespace Bars.GisIntegration.Smev.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GisIntegration.Smev.DomainService;
    using Bars.Gkh.Domain;

    public class ErpIntegrationController : BaseController
    {
        /// <summary>
        /// Отправить в ЕРП
        /// </summary>
        public ActionResult SendToErp(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IErpIntegrationService>();
            using (this.Container.Using(service))
            {
                return service.SendDisposal(baseParams).ToJsonResult();
            }
        }

        /// <summary>
        /// Запрос справочника прокуратур
        /// </summary>
        public ActionResult RequestProsecutorsOfficesToErp(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IErpIntegrationService>();
            using (this.Container.Using(service))
            {
                return service.RequestProsecutorsOffices(baseParams).ToJsonResult();
            }
        }
    }
}