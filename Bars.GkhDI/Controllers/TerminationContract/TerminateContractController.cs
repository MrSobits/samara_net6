namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class TerminateContractController : B4.Alt.DataController<ManOrgContractRealityObject>
    {
        public override ActionResult List(BaseParams baseParams)
        {
            var listResult = (ListDataResult)Container.Resolve<IViewModel<ManOrgContractRealityObject>>("TerminateContract").List(DomainService, baseParams);
            return new JsonNetResult(new { success = true, data = listResult.Data, totalCount = listResult.TotalCount });
        }
    }
}
