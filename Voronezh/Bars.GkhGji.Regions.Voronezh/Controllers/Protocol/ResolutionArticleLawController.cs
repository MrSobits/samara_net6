namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using DomainService;
    using Bars.GkhGji.Entities;
    using Entities;
    using Bars.Gkh.Domain;

    public class ResolutionArticleLawController :BaseController
    {
      
        public ActionResult GetListResolution(BaseParams baseParams)
        {
            var resolutionService = Container.Resolve<IResolutionArticleLawService>();
            try
            {
                return resolutionService.GetListResolution(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }

        public ActionResult GetListDisposal(BaseParams baseParams)
        {
            var resolutionService = Container.Resolve<IResolutionArticleLawService>();
            try
            {
                return resolutionService.GetListDisposal(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }

    }
}