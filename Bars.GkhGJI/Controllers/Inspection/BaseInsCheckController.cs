namespace Bars.GkhGji.Controllers
{

    using Microsoft.AspNetCore.Mvc;

    using B4;
    using B4.Modules.DataExport.Domain;
    using B4.Modules.FileStorage;
    using DomainService;
    using Entities;

    public class BaseInsCheckController: BaseInsCheckController<BaseInsCheck>
    {
    }

    public class BaseInsCheckController<T> : FileStorageDataController<T>
        where T: BaseInsCheck
    {
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var service = Container.Resolve<IBaseInsCheckService>();

            try
            {
                var result = service.GetInfo(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public ActionResult GetStartFilters()
        {
            var service = Container.Resolve<IBaseInsCheckService>();
            try
            {
                var result = service.GetStartFilters();
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("BaseInsCheckDataExport");
            try
            {
                return export != null ? export.ExportData(baseParams) : null;
            }
            finally 
            {
                Container.Release(export);
            }
        }
    }
}