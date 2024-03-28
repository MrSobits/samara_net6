namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Overhaul.Entities;

    public class RealEstateTypeMunicipalityController : B4.Alt.DataController<RealEstateTypeMunicipality>
    {
        public IRealEstateTypeMunicipalityService Service { get; set; }

        public ActionResult AddMunicipality(BaseParams baseParams)
        {
            var result = Service.AddMunicipality(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}