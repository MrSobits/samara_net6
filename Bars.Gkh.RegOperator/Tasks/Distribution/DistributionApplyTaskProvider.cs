namespace Bars.Gkh.RegOperator.Tasks.Distribution
{
    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;

    /// <summary>
    /// Поставщик задачи подтверждения банковских выписок
    /// </summary>
    public class DistributionApplyTaskProvider : ITaskProvider
    {
        private readonly string description;

        public DistributionApplyTaskProvider(string description)
        {
            this.description = description;
        }

        /// <inheritdoc />
        public CreateTasksResult CreateTasks(BaseParams @params)
        {
            return new CreateTasksResult(new[]
            {
                new TaskDescriptor("Распределение банковской операции", DistributionApplyTaskExecutor.Id, @params)
                {
                    Description = this.description,
                    Dependencies = new[]
                    {
                        new Dependency
                        {
                            Key = DistributionUndoTaskExecutor.Id,
                            Scope = DependencyScope.InsideExecutors
                        },
                        new Dependency
                        {
                            Key = DistributionApplyTaskExecutor.Id,
                            Scope = DependencyScope.InsideExecutors
                        },
                        new Dependency
                        {
                            Key = DistributionUndoPartiallyTaskExecutor.Id,
                            Scope = DependencyScope.InsideExecutors
                        }
                    }
                }
            });
        }

        /// <inheritdoc />
        public string TaskCode => nameof(DistributionApplyTaskProvider);
    }
}