using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;

namespace Bars.Gkh.Tasks.FiasUpdater
{
    /// <summary>
    /// 
    /// </summary>
    public class FiasUpdaterTaskProvider : ITaskProvider
    {
        #region Properties

        public string TaskCode => "FiasUpdaterTaskProvider";

        #endregion

        #region Public methods

        /// <summary>
        /// Создает задачу на проверку ответов из СМЭВ
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            return new CreateTasksResult(
                new TaskDescriptor[] {
                    new TaskDescriptor(
                        "Автоматическое обновление ФИАС",
                        FiasUpdaterTaskExecutor.Id,
                        baseParams
                        )
                }
            );
        }

        #endregion
    }
}
