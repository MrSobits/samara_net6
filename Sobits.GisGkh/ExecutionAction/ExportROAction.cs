namespace Sobits.GisGkh.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Sobits.GisGkh.Tasks.ProcessGisGkhAnswers;

    /// <summary>
    /// Действие Создание запросов на получение информации о домах из ГИС ЖКХ
    /// </summary>
    public class ExportROAction : BaseExecutionAction
    {
        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Создание запросов на получение информации о домах из ГИС ЖКХ";

        /// <summary>
        /// Название действия
        /// </summary>
        public override string Name => "Создание запросов на получение информации о домах из ГИС ЖКХ";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var taskManager = Container.Resolve<ITaskManager>();

            try
            {
                taskManager.CreateTasks(new ExportROTaskProvider(Container), new BaseParams());
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