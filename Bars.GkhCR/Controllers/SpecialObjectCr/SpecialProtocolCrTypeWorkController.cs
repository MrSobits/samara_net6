namespace Bars.GkhCr.Controllers.ObjectCr
{
    using System.Collections;
    using B4;
    using DomainService;

    using Microsoft.AspNetCore.Mvc;

    public class SpecialProtocolCrTypeWorkController : B4.Alt.DataController<Entities.SpecialProtocolCrTypeWork>
    {
        protected ISpecialProtocolCrTypeWorkService Service;

        public SpecialProtocolCrTypeWorkController(ISpecialProtocolCrTypeWorkService protocolCrTypeWorkService)
        {
            this.Service = protocolCrTypeWorkService;
        }

        public ActionResult ListProtocolCrTypeWork(BaseParams baseParams)
        {
            var result = (ListDataResult) this.Service.ListProtocolCrTypeWork(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult AddTypeWorks(BaseParams baseParams)
        {
            var result = this.Service.AddTypeWorks(baseParams);
            return result.Success ? new JsonNetResult(result) : this.JsFailure(result.Message);
        }
    }
}
