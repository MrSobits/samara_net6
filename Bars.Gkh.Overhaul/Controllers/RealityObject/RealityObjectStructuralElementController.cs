namespace Bars.Gkh.Overhaul.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using DomainService;
    using Entities;

    public class RealityObjectStructuralElementController : FileStorageDataController<RealityObjectStructuralElement>
    {
        public ActionResult IsRequiredStructElAdded(BaseParams baseParams)
        {
            var service = Container.Resolve<IStructuralElementService>();
            return new JsonNetResult(service.IsStructElForRequiredGroupsAdded(baseParams));
        }

        public ActionResult GetHistory(BaseParams baseParams)
        {
            var roSeService = Container.Resolve<IRealityObjectStructElService>();
            using (Container.Using(roSeService))
            {
                var result = (ListDataResult)roSeService.GetHistory(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);

            }
        }

        public ActionResult GetHistoryDetail(BaseParams baseParams)
        {
            var roSeService = Container.Resolve<IRealityObjectStructElService>();
            using (Container.Using(roSeService))
            {
                var result = (ListDataResult)roSeService.GetHistoryDetail(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);

            }
        }
    }
}