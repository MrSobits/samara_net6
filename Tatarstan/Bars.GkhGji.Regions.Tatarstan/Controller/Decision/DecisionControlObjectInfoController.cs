namespace Bars.GkhGji.Regions.Tatarstan.Controller.Decision
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.Decision;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;

    public class DecisionControlObjectInfoController : B4.Alt.DataController<DecisionControlObjectInfo>
    {
        public ActionResult ListControlObjectKind(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDecisionControlObjectInfoService>();

            using (this.Container.Using(service))
            {
                var result = service.ListControlObjectKind(baseParams);
                return result.Success ? new JsonListResult((IList) result.Data) : JsonListResult.EmptyList;
            }
        }
        
        public ActionResult ListInspGjiRealityObject(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDecisionControlObjectInfoService>();
            
            using (this.Container.Using(service))
            {
                var result = service.ListInspGjiRealityObject(baseParams);
                return result.Success ? new JsonListResult((IList) result.Data) : JsonListResult.EmptyList;
            }
        }
    }
}