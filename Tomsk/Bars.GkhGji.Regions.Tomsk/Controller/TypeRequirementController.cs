namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tomsk.DomainService;

    public class TypeRequirementController : BaseController
    {
        public ITypeRequirementService Service { get; set; }

        public ActionResult GetItemsByDoc(BaseParams baseParams)
        {
            var result = (ListDataResult) Service.GetItemsByDoc(baseParams);

            return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
        }
    }
}