// -----------------------------------------------------------------------
// <copyright file="BelayOrganizationController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Entities;

    public class BelayOrganizationController : B4.Alt.DataController<BelayOrganization>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("BelayOrganizationDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}