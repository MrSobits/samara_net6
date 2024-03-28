using Bars.B4.IoC;

namespace Bars.GkhCr.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class SpecialProtocolCrController : FileStorageDataController<SpecialProtocolCr>
    {
        public ActionResult GetDates(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ISpecialProtocolService>();
            using (this.Container.Using(service))
            {
                var result = (BaseDataResult) service.GetDates(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
        }

        public ActionResult GetTypeDocumentCr(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ISpecialProtocolService>();
            using (this.Container.Using(service))
            {
                var result = (ListDataResult) this.Container.Resolve<ISpecialProtocolService>().GetTypeDocumentCr(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
        }

        public ActionResult AddTypeWorks(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ISpecialProtocolService>();
            using (this.Container.Using(service))

            using (this.Container.Using(service))
            {
                var result = service.AddTypeWorks(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
        }
    }
}

