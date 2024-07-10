using Bars.B4.Modules.DataExport.Domain;

namespace Bars.Gkh.Controllers.Administration
{
    using Bars.B4;

    using Microsoft.AspNetCore.Mvc;
    public class ExportController : BaseController
    {
        public ActionResult GetManagementSysReport(BaseParams baseParams)
        {
            //var result = Container.Resolve<IExportService>().GetManagementSys(baseParams);

            //if (result.Success == false)
            //{
            //    return JsonNetResult.Failure(result.Message);
            //}

            //return new FileStreamResult((MemoryStream)result.Data, "application/vnd.ms-excel") { FileDownloadName = "Passport.xlsx" };

            var export = Container.Resolve<IDataExportService>("ManagementSysExport");

            if (export != null)
            {
                return export.ExportData(baseParams);
            }

            return null;
        }
    }
}
