using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;
using Castle.Windsor;
using Fasterflect;

namespace Bars.GkhGji.Regions.Chelyabinsk.Tasks.ERKNMDictRequest
{
    /// <summary>
    /// Провайдер задачи на получение оплат в ГИС ГМП
    /// </summary>
    public class SendERKNMDictRequestProvider : ITaskProvider
    {
        private readonly IWindsorContainer container;

        public string TaskCode => "SendERKNMDictRequestProvider";

        public SendERKNMDictRequestProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Создает задачу на получение оплат в ГИС ГМП
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var @params = baseParams.Params.DeepClone();

            return new CreateTasksResult(
                new TaskDescriptor[] {
                    new TaskDescriptor(
                        "Запрос справочника из ЕРКНМ",
                        SendERKNMDictRequestExecutor.Id,
                           new BaseParams { Params = @params })
                }
            );
        }
    }
}
