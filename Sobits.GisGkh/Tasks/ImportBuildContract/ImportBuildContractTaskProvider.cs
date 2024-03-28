using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;
using Castle.Windsor;

namespace Sobits.GisGkh.Tasks.ProcessGisGkhAnswers
{
    /// <summary>
    /// Провайдер задачи на массовую выгрузку договоров КПР в ГИС ЖКХ
    /// </summary>
    public class ImportBuildContractTaskProvider : ITaskProvider
    {
        private readonly IWindsorContainer container;

        public string TaskCode => "ImportBuildContractTaskProvider";

        public ImportBuildContractTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Создает задачу на массовую выгрузку договоров КПР в ГИС ЖКХ
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            return new CreateTasksResult(
                new TaskDescriptor[] {
                    new TaskDescriptor(
                        "Массовая выгрузка договоров КПР в ГИС ЖКХ",
                        ImportBuildContractTaskExecutor.Id,
                        baseParams)
                }
            );
        }
    }
}
