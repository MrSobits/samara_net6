namespace Bars.GkhCr.Regions.Tatarstan.Controllers.Dict
{
    using Bars.B4;
    using Bars.GkhCr.Regions.Tatarstan.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4.IoC;

    using Microsoft.AspNetCore.Mvc;

    public class ElementOutdoorController : B4.Alt.DataController<ElementOutdoor>
    {
        public ActionResult AddWorks(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IElementOutdoorService>();
            using (this.Container.Using(service))
            {
                var result = service.AddWorks(baseParams);

                return result.Success
                    ? JsonNetResult.Success
                    : JsonNetResult.Failure(result.Message);
            }
        }

        public ActionResult DeleteWork(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IElementOutdoorService>();
            using (this.Container.Using(service))
            {
                var result = service.DeleteWork(baseParams);

                return result.Success
                    ? JsonNetResult.Success
                    : JsonNetResult.Failure(result.Message);
            }
        }
    }
}