using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;
using Castle.Windsor;
using global::Quartz.Util;

namespace Sobits.GisGkh.Tasks.ProcessGisGkhAnswers
{
    using Fasterflect;

    /// <summary>
    /// Провайдер задачи на обработку ответов из ГИС ЖКХ
    /// </summary>
    public class ProcessGisGkhAnswersTaskProvider : ITaskProvider
    {
        #region Fields

        private readonly IWindsorContainer container;

        #endregion

        #region Properties

        public string TaskCode => "ProcessGisGkhAnswersTaskProvider";

        #endregion

        #region Constructors

        public ProcessGisGkhAnswersTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Создает задачу на обработку ответов из ГИС ЖКХ
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var @params = baseParams.Params.DeepClone();

            return new CreateTasksResult(
                new TaskDescriptor[] {
                    new TaskDescriptor(
                        "Обработка ответов из ГИС ЖКХ",
                        ProcessGisGkhAnswersTaskExecutor.Id,
                           new BaseParams { Params = @params })
                }
            );
        }

        #endregion
    }
}
