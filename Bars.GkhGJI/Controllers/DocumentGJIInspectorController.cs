namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class DocumentGjiInspectorController : B4.Alt.DataController<DocumentGjiInspector>
    {
        public ActionResult AddInspectors(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDocumentGjiInspectorService>();

            using (this.Container.Using(service))
            {
                var result = service.AddInspectors(baseParams);
                return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
            }
        }
    }
}