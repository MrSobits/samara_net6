namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.DomainService;

    /// <summary>
    /// компетентные организации
    /// </summary>
    public class CompetentOrgGjiController : B4.Alt.DataController<CompetentOrgGji>
    {
        public ActionResult AddRevenueSource(BaseParams baseParams)
        {
            var service = Container.Resolve<ICompetentOrgGjiService>();
            try
            {
                var result = service.AddRevenueSource(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult AddContragents(BaseParams baseParams)
        {
            var service = Container.Resolve<ICompetentOrgGjiService>();
            try
            {
                var result = service.AddContragents(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}