namespace Bars.Gkh.RegOperator.Export.Impl
{
    using B4;
    using B4.Modules.Tasks.Common.Service;
    using Tasks.Charges.Providers;

    /// <summary>
    /// 
    /// </summary>
    public class ChargeExportService : IChargeExportService
    {   /// <summary>
        /// Менеджер задач
        /// </summary>
        public ITaskManager TaskManager { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult Export(BaseParams baseParams)
        {
            return this.TaskManager.CreateTasks(new ChargesExportTaskProvider(), baseParams);
        }
    }
}