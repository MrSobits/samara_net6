namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using GkhGji.Entities;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;

    public class KindCheckGjiSpecController : B4.Alt.DataController<KindCheckGji>
    {
        public IKindCheckGjiService Service { get; set; }

        public ActionResult SpecList(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.SpecList(baseParams);
            return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
        }
    }
}