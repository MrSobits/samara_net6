namespace Bars.Gkh.Controllers.Dict.Multipurpose
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh.DomainService.Multipurpose;
    using Bars.Gkh.Entities.Dicts.Multipurpose;

    public class MultipurposeGlossaryItemController : B4.Alt.DataController<MultipurposeGlossaryItem>
    {
        public ActionResult ListByGlossaryCode(BaseParams baseParams)
        {
            var data = this.Resolve<IMultipurposeGlossaryItemService>().ListByGlossaryCode(baseParams);

            return new JsonNetResult(new { success = true, data = data.Data, totalCount = data.TotalCount });
        }
    }
}