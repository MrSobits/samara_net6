namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class RealityObjectProtocolController : FileStorageDataController<RealityObjectProtocol>
    {
        public ActionResult GetProtocolByRealityObjectId(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IRealityObjectProtocolService>().GetProtocolByRealityObjectId(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}
