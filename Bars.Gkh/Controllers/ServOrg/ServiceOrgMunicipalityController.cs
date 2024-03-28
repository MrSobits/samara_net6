namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Entities;
    using DomainService;

    public class ServiceOrgMunicipalityController : B4.Alt.DataController<ServiceOrgMunicipality>
    {
        public ActionResult AddMunicipalities(BaseParams baseParams)
        {
            var result = Container.Resolve<IServiceOrgMunicipalityService>().AddMunicipalities(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}