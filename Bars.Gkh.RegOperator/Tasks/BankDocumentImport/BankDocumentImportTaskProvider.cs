namespace Bars.Gkh.RegOperator.Tasks.BankDocumentImport
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.Tasks.Common.Contracts;
    using B4.Modules.Tasks.Common.Contracts.Result;
    using B4.Modules.Tasks.Common.Service;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;

    /// <summary>
    /// Провайдер для BankDocumentImport
    /// </summary>
    public class BankDocumentImportTaskProvider : ITaskProvider
    {
        private readonly IWindsorContainer container;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        public BankDocumentImportTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Создать задачи
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Описатель задачи</returns>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var bankDocImportDomain = this.container.ResolveDomain<BankDocumentImport>();
            var ids = baseParams.Params.GetAs<long[]>("packetIds");
            var descriptors = new List<TaskDescriptor>();
            var description = "Подтверждаются реестры ({0} шт.)";
            using (this.container.Using(bankDocImportDomain))
            {
                description = description.FormatUsing(bankDocImportDomain.GetAll().Count(x => ids.Contains(x.Id)));
            }

            descriptors.Add(new TaskDescriptor(
                            "Подтверждение оплат в реестрах",
                            BankDocumentImportTaskExecutor.Id,
                            baseParams)
            {
                Description = description,
                Dependencies = new []
                {
                    new Dependency
                    {
                        Key = this.TaskCode,
                        Scope = DependencyScope.InsideGlobalTasks
                    } 
                },
                FailCallback = BankDocumentImportAcceptFailureCallback.Id
            });

            return new CreateTasksResult(descriptors.ToArray());
        }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string TaskCode { get { return "BankDocumentImportAccept"; } }
    }
}
