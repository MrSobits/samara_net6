namespace Bars.GkhGji.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Microsoft.AspNetCore.Mvc;

    public class EntityChangeLogRecordController : FileStorageDataController<EntityChangeLogRecord>
    {
        public ActionResult GetAppealHistory(BaseParams baseParams)
        {
            var logService = Container.Resolve<ILogEntityHistoryService>();
            try
            {
                return logService.GetAppealHistory(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }
    }
}