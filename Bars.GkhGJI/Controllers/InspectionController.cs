namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;

    public sealed class InspectionController : BaseController
    {
        public ActionResult ListPersonInspection(BaseParams baseParams)
        {
            var service = Container.Resolve<IPersonInspectionService>();
            try
            {
                var result = (ListDataResult)service.GetPersonInspectionTypes(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ListJurPersonTypes(BaseParams baseParams)
        {
            var service = Container.Resolve<IJurPersonService>();
            try
            {
                var result = (ListDataResult)service.GetJurPersonTypes(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}