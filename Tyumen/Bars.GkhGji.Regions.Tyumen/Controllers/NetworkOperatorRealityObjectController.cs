namespace Bars.GkhGji.Regions.Tyumen.Controllers
{
    using Bars.B4;
    using Bars.GkhGji.Regions.Tyumen.DomainService.NetworkOperator;
    using Bars.GkhGji.Regions.Tyumen.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class NetworkOperatorRealityObjectController : B4.Alt.DataController<NetworkOperatorRealityObject>
    {
        public INetworOperatorRealityObjectService Service { get; set; }

        /*public override ActionResult Create(BaseParams baseParams)
        {
            var result = Service.Save(baseParams);
            return new JsonNetResult(result);
        }

        public override ActionResult Update(BaseParams baseParams)
        {
            var result = Service.Update(baseParams);
            return new JsonNetResult(result);
        }*/

        public ActionResult SaveTechDecisions(BaseParams baseParams)
        {
            var result = Service.SaveTechDecisions(baseParams);
            return new JsonNetResult(result);
        }
    }
}