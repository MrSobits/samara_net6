namespace Bars.GisIntegration.UI.Service.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.GisIntegration.Base;
    using Bars.GisIntegration.UI.ViewModel;

    using Castle.Windsor;

    /// <summary>
    /// Сервис дерева задач
    /// </summary>
    public class TaskTreeService : ITaskTreeService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить результат триггера подготовки данных
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие идентификатор триггера или задачи</param>
        /// <returns>Результат выполнения операции, 
        /// содержащий результат подготовки данных</returns>
        public IDataResult GetPreparingDataTriggerResult(BaseParams baseParams)
        {
            var triggerId = baseParams.Params.GetAs<long>("triggerId");

            var taskId = baseParams.Params.GetAs<long>("taskId");

            if (triggerId == 0 && taskId == 0)
            {
                return new BaseDataResult(false, "Пустой идентификатор триггера и задачи.");
            }

            PrepareDataResult prepareDataResult;

            var taskManager = this.Container.Resolve<ITaskManager>();
            var packageViewModel = this.Container.Resolve<IPackageViewModel>();

            try
            {
                if (triggerId == 0)
                {
                    var trigger = taskManager.GetTaskPreparingDataTrigger(taskId);
                    triggerId = trigger.Id;
                }

                prepareDataResult = taskManager.GetPreparingDataTriggerResult(triggerId);

                if (prepareDataResult != null)
                {
                    return new BaseDataResult(
                        new
                        {
                            prepareDataResult.ValidateResult,
                            Packages = prepareDataResult.Packages.Select(
                                x => packageViewModel.GetPackageView(x)).ToList(),
                            prepareDataResult.UploadAttachmentsResult
                        });
                }
            }
            finally
            {
                this.Container.Release(taskManager);
                this.Container.Release(packageViewModel);
            }

            return new BaseDataResult(false, "Нет результата подготовки данных");
        }
    }
}