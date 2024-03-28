namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;

    public class LongProgramController : BaseController
    {
        public ActionResult MakeLongProgram(BaseParams baseParams)
        {
            var result = Container.Resolve<ILongProgramService>().MakeLongProgram(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult MakeLongProgramAll(BaseParams baseParams)
        {
            var result = Container.Resolve<ILongProgramService>().MakeLongProgramAll(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult SetPriority(BaseParams baseParams)
        {
            var result = Container.Resolve<IPriorityService>().SetPriority(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult SetPriorityAll(BaseParams baseParams)
        {
            var result = Container.Resolve<IPriorityService>().SetPriorityAll(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }
    }
}