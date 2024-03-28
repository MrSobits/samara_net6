namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.GkhDi.DomainService;
    using Bars.GkhDi.Entities;

    public class GroupDiController : B4.Alt.DataController<GroupDi>
    {
        public ActionResult GetGroupActions(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IGroupDiService>().GetGroupActions(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}
