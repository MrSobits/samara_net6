namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class BaseJurPersonController : BaseJurPersonController<BaseJurPerson>
    {
    }

    public class BaseJurPersonController<T> : B4.Alt.DataController<T>
        where T : BaseJurPerson
    {
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = Container.Resolve<IBaseJurPersonService>().GetInfo(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetStartFilters()
        {
            var result = Container.Resolve<IBaseJurPersonService>().GetStartFilters();
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("BaseJurPersonDataExport");

            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}