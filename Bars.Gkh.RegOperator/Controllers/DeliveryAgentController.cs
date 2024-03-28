namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class DeliveryAgentController : B4.Alt.DataController<DeliveryAgent>
    {
        public IDeliveryAgentService Service { get; set; }

        public ActionResult ListWithoutPaging(BaseParams baseParams)
        {
            var result = (ListDataResult)this.Container.Resolve<IDeliveryAgentService>().ListWithoutPaging(baseParams);

            if (result.Success)
            {
                return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
            }

            return JsonNetResult.Message(result.Message);
        }

        public ActionResult AddMunicipalities(BaseParams baseParams)
        {
            var result = Service.AddMunicipalities(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddRealityObjects(BaseParams baseParams)
        {
            var result = Service.AddRealityObjects(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListRealObjForDelAgent(BaseParams baseParams)
        {
            return new JsonNetResult(Service.ListRealObjForDelAgent(baseParams));
        }
    }
}