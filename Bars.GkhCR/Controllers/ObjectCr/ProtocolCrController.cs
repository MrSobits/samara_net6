using Bars.B4.IoC;

namespace Bars.GkhCr.Controllers
{
	using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    public class ProtocolCrController : FileStorageDataController<ProtocolCr>
    {
        public ActionResult GetDates(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<IProtocolService>().GetDates(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetTypeDocumentCr(BaseParams baseParams)
        {
            var result = (ListDataResult)Container.Resolve<IProtocolService>().GetTypeDocumentCr(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

		public ActionResult AddTypeWorks(BaseParams baseParams)
		{
			var protocolService = Container.Resolve<IProtocolService>();
			using (Container.Using(protocolService))
			{
				var result = protocolService.AddTypeWorks(baseParams);
				return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
			}
		}
    }
}
