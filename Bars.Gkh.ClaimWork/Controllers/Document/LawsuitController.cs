namespace Bars.Gkh.Controllers.Document
{
   using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Modules.DataExport.Domain;
    using B4.Modules.FileStorage;
    using Modules.ClaimWork.Entities;

    public abstract class LawsuitController<TLawsuit> : FileStorageDataController<TLawsuit>
        where TLawsuit : Lawsuit
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("LawsuitExport");
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

    public class LawsuitController : LawsuitController<Lawsuit>
    {
        
    }
}