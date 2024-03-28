// -----------------------------------------------------------------------
// <copyright file="ServiceOrgServiceController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class ServiceOrgServiceController : B4.Alt.DataController<ServiceOrgService>
    {
        public ActionResult AddTypeServiceObjects(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IServiceOrgServService>().AddTypeServiceObjects(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}
