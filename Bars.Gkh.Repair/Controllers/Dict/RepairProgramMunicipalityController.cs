namespace Bars.Gkh.Repair.Controllers
{
    using Bars.B4;

    using Bars.Gkh.Repair.DomainService;
    using Bars.Gkh.Repair.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class RepairProgramMunicipalityController : B4.Alt.DataController<RepairProgramMunicipality>
    {
        public IRepairProgramMunicipalityService RepairProgramMunicipalityService { get; set; }

        public ActionResult AddMunicipality(BaseParams baseParams)
        {
            var result = (BaseDataResult)RepairProgramMunicipalityService.AddMunicipality(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}