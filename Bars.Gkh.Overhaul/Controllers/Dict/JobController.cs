namespace Bars.Gkh.Overhaul.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using DomainService;
    using B4;
    using Entities;

    internal class JobController : B4.Alt.DataController<Job>
    {
        public ActionResult ListTree(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IJobService>().ListTree(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}
