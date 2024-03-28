using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;

namespace Bars.Gkh.RegOperator
{
    /// <summary>
    /// 
    /// </summary>
    public class RecalcSaldoProvider : ITaskProvider
    {
        //#region Fields

        //private readonly IWindsorContainer container;

        //#endregion

        #region Properties

        public string TaskCode => "RecalcSaldoProvider";

        #endregion

        //#region Constructors

        //public ExecutorTestTaskProvider(IWindsorContainer container)
        //{
        //    this.container = container;
        //}

        //#endregion

        #region Public methods

        /// <summary>
        /// Создает задачу пересчет сальдо
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            return new CreateTasksResult(
                new TaskDescriptor[] {
                    new TaskDescriptor(
                        "Пересчет сальдо",
                        RecalcSaldoExecutor.Id,
                        baseParams
                        )
                }
            );
        }

        #endregion
    }
}
