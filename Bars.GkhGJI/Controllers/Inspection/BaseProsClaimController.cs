namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class BaseProsClaimController : BaseProsClaimController<BaseProsClaim>
    {
    }
    
    public class BaseProsClaimController<T> : FileStorageDataController<T>
        where T : BaseProsClaim
    {
        
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var service = Container.Resolve<IBaseProsClaimService>();
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

        public ActionResult ChangeInspState(BaseParams baseParams)
        {
            var service = Container.Resolve<IBaseProsClaimService>();
            try
            {
                var result = service.ChangeInspState(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("BaseProsClaimDataExport");

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
