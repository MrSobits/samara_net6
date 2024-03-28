using Bars.B4;
using Bars.B4.IoC;
using Bars.GkhGji.Regions.Tatarstan.DomainService;
using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.ErknmTypeDocuments;
using Microsoft.AspNetCore.Mvc;

namespace Bars.GkhGji.Regions.Tatarstan.Controller
{
    public class ErknmTypeDocumentController : B4.Alt.DataController<ErknmTypeDocument>
    {
        public ActionResult ListWithoutPaging(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IErknmTypeDocumentsService>();
            using (this.Container.Using(service))
            {
                return service.ListWithoutPaging(baseParams);
            }
        }
    }
}