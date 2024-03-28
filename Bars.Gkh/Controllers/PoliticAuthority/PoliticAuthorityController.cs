namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class PoliticAuthorityController : B4.Alt.DataController<PoliticAuthority>
    {
        public ActionResult AddMunicipalities(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IPoliticAuthorityService>().AddMunicipalities(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IPoliticAuthorityService>().GetInfo(baseParams);
            if (result.Success)
            {
                return new JsonNetResult(result.Data);
            }

            return JsonNetResult.Failure(result.Message);
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("PoliticAuthorityDataExport");

            if (export != null)
            {
                return export.ExportData(baseParams);
            }

            return null;
        }
    }
}