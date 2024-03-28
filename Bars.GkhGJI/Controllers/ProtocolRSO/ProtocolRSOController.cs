namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Entities;

    public class ProtocolRSOController : ProtocolRSOController<ProtocolRSO>
    {
    }

    public class ProtocolRSOController<T> : B4.Alt.DataController<T>
        where T : ProtocolRSO
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ProtocolRSODataExport");

            try
            {
                if (export != null)
                {
                    return export.ExportData(baseParams);
                }

                return null;
            }
            finally 
            {
                Container.Release(export);
            }
        }
    }
}