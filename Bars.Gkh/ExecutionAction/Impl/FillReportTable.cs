using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Tasks;
using System;

namespace Bars.Gkh.ExecutionAction.Impl
{
    public class FillReportTable : BaseExecutionAction
    {
        public override string Description => "Заполняет таблицу report_temp_table в базе";

        public override string Name => "Заполнение вспомогательной таблицы для отчетов";

        public override Func<IDataResult> Action => FillTable;

        private IDataResult FillTable()
        {
            var taskManager = Container.Resolve<ITaskManager>();

            try
            {
                taskManager.CreateTasks(new FillReportTableProvider(), new BaseParams());
                return new BaseDataResult(true, "Задача успешно поставлена");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                Container.Release(taskManager);
            }
        }
    }
}