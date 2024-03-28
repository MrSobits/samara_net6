using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;
using Castle.Windsor;
using Fasterflect;

namespace Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers
{
    /// <summary>
    /// Провайдер задачи на проверку ответов из ГИС ГМП (потенциально из всего СМЭВ)
    /// </summary>
    public class GetSMEV2AnswersTaskProvider : ITaskProvider
    {
        #region Fields

        private readonly IWindsorContainer container;

        #endregion

        #region Properties

        public string TaskCode => "GetSMEV2AnswersTaskProvider";

        #endregion

        #region Constructors

        public GetSMEV2AnswersTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Создает задачу на проверку ответов из СМЭВ
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var @params = baseParams.Params.DeepClone();

            return new CreateTasksResult(
                new TaskDescriptor[] {
                    new TaskDescriptor(
                        "Проверка наличия ответов из СМЭВ2",
                        GetSMEV2AnswersTaskExecutor.Id,
                           new BaseParams { Params = @params })
                }
            );
        }

        #endregion
    }
}
