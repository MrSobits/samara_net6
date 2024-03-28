namespace Bars.GkhGji.Regions.Chelyabinsk.Controllers.MKDLicRequest
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Chelyabinsk.DomainService.MKDLicRequest;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using Microsoft.AspNetCore.Mvc;

    public class MKDLicRequestExecutantController : FileStorageDataController<MKDLicRequestExecutant>
    {
        public ActionResult AddExecutants(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IMKDLicRequestExecutantService>().AddExecutants(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult RedirectExecutant(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IMKDLicRequestExecutantService>().RedirectExecutant(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListAppealOrderExecutant(BaseParams baseParams)
        {
            var appealService = Container.Resolve<IMKDLicRequestExecutantService>();
            try
            {
                return appealService.ListAppealOrderExecutant(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }
    }
}