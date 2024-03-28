namespace Bars.GkhCr.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    public class ControlDateController : B4.Alt.DataController<ControlDate>
    {
        public ActionResult AddStageWorks(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<IControlDateService>().AddStageWorks(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddWorks(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<IControlDateService>().AddWorks(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}
