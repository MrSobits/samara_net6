namespace Bars.B4.Modules.Analytics.Reports.Controllers
{
    using Bars.B4.Config;
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Modules.TaskManager.Contracts;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Переопределение контроллера b4
    /// Для сохранения истории при печати
    /// </summary>
    public class GkhPrintFormController : Alt.DataController<PrintForm>
    {
        public IPrintFormService PrintFormService { get; set; }

        public ActionResult AllPrintForms(BaseParams baseParams)
        {
            var result = this.PrintFormService.AllPrintForms(baseParams);
            return new JsonGetResult(result.Data);
        }

        public ActionResult ListPrintFormsClasses(BaseParams baseParams)
        {
            ListDataResult listDataResult = (ListDataResult)this.PrintFormService.ListPrintFormsClasses(baseParams);
            return new JsonNetResult(
                new
                {
                    success = true,
                    data = listDataResult.Data,
                    totalCount = listDataResult.TotalCount
                });
        }

        public ActionResult GetPrintFormResult(BaseParams baseParams)
        {
            var taskEnabled = false;

            var configProvider = this.Container.Resolve<IConfigProvider>();
            var config = configProvider.GetConfig();

            if (config.ModulesConfig.ContainsKey("Bars.B4.Modules.TaskManager"))
            {
                var moduleConfig = config.ModulesConfig["Bars.B4.Modules.TaskManager"];

                taskEnabled = moduleConfig.GetAs("enabled", false);
            }

            if (taskEnabled)
            {
                var taskFactory = this.Container.Resolve<ITaskFactory>();

                var taskId = taskFactory.SendTask("report", baseParams);

                return new JsonNetResult(new
                {
                    taskedReport = true,
                    taskId
                });
            }

            IDataResult printFormResult = this.PrintFormService.GetPrintFormResult(baseParams);
            return new JsonNetResult(new
            {
                taskedReport = false,
                fileId = printFormResult.Data
            });
        }
    }
}
