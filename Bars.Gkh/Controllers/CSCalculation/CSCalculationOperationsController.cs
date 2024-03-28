namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Entities;
    using Bars.B4;
    using DomainService;
    using System.Collections;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;

    public class CSCalculationOperationsController : BaseController
    {
        public ICSCalculationOperationsService service { get; set; }
        public ActionResult CalculateCS(BaseParams baseParams)
        {
            try
            {
                var data = service.CalculateCS(baseParams);
                return JsSuccess(data);
            }
            finally
            {

            }
        }

        public ActionResult AddTarifs(BaseParams baseParams)
        {
            var result = service.AddTarifs(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddCoefficient(BaseParams baseParams)
        {
            var result = service.AddCoefficient(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddCategoryes(BaseParams baseParams)
        {
            var result = service.AddCategoryes(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetListRoom(BaseParams baseParams)
        {
           
            try
            {
                return service.GetListRoom(baseParams).ToJsonResult();
            }
            finally
            {
                //  Container.Release(service);
            }
        }

    }
 
}