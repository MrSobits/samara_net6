namespace Bars.GisIntegration.Smev.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils.Web;
    using Bars.GisIntegration.Smev.DomainService;
    using Bars.Gkh.Domain;

    /// <summary>
    /// Контроллер для базового функционала реестра интеграции
    /// </summary>
    public class BaseIntegrationController : BaseController
    {
        public ActionResult GetTriggerProtocolView(BaseParams baseParams)
        {
            var baseIntegrationService = this.Container.Resolve<IBaseIntegrationService>();

            using (this.Container.Using(baseIntegrationService))
            {
                return baseIntegrationService.GetTriggerProtocolView(baseParams).ToJsonResult();
            }
        }

        public ActionResult GetXmlData(BaseParams baseParams)
        {
            var baseIntegrationService = this.Container.Resolve<IBaseIntegrationService>();

            using (this.Container.Using(baseIntegrationService))
            {
                return baseIntegrationService.GetXmlData(baseParams).ToJsonResult();
            }
        }

        public ActionResult GetLogFile(BaseParams baseParams)
        {
            var baseIntegrationService = this.Container.Resolve<IBaseIntegrationService>();

            using (this.Container.Using(baseIntegrationService))
            {
                return (DownloadResult)baseIntegrationService.GetLogFile(baseParams).Data;
            }
        }
    }
}