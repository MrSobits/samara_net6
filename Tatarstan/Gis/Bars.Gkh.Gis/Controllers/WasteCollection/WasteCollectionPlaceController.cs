namespace Bars.Gkh.Gis.Controllers.WasteCollection
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Modules.DataExport.Domain;
    using B4.Modules.FileStorage;
    using Entities.WasteCollection;

    public class WasteCollectionPlaceController: FileStorageDataController<WasteCollectionPlace>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("WasteCollectionDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}