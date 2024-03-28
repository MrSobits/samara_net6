namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Entities;
    using B4.Modules.FileStorage;
    using DomainService;

    public class ManagingOrganizationController : FileStorageDataController<ManagingOrganization>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("ManagingOrganizationDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }

        public ActionResult GetManOrgByContagentId(BaseParams baseParams)
        {
            var managingOrganizationService = Container.Resolve<IManagingOrganizationService>();
            try
            {
                var result = managingOrganizationService.GetManOrgByContagentId(baseParams);

                return new JsonGetResult(result.Data);
            }
            finally
            {
                Container.Release(managingOrganizationService);
            }
        }
    }
}