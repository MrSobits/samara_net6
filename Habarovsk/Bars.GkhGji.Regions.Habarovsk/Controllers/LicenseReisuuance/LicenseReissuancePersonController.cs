namespace Bars.GkhGji.Regions.Habarovsk.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Modules.FileStorage;
    using DomainService;
    using Entities;


    public class LicenseReissuancePersonController :  B4.Alt.DataController<LicenseReissuancePerson>
    {
        public ILicenseReissuancePersonService service { get; set; }

        public ActionResult AddPersons(BaseParams baseParams)
        {
            var result = service.AddPersons(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : JsFailure(result.Message);
        }

        public ActionResult AddProvDocs(BaseParams baseParams)
        {
            var result = service.AddProvDocs(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : JsFailure(result.Message);
        }

    }
}