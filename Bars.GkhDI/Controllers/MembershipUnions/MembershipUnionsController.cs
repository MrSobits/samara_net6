namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;

    using Gkh.Entities;

    public class MembershipUnionsController : B4.Alt.DataController<ManagingOrgMembership>
    {
        public override ActionResult List(BaseParams baseParams)
        {
            var listResult = (ListDataResult)Container.Resolve<IViewModel<ManagingOrgMembership>>("MembershipUnions").List(DomainService, baseParams);
            return new JsonNetResult(new { success = true, data = listResult.Data, totalCount = listResult.TotalCount });
        }
    }
}
