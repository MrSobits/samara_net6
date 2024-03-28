namespace Bars.GkhGji.Regions.Zabaykalye.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;

    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;

    public class ZabaykalyeProtocolController : GkhGji.Controllers.ProtocolController
    {
        public IBlobPropertyService<Protocol, ProtocolLongDescription> LongTextService { get; set; }

        public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            var result = LongTextService.Save(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            var result = LongTextService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}