namespace Bars.Gkh.RegOperator.Tasks.ExtractMerge
{
    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;

    /// <summary>
    /// </summary>
    public class ExtractMergeTaskProvider : ITaskProvider
    {
        //#region Fields

        //private readonly IWindsorContainer container;

        //#endregion

        #region Properties
        public string TaskCode => "ExtractMergeTaskProvider";
        #endregion

        //#region Constructors

        //public ExecutorTestTaskProvider(IWindsorContainer container)
        //{
        //    this.container = container;
        //}

        //#endregion

        #region Public methods
        /// <summary>
        /// Создает задачу на проверку ответов из СМЭВ
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            return new CreateTasksResult(
                new[]
                {
                    new TaskDescriptor(
                        "Сопоставление выписок из Росреестра",
                        ExtractMergeTaskExecutor.Id,
                        baseParams)
                });
        }
        #endregion
    }
}