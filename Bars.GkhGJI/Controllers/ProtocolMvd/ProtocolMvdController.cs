namespace Bars.GkhGji.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Entities;

    public class ProtocolMvdController: ProtocolMvdController<ProtocolMvd>
    {
    }

    public class ProtocolMvdController<T> : B4.Alt.DataController<T>
        where T : ProtocolMvd
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ProtocolMvdDataExport");

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