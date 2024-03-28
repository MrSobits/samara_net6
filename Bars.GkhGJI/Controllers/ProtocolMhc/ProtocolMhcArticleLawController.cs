namespace Bars.GkhGji.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ProtocolMhcArticleLawController : B4.Alt.DataController<ProtocolMhcArticleLaw>
    {
        public ActionResult AddArticles(BaseParams baseParams)
        {
            var service = Container.Resolve<IProtocolMhcArticleLawService>();
            try
            {
                var result = service.AddArticles(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }
    }
}