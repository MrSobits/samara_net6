namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;

    using Bars.B4.Modules.FileStorage;
    using Bars.GkhDi.DomainService;

    using Entities;

    public class ServiceController : FileStorageDataController<BaseService>
    {
        private readonly IServService servService;

        public ServiceController(IServService servService)
        {
            this.servService = servService;
        }
        
        public ActionResult GetCountMandatory(BaseParams baseParams)
        {
            var result = (BaseDataResult) this.servService.GetCountMandatory(baseParams);
            return result.Success 
                ? new JsonNetResult(result.Data)
                : JsonNetResult.Failure(result.Message);
        }

        public ActionResult CopyService(BaseParams baseParams)
        {
            var result = (BaseDataResult) this.servService.CopyService(baseParams);
            return result.Success
                ? new JsonNetResult(new { success = true, message = result.Message, data = result.Data })
                : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetInfoServPeriod(BaseParams baseParams)
        {
            var result = (BaseDataResult) this.servService.GetInfoServPeriod(baseParams);
            return result.Success 
                ? new JsonNetResult(result.Data)
                : JsonNetResult.Failure(result.Message);
        }

        public ActionResult CopyServPeriod(BaseParams baseParams)
        {
            var result = (BaseDataResult) this.servService.CopyServPeriod(baseParams);
            return result.Success
                ? new JsonNetResult(new { success = true, message = result.Message, data = result.Data })
                : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetUnfilledMandatoryServs(BaseParams baseParams)
        {
            var result = (BaseDataResult) this.servService.GetUnfilledMandatoryServs(baseParams);
            return result.Success
                ? new JsonNetResult(new { success = true, message = result.Message })
                : JsonNetResult.Failure(result.Message);
        }

        public ActionResult CopyUninhabitablePremisesPeriod(BaseParams baseParams)
        {
            var result = (BaseDataResult) this.servService.CopyUninhabitablePremisesPeriod(baseParams);
            return result.Success
                ? new JsonNetResult(new { success = true, message = result.Message, data = result.Data })
                : JsonNetResult.Failure(result.Message);
        }

        public ActionResult CopyCommonAreasPeriod(BaseParams baseParams)
        {
            var result = (BaseDataResult) this.servService.CopyCommonAreasPeriod(baseParams);
            return result.Success
                ? new JsonNetResult(new { success = true, message = result.Message, data = result.Data })
                : JsonNetResult.Failure(result.Message);
        }
    }
}

