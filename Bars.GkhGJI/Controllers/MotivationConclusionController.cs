namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Entities;

    public class MotivationConclusionController<T> : B4.Alt.DataController<T>
        where T: MotivationConclusion
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("MotivationConclusionDataExport");
            using (this.Container.Using(export))
            {
                return export?.ExportData(baseParams);
            }
        }
    }
}