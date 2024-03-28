using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;
using Castle.Windsor;

namespace Bars.GkhGji.Regions.Voronezh.Tasks.EGRIPSendInformationRequest
{
    using Fasterflect;

    /// <summary>
    /// Провайдер задачи на запрос данныъ из ЕГРИП
    /// </summary>
    public class SendEGRIPRequestTaskProvider : ITaskProvider
    {
        #region Fields

        private readonly IWindsorContainer container;

        #endregion

        #region Properties

        public string TaskCode => "SendEGRIPRequestTaskProvider";

        #endregion

        #region Constructors

        public SendEGRIPRequestTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Создает задачу на запрос данных из ЕГРИП
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var @params = baseParams.Params.DeepClone();

            return new CreateTasksResult(
                new TaskDescriptor[] {
                    new TaskDescriptor(
                        "запрос данных из ЕГРИП",
                        SendEGRIPRequestTaskExecutor.Id,
                           new BaseParams { Params = @params })
                }
            );
        }

        #endregion
    }
}
