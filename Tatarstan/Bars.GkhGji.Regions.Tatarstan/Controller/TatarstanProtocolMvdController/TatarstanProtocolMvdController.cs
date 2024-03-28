namespace Bars.GkhGji.Regions.Tatarstan.Controller.Resolution
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Controllers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;
    public class TatarstanProtocolMvdController : TatarstanProtocolMvdController<TatarstanProtocolMvd>
    {
    }
    public class TatarstanProtocolMvdController<T> : B4.Alt.DataController<T>
        where T : TatarstanProtocolMvd
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("TatarstanProtocolMvdDataExport");

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