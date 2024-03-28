namespace Bars.GkhGji.Regions.Tatarstan.Controller.TatarstanProtocolGji
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.TatarstanProtocolGji;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class TatarstanProtocolGjiController : B4.Alt.DataController<TatarstanProtocolGji>
    {
        public ActionResult ExportToExcel(BaseParams baseParams)
        {
            var exporter = this.Container.Resolve<IDataExportService>("TatarstanProtocolGjiExporter");
            using (this.Container.Using(exporter))
            {
                return exporter.ExportData(baseParams);
            }
        }
        
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ITatarstanProtocolGjiService>();
            using (this.Container.Using(service))
            {
                var result = service.GetInfo(baseParams);
                return result.Success ? this.JsSuccess(result.Data) : this.JsFailure(result.Message);
            }
        }
    }
}
