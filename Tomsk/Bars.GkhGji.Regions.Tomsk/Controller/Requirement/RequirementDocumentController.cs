namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using System.Collections;

    using Bars.GkhGji.Regions.Tomsk.Entities;

    using DomainService;

    public class RequirementDocumentController : B4.Alt.DataController<RequirementDocument>
    {
        public IRequirementDocumentService ReqDocService { get; set; }

        public ActionResult CreateProtocol(BaseParams baseParams)
        {
            var result = ReqDocService.CreateProtocol(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
        }
    }
}
