namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Entities;
    using DomainService;

    public class ManagingOrgMunicipalityController : B4.Alt.DataController<ManagingOrgMunicipality>
    {
        public ActionResult AddMunicipalities(BaseParams baseParams)
        {
            var result = Container.Resolve<IManagingOrgMunicipalityService>().AddMunicipalities(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}