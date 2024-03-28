namespace Sobits.GisGkh.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.ExecutionAction;
    using Sobits.GisGkh.Tasks.ProcessGisGkhAnswers;

    /// <summary>
    /// Действие Создание задачи на выгрузку договоров КПР в ГИС ЖКХ
    /// </summary>
    public class ImportBuildContractAction : BaseExecutionAction
    {
        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Создание задачи на выгрузку договоров КПР в ГИС ЖКХ";

        /// <summary>
        /// Название действия
        /// </summary>
        public override string Name => "Создание задачи на выгрузку договоров КПР в ГИС ЖКХ";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var taskManager = Container.Resolve<ITaskManager>();

            try
            {
                taskManager.CreateTasks(new ImportBuildContractTaskProvider(Container), new BaseParams());
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