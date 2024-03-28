namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class RegopCalcAccountRealityObjectController : B4.Alt.DataController<CalcAccountRealityObject>
    {
        private readonly IRegopCalcAccountRealityObjectService _service;

        public RegopCalcAccountRealityObjectController(IRegopCalcAccountRealityObjectService service)
        {
            _service = service;
        }

        public ActionResult MassCreate(BaseParams baseParams)
        {
            return new JsonNetResult(_service.MassCreate(baseParams));
        }

        public ActionResult ListAccounts(BaseParams baseParams)
        {
            return new JsonNetResult(_service.ListAccounts(baseParams));
        }

        public ActionResult ListSpecialAccounts(BaseParams baseParams)
        {
            return new JsonNetResult(_service.ListSpecialAccounts(baseParams));
        }

        public ActionResult ListOperations(BaseParams baseParams)
        {
            return new JsonNetResult(_service.ListOperations(baseParams));
        }

        public ActionResult GetRegopMoneyInfo(BaseParams baseParams)
        {
            return new JsonNetResult(_service.GetRegopMoneyInfo(baseParams));
        }

        public ActionResult ListRealObjForRegopCalcAcc(BaseParams baseParams)
        {
            return new JsonNetResult(_service.ListRealObjForRegopCalcAcc(baseParams));
        }
    }
}