namespace Bars.GkhCr.Regions.Tatarstan.Controllers.Dict
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhCr.Regions.Tatarstan.DomainService;
    using Bars.GkhCr.Regions.Tatarstan.Entities.Dict.RealityObjectOutdoorProgram;
    using Microsoft.AspNetCore.Mvc;

    public class RealityObjectOutdoorProgramController : FileStorageDataController<RealityObjectOutdoorProgram>
    {
        public ActionResult CopyProgram(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IRealityObjectOutdoorProgramService>();
            using (this.Container.Using(service))
            {
                var result = service.CopyProgram(baseParams);
                return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
            }
        }

        public ActionResult GetProgramNames(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IRealityObjectOutdoorProgramService>();
            using (this.Container.Using(service))
            {
                var result = (ListDataResult)service.GetProgramNames(baseParams);
                return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
            }
        }
    }
}
