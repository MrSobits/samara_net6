namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Overhaul.Tat.DomainService;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class DpkrDocumentRealityObjectController: B4.Alt.DataController<DpkrDocumentRealityObject>
    {
        public ActionResult GetRealityObjectsList(BaseParams baseParams)
        {
            var dpkrDocumentRealityObjectService = this.Container.Resolve<IDpkrDocumentRealityObjectService>();

            using (this.Container.Using(dpkrDocumentRealityObjectService))
            {
                return new JsonNetResult(dpkrDocumentRealityObjectService.GetRealityObjectsList(baseParams));
            }
        }
        
        public ActionResult AddRealityObjects(BaseParams baseParams)
        {
            var dpkrDocumentRealityObjectService = this.Container.Resolve<IDpkrDocumentRealityObjectService>();

            using (this.Container.Using(dpkrDocumentRealityObjectService))
            {
                return new JsonNetResult(dpkrDocumentRealityObjectService.AddRealityObjects(baseParams));
            }
        }
    }
}