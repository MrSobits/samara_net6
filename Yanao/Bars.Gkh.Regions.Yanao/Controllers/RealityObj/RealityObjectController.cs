namespace Bars.Gkh.Regions.Yanao.Controllers.RealityObj
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Regions.Yanao.DomainService;
 
    public class RealityObjectController : Gkh.Controllers.RealityObjectController
    {
        public IRealityObjectExtensionService RealityObjectExtensionService { get; set; }

        public override ActionResult Update(BaseParams baseParams)
        {
            RealityObjectExtensionService.SaveTechPassportScanFile(baseParams);

            return base.Update(baseParams);
        }
    }
}