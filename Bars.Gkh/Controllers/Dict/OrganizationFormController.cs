namespace Bars.Gkh.Controllers.Dict
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService.Dict;
    using Bars.Gkh.Entities;

    public class OrganizationFormController : B4.Alt.DataController<OrganizationForm>
    {
        public ActionResult Import(BaseParams baseParams) {
            var importService = Container.Resolve<IOrganizationFormImportService>();
            var xlsFile = baseParams.Files["File"];
            var result = importService.Import(xlsFile);
            return JsSuccess(result);
        }
    }
}
