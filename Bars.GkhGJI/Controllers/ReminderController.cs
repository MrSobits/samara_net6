namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.Gkh.Domain;

    /// <summary>
    /// Для панели руководителя и Доске задач Инспектора
    /// </summary>
    public class ReminderController : B4.Alt.DataController<Reminder>
    {
        public ActionResult ListAppealCitsReminder(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IExtReminderService>();
            try
            {
                return service.ListAppealCitsReminder(baseParams).ToJsonResult();
            }
            finally
            {
                this.Container.Release(service);
            }
        }
        public ActionResult ListWidgetInspector(BaseParams baseParams)
        {
            var service = Container.Resolve<IReminderService>();
            try
            {
                var result = (ListDataResult)service.ListWidgetInspector(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public ActionResult ListTypeReminder(BaseParams baseParams)
        {
            var service = Container.Resolve<IReminderService>();
            try
            {
                var result = (ListDataResult)service.ListTypeReminder(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ListTaskState(BaseParams baseParams)
        {
            var service = Container.Resolve<IReminderService>();
            try
            {
                var result = (ListDataResult)service.ListTaskState(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ListTaskControl(BaseParams baseParams)
        {
            var service = Container.Resolve<IReminderService>();
            try
            {
                var result = (ListDataResult)service.ListTaskControl(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ListReminderOfHead(BaseParams baseParams)
        {
            var service = Container.Resolve<IReminderService>();
            try
            {
                var result = (ListDataResult)service.ListReminderOfHead(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ListReminderOfInspector(BaseParams baseParams)
        {
            var service = Container.Resolve<IReminderService>();
            try
            {
                var result = (ListDataResult)service.ListReminderOfInspector(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ExportReminderOfInspector(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ReminderOfInspectorDataExport");
            try
            {
                return export != null ? export.ExportData(baseParams) : null;
            }
            finally
            {
                Container.Release(export);
            }
        }

        public ActionResult ExportReminderOfHead(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ReminderOfHeadDataExport");
            try
            {
                return export != null ? export.ExportData(baseParams) : null;
            }
            finally
            {
                Container.Release(export);
            }
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IReminderService>();
            try
            {
                var result = this.Container.Resolve<IReminderService>().GetInfo(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}
