using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;

namespace Bars.Gkh.Tasks.ExecutorTest
{
    /// <summary>
    /// 
    /// </summary>
    public class GisSyncDictionariesTaskProvider : ITaskProvider
    {
        //#region Fields

        //private readonly IWindsorContainer container;

        //#endregion

        #region Properties

        public string TaskCode => "GisSyncDictionariesTaskProvider";

        #endregion

        //#region Constructors

        //public ExecutorTestTaskProvider(IWindsorContainer container)
        //{
        //    this.container = container;
        //}

        //#endregion

        #region Public methods

        /// <summary>
        /// Создает задачу на проверку ответов из СМЭВ
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            return new CreateTasksResult(
                new TaskDescriptor[] {
                    new TaskDescriptor(
                        "Синхронизация справочников ГИС",
                        GisSyncDictionariesTaskExecutor.Id,
                        baseParams
                        )
                }
            );
        }

        #endregion
    }
}
