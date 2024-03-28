using System;

namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;

    using Bars.Gkh.RegOperator.DomainService;

    public class PerfWorkActPaymentController : BaseController
    {
        public IPerformedWorkActPaymentService Service { get; set; }

        public ActionResult ExportToTxt(BaseParams baseParams)
        {
            try
            {
                var result = Service.ExportToTxt(baseParams);

                return new JsonNetResult(result.Data);
            }
            catch (Exception e)
            {
                return JsFailure(e.Message);
            }
        }
        
    }
}