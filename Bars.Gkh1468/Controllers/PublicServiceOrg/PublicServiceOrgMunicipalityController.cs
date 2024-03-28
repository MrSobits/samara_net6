namespace Bars.Gkh1468.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh1468.DomainService;
    using Bars.Gkh1468.Entities;

    public class PublicServiceOrgMunicipalityController : B4.Alt.DataController<PublicServiceOrgMunicipality>
    {
        public ActionResult AddMunicipalityes(BaseParams baseParams)
        {
            var result = Container.Resolve<IPublicServiceOrgMunicipalityService>().AddMunicipalities(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}