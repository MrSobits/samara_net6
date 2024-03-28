namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;

    using Entities;

    // Перекрыт контроллер поскольку новая сущность прекрыла базовую
    public class HmaoWorkPriceController : Bars.Gkh.Overhaul.Controllers.WorkPriceController<HmaoWorkPrice>
    {
        public ActionResult ExportHmao(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("HmaoWorkPriceDataExport");
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