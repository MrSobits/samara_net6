using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;
using Castle.Windsor;
using Fasterflect;


namespace Bars.GkhGji.Regions.Chelyabinsk.Tasks.EGRNSendInformationRequest
{
    /// <summary>
    /// Провайдер задачи на запрос данныъ из ЕГРН
    /// </summary>
    public class SendEGRNRequestTaskProvider : ITaskProvider
    {
        #region Fields

        private readonly IWindsorContainer container;

        #endregion

        #region Properties

        public string TaskCode => "SendEGRNRequestTaskProvider";

        #endregion

        #region Constructors

        public SendEGRNRequestTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Создает задачу на запрос данных из ЕГРН
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var @params = baseParams.Params.DeepClone();

            return new CreateTasksResult(
                new TaskDescriptor[] {
                    new TaskDescriptor(
                        "запрос данных из ЕГРН",
                        SendEGRNRequestTaskExecutor.Id,
                           new BaseParams { Params = @params })
                }
            );
        }

        #endregion
    }
}
