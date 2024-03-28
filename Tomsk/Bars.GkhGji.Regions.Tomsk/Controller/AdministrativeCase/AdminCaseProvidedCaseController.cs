namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.GkhGji.Regions.Tomsk.DomainService;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class AdministrativeCaseProvidedDocController : B4.Alt.DataController<AdministrativeCaseProvidedDoc>
    {
        public ActionResult AddProvidedDocs(BaseParams baseParams)
        {
            var result = Resolve<IAdminCaseProvidedDocService>().AddProvidedDocs(baseParams);
            return result.Success ? JsSuccess() : JsFailure(result.Message);
        }
    }
}