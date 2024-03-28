using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;
using Castle.Windsor;
using global::Quartz.Util;

namespace Bars.GkhCR.Tasks
{
    using Fasterflect;

    /// <summary>
    /// Провайдер задачи на отправку оплаты в ГИС ГМП
    /// </summary>
    public class CrFileRegisterTaskProvider : ITaskProvider
    {
        #region Fields

        private readonly IWindsorContainer container;

        #endregion

        #region Properties

        public string TaskCode => "CrFileRegisterTaskProvider";

        #endregion

        #region Constructors

        public CrFileRegisterTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Создает задачу на отправку оплаты в ГИС ГМП
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var @params = baseParams.Params.DeepClone();

            return new CreateTasksResult(
                new TaskDescriptor[] {
                    new TaskDescriptor(
                        "Формирование архива файлов",
                        CrFileRegisterTaskExecutor.Id,
                           new BaseParams { Params = @params })
                }
            );
        }

        #endregion
    }
}
