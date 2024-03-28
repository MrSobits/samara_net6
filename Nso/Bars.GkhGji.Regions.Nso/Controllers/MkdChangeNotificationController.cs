namespace Bars.GkhGji.Regions.Nso.Controllers
{
	using Bars.B4;
	using Bars.B4.Modules.DataExport.Domain;
	using Bars.B4.Modules.FileStorage;
	using Bars.GkhGji.Regions.Nso.Entities;
	using Microsoft.AspNetCore.Mvc;
	using Bars.GkhGji.Regions.Nso.DomainService;

	public class MkdChangeNotificationController : FileStorageDataController<MkdChangeNotification>
    {
		public IMkdChangeNotificationService NotificationService { get; set; }

        public ActionResult Export(BaseParams baseParams)
        {
			var export = Container.Resolve<IDataExportService>("MkdChangeNotificationExport");
            return export != null ? export.ExportData(baseParams) : null;
        }

		public ActionResult GetManagingOrgDetails(BaseParams baseParams)
		{
			var result = NotificationService.GetManagingOrgDetails(baseParams);
			return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
		}

		public ActionResult GetManagingOrgByAddressName(BaseParams baseParams)
		{
			var result = NotificationService.GetManagingOrgByAddressName(baseParams);
			return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
		}
	}
}