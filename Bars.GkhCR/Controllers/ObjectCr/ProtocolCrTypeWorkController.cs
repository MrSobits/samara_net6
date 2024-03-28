namespace Bars.GkhCr.Controllers.ObjectCr
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections;
    using B4;
    using DomainService;

    public class ProtocolCrTypeWorkController : B4.Alt.DataController<Entities.ProtocolCrTypeWork>
    {
        protected IProtocolCrTypeWorkService Service;

        public ProtocolCrTypeWorkController(IProtocolCrTypeWorkService protocolCrTypeWorkService)
        {
            Service = protocolCrTypeWorkService;
        }

        public ActionResult ListProtocolCrTypeWork(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListProtocolCrTypeWork(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult AddTypeWorks(BaseParams baseParams)
        {
            var result = Service.AddTypeWorks(baseParams);
            return result.Success ? new JsonNetResult(result) : JsFailure(result.Message);
        }
    }
}
