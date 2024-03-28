namespace Bars.GkhGji.Regions.Tatarstan.Controller.ResolPros
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Entities;

    public class ResolProsTatarstanController : ResolProsTatarstanController<ResolPros>
    {
    }

    public class ResolProsTatarstanController<T> : B4.Alt.DataController<T>
        where T : ResolPros
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ResolProsTatarstanDataExport");

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