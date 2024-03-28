namespace Bars.GkhRf.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.GkhRf.DomainService;

    public class RealityObjectForTransferController : B4.Alt.DataController<RealityObject>
    {
        public override ActionResult List(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IRealObjForTransferService>().List(baseParams);
            return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
        }
    }
}