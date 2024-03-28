namespace Bars.Gkh.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class InspectorController : B4.Alt.DataController<Inspector>
    {
        public IInspectorService InspectorService { get; set; }

        public ActionResult SubcribeToInspectors(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<IInspectorService>().SubcribeToInspectors(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = InspectorService.GetInfo(baseParams);
            if (result.Success)
            {
                return new JsonNetResult(result.Data);
            }

            return JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddZonalInspection(BaseParams baseParams)
        {
            var result = InspectorService.AddZonalInspection(baseParams);
            if (result.Success)
            {
                return new JsonNetResult(new { success = true });
            }

            return JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListZonalInspection(BaseParams baseParams)
        {
            var result = (ListDataResult)InspectorService.ListZonalInspection(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }
    }
}