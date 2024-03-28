namespace Bars.Gkh.Gis.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using CommonParams;
    using DomainService;
    
    public class CommonParamsController : BaseController
    {
        private IAnalysisReportService Service { get; set; }

       public virtual ActionResult List(BaseParams baseParams)
       {
           return JsSuccess(
               Container.ResolveAll<IGisCommonParam>().ToList<IGisCommonParam>().Select(x => new
                       {
                           x.Name,
                           x.Code,
                           x.CommonParamType,
                           x.IsPrecision
                       }
                   )
               );
       }

       public ActionResult GetAnalysisReportData(BaseParams baseParams)
       {
           Service = Container.Resolve<IAnalysisReportService>();
           var result = (BaseDataResult)Service.GetAnalysisReportData(baseParams);

           return new JsonNetResult(new { success = true, data = result.Data });
       }
    }
}
