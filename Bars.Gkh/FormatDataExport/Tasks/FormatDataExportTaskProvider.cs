namespace Bars.Gkh.FormatDataExport.Tasks
{
    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;

    /// <summary>
    /// Поставщик задачи на экспорт в формате 4.0.X
    /// </summary>
    public class FormatDataExportTaskProvider : ITaskProvider
    {
        /// <inheritdoc />
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            return new CreateTasksResult(new[]
            {
                new TaskDescriptor("Выгрузка в формате 4.0.5", this.TaskCode, baseParams)
                {
                    Dependencies = new[]
                    {
                        new Dependency
                        {
                            Scope = DependencyScope.InsideGlobalTasks,
                            Key = this.TaskCode
                        }
                    }
                }
            });
        }

        /// <inheritdoc />
        public string TaskCode => FormatDataExportTaskExecutor.Id;
    }
}