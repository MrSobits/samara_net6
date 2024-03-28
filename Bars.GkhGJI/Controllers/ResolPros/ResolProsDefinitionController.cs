namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using System.ComponentModel;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ResolProsDefinitionController : FileStorageDataController<ResolProsDefinition>
    {
        public ActionResult ListTypeDefinition(BaseParams baseParams)
        {
            var service = Container.Resolve<IProtocolDefinitionService>();
            try
            {
                var result = (ListDataResult)service.ListTypeDefinition(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}