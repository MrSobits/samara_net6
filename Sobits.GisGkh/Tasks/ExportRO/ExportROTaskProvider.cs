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
    /// Провайдер задачи на массовые запросы получения информации о домах из ГИС ЖКХ
    /// </summary>
    public class ExportROTaskProvider : ITaskProvider
    {
        #region Fields

        private readonly IWindsorContainer container;

        #endregion

        #region Properties

        public string TaskCode => "ExportROTaskProvider";

        #endregion

        #region Constructors

        public ExportROTaskProvider(IWindsorContainer container)
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
                        "Массовые запросы на получения информации о домах из ГИС ЖКХ",
                        ExportROTaskExecutor.Id,
                           new BaseParams { Params = @params })
                }
            );
        }

        #endregion
    }
}
