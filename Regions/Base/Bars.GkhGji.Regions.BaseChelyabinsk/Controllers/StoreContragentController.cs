namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.DomainService;

    public class StoreContragentController : B4.Alt.DataController<Contragent>
    {
        public override ActionResult List(BaseParams baseParams)
        {
            var result = (ListDataResult)this.Resolve<IBaseJurPersonContragentService>().List(baseParams);
            return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
        }
    }
}