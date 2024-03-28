namespace Bars.Gkh.Controllers.RealityObj
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class CapitalGroupController : B4.Alt.DataController<CapitalGroup>
    {
        public ActionResult ListWithoutPaging(BaseParams baseParams)
        {
            var result = (ListDataResult)this.Container.Resolve<IRealityObjectService>().ListWoPagingCapitalGroup(baseParams);

            if (result.Success)
            {
                return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
            }

            return JsonNetResult.Message(result.Message);
        }
    }
}