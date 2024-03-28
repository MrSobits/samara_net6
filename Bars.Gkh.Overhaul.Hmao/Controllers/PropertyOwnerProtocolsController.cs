namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Microsoft.AspNetCore.Mvc;
    public class PropertyOwnerProtocolsOperationsController : BaseController
    {
        public ActionResult Export(BaseParams baseParams)
        {
            IDataExportService export = null;
            try
            {
                export = this.Container.Resolve<IDataExportService>("PropertyOwnerProtocolsExport");
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