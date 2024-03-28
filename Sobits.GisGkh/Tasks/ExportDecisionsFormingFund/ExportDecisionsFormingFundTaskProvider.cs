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
    /// Провайдер задачи на массовые запросы получения решений о формировании ФКР из ГИС ЖКХ
    /// </summary>
    public class ExportDecisionsFormingFundTaskProvider : ITaskProvider
    {
        #region Fields

        private readonly IWindsorContainer container;

        #endregion

        #region Properties

        public string TaskCode => "ExportDecisionsFormingFundTaskProvider";

        #endregion

        #region Constructors

        public ExportDecisionsFormingFundTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Создает задачу на созданеие массовых запросов на получения решений о формировании ФКР из ГИС ЖКХ
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var @params = baseParams.Params.DeepClone();

            return new CreateTasksResult(
                new TaskDescriptor[] {
                    new TaskDescriptor(
                        "Массовые запросы на получения решений о формировании ФКР из ГИС ЖКХ",
                        ExportDecisionsFormingFundTaskExecutor.Id,
                           new BaseParams { Params = @params })
                }
            );
        }

        #endregion
    }
}
