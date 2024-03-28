using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;
using Castle.Windsor;
using Fasterflect;


namespace Bars.GkhGji.Regions.Chelyabinsk.Tasks.EGRULSendInformationRequest
{
    /// <summary>
    /// Провайдер задачи на запрос данныъ из ЕГРЮЛ
    /// </summary>
    public class SendInformationRequestTaskProvider : ITaskProvider
    {
        #region Fields

        private readonly IWindsorContainer container;

        #endregion

        #region Properties

        public string TaskCode => "SendInformationRequestTaskProvider";

        #endregion

        #region Constructors

        public SendInformationRequestTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Создает задачу на запрос данныъ из ЕГРЮЛ
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var @params = baseParams.Params.DeepClone();

            return new CreateTasksResult(
                new TaskDescriptor[] {
                    new TaskDescriptor(
                        "Запрос данных из ЕГРЮЛ",
                        SendInformationRequestTaskExecutor.Id,
                           new BaseParams { Params = @params })
                }
            );
        }

        #endregion
    }
}
