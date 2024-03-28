namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    // Заглушка для того чтобы в существующих регионах там где от него наследовались не полетело 
    public class ProtocolController : ProtocolController<Protocol>
    {
        // Внимание !! Код override нужно писать не в этом классе, а в ProtocolController<T>
    }

    // Класс переделан на для того чтобы в регионах можно было расширят ьсущность через subclass 
    // и при этом не писать дублирующий серверный код
    public class ProtocolController <T> : B4.Alt.DataController<T>
        where T: Protocol
    {
        public ActionResult GetInfo(long? documentId)
        {
            var result = Container.Resolve<IProtocolService>().GetInfo(documentId);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListView(BaseParams baseParams)
        {
            var result = (ListDataResult)Container.Resolve<IProtocolService>().ListView(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListForStage(BaseParams baseParams)
        {
            var result = (ListDataResult)Container.Resolve<IProtocolService>().ListForStage(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProtocolService>();

            using (this.Container.Using(service))
            {
                return service.Export(baseParams);
            }
        }
    }
}