namespace Bars.Gkh.Regions.Tatarstan.Controller.Outdoor
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Regions.Tatarstan.DomainService;

    public class WorkRealityObjectOutdoorController : B4.Alt.DataController<Entities.Dicts.WorkRealityObjectOutdoor>
    {
        public ActionResult ListOutdoorWorksByPeriod(BaseParams baseParams)
        {
            IOutdoorWorkService workService;

            using (this.Container.Using(workService = this.Container.Resolve<IOutdoorWorkService>()))
            {
                var result = (ListDataResult) workService.ListOutdoorWorksByPeriod(baseParams);

                return result.Success
                    ? new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount })
                    : JsonNetResult.Message(result.Message);
            }
        }
    }
}