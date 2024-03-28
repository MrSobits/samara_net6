namespace Bars.Gkh.Controllers.Administration
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.DomainService.Administration;

    public class FormatDataExportEntityInfoController : BaseController
    {
        [BasicAuthorization]
        [HttpPost]
        public ActionResult Update(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IFormatDataExportEntityInfoService>();
            using (this.Container.Using(service))
            {
                var result = service.UpdateExportEntitiesInfo(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
        }
    }
}
