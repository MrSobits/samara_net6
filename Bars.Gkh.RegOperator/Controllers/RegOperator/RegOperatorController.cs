namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using B4.IoC;
    using B4.Modules.DataExport.Domain;
    using B4.Utils;

    using Bars.Gkh.Modules.RegOperator.DomainService;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    public class RegOperatorController : B4.Alt.DataController<RegOperator>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("RegOperatorExport");
            using (this.Container.Using(export))
            {
                return export.ExportData(baseParams);
            }
        }

        public ActionResult GetCurrentRegop()
        {
            var service = this.Resolve<IRegopService>();
            using (this.Container.Using(service))
            {
                var regop = service.GetCurrentRegOperator();
                return new JsonNetResult(regop.Return(x => x.Contragent));
            }
        }
    }
}