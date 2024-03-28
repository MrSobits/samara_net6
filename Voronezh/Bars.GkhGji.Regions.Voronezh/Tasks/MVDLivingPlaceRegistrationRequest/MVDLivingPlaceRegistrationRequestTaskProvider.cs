using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;
using Castle.Windsor;
using Fasterflect;


namespace Bars.GkhGji.Regions.Voronezh.Tasks
{
    using Fasterflect;
    
    /// <summary>
    /// Провайдер задачи на запрос данныъ из ЕГРИП
    /// </summary>
    public class MVDLivingPlaceRegistrationRequestTaskProvider : ITaskProvider
    {
        #region Fields

        private readonly IWindsorContainer container;

        #endregion

        #region Properties

        public string TaskCode => "MVDLivingPlaceRegistrationRequestTaskProvider";

        #endregion

        #region Constructors

        public MVDLivingPlaceRegistrationRequestTaskProvider(IWindsorContainer container)
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
                        "Запрос сведений о регистрации",
                        MVDLivingPlaceRegistrationRequestTaskExecutor.Id,
                           new BaseParams { Params = @params })
                }
            );
        }

        #endregion
    }
}
