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
    /// Провайдер задачи на массовые запросы получения информации об организациях из ГИС ЖКХ
    /// </summary>
    public class ExportOrgTaskProvider : ITaskProvider
    {
        #region Fields

        private readonly IWindsorContainer container;

        #endregion

        #region Properties

        public string TaskCode => "ExportOrgTaskProvider";

        #endregion

        #region Constructors

        public ExportOrgTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Создает задачу на созданеие массовых запросов на получения информации об организациях из ГИС ЖКХ
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var @params = baseParams.Params.DeepClone();

            return new CreateTasksResult(
                new TaskDescriptor[] {
                    new TaskDescriptor(
                        "Массовые запросы на получения информации об организациях из ГИС ЖКХ",
                        ExportOrgTaskExecutor.Id,
                           new BaseParams { Params = @params })
                }
            );
        }

        #endregion
    }
}
