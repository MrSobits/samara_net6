// -----------------------------------------------------------------------
// <copyright file="ServiceOrganizationController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Entities;

    public class ServiceOrganizationController : B4.Alt.DataController<ServiceOrganization>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ServiceOrganizationDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}
