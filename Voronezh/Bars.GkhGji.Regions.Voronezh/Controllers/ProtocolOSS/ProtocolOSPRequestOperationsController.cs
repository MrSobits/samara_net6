namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Entities;
    using Bars.B4;
    using DomainService;
    using Bars.GkhGji.Entities;
    using System.Collections;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;

    public class ProtocolOSPRequestOperationsController : BaseController
    {
        public IProtocolOSPRequestOperationsService service { get; set; }
        public ActionResult SendAnswer(BaseParams baseParams)
        {
            var result = service.SendAnswer(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }           

        public ActionResult GetDocInfo(BaseParams baseParams)
        {
            try
            {
                var result = service.GetDocInfo(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {

            }
        }

        public ActionResult Export(BaseParams baseParams)
        {
            IDataExportService export = null;
            try
            {
                export = this.Container.Resolve<IDataExportService>("ProtocolOSPRequestDataExport");
                return export.ExportData(baseParams);
            }
            finally
            {
                if (export != null)
                {
                    this.Container.Release(export);
                }
            }
        }
    }
 
}