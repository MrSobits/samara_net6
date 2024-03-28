namespace Bars.Gkh.RegOperator.Export.Reports.Impl
{
    using B4;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Castle.Windsor;
    using Tasks.Reports;

    /// <summary>
    /// Экспортер информации по лицевым счетам
    /// </summary>
    public class PersonalAccountInfoExportService : IPersonalAccountInfoExportService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }
        /// <summary>
        /// Менеджер задач
        /// </summary>
        public ITaskManager TaskManager { get; set; }

        /// <summary>
        /// Экспорт информации по ЛС
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения</returns>
        public IDataResult Export(BaseParams baseParams)
        {
            return this.TaskManager.CreateTasks(new PersonalAccountInfoExportTaskProvider(), baseParams);
        }
    }
}
