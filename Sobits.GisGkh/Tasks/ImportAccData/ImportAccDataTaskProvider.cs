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
    /// Провайдер задачи на массовые запросы получения информации о ЛС из ГИС ЖКХ
    /// </summary>
    public class ImportAccDataTaskProvider : ITaskProvider
    {
        #region Fields

        private readonly IWindsorContainer container;

        #endregion

        #region Properties

        public string TaskCode => "ExportAccDataTaskProvider";

        #endregion

        #region Constructors

        public ImportAccDataTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Создает задачу на созданеие массовых запросов на получения информации о ЛС из ГИС ЖКХ
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var @params = baseParams.Params.DeepClone();

            return new CreateTasksResult(
                new TaskDescriptor[] {
                    new TaskDescriptor(
                        "Массовые запросы на выгрузку информации о ЛС в ГИС ЖКХ",
                        ImportAccDataTaskExecutor.Id,
                           new BaseParams { Params = @params })
                }
            );
        }

        #endregion
    }
}
