namespace Bars.GisIntegration.Smev.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GisIntegration.Smev.DomainService;
    using Bars.Gkh.Domain;

    public class ErknmIntegrationController : BaseController
    {
        public ActionResult DocumentList(BaseParams baseParams)
        {
            var erknmIntegrationService = this.Container.Resolve<IErknmIntegrationService>();

            using (this.Container.Using(erknmIntegrationService))
            {
                return erknmIntegrationService.DocumentList(baseParams).ToJsonResult();
            }
        }
        
        /// <summary>
        /// Отправить в ЕРКНМ
        /// </summary>
        public ActionResult SendToErknm(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IErknmIntegrationService>();
            using (this.Container.Using(service))
            {
                return service.SendToErknm(baseParams).ToJsonResult();
            }
        }

        /// <summary>
        /// Проверка перед отправкомй в ЕРКНМ
        /// </summary>
        public ActionResult BeforeSendCheck(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IErknmIntegrationService>();
            using (this.Container.Using(service))
            {
                return service.BeforeSendCheck(baseParams).ToJsonResult();
            }
        }

        /// <summary>
        /// Выгрузить в Еxcel
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult ExcelFileExport(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IErknmIntegrationService>();
            using (this.Container.Using(service))
            {
                return service.ExcelFileExport(baseParams);
            }
        }
    }
}