namespace Bars.Gkh.Overhaul.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using DomainService;
    using Gkh.Entities.CommonEstateObject;

    public class CommonEstateObjectController : B4.Alt.DataController<CommonEstateObject>
    {
        public ICommonEstateObjectService CommonEstateObjectService { get; set; }

        public ActionResult ListTree(BaseParams baseParams)
        {
            var result = Container.Resolve<IListCeoService>().ListTree(baseParams);
            return new JsonNetResult(result.Data);
        }

        public ActionResult AddWorks(BaseParams baseParams)
        {
            var result = (BaseDataResult)this.CommonEstateObjectService.AddWorks(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }


        public ActionResult AddFeatureViol(BaseParams baseParams)
        {
            var result = (BaseDataResult)this.CommonEstateObjectService.AddFeatureViol(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult Export(BaseParams baseParams)
        {
            return new ReportStreamResult(this.CommonEstateObjectService.PrintReport(baseParams), "ceo_export.xlsx");
        }

        public ActionResult ListForRealObj(BaseParams baseParams)
        {
            var result = (ListDataResult)this.CommonEstateObjectService.ListForRealObj(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
        }
    }
}