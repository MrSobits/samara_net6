namespace Bars.Gkh.ClaimWork.Controllers.Document
{
    using System.ComponentModel;
    using Microsoft.AspNetCore.Mvc;
    using B4.Modules.DataExport.Domain;
    using Entities;
    using Bars.B4;

    using Microsoft.AspNetCore.Mvc;

    public class ActViolIdentificationClwController : B4.Alt.DataController<ActViolIdentificationClw>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ActViolIdentificationExport");
            try
            {
                return export.ExportData(baseParams);
            }
            finally
            {
                Container.Release(export);
            }
        }
    }
}