namespace Bars.GkhGji.Regions.Smolensk.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Regions.Smolensk.Entities.Protocol;

    public class ProtocolSmolController : Bars.GkhGji.Controllers.ProtocolController<ProtocolSmol>
    {
        public IBlobPropertyService<ProtocolSmol, ProtocolViolationDescription> DescriptionService { get; set; }

        public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            var result = this.DescriptionService.Save(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            var result = this.DescriptionService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}