﻿namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class PublishedProgramRecordController : B4.Alt.DataController<PublishedProgramRecord>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("PublishedProgramRecordExport");
            return export != null ? export.ExportData(baseParams) : null;
        }

    }
}