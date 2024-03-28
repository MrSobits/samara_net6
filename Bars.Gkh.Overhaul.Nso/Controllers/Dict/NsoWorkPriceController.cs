namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;

    using Entities;

    // Перекрыт контроллер поскольку новая сущность прекрыла базовую
    public class NsoWorkPriceController : Bars.Gkh.Overhaul.Controllers.WorkPriceController<NsoWorkPrice>
    {
        public ActionResult ExportNso(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("NsoWorkPriceDataExport");
            try
            {
                return export != null ? export.ExportData(baseParams) : null;
            }
            finally
            {
                Container.Release(export);
            }
        }
    }
}