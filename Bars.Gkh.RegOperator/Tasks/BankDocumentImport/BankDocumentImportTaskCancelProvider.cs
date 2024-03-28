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
    /// Провайдер для задач отмены реестров
    /// </summary>
    public class BankDocumentImportTaskCancelProvider : ITaskProvider
    {
        private readonly IWindsorContainer container;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        public BankDocumentImportTaskCancelProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Создать задачи
        /// </summary>
        /// <param name="params">Параметры</param>
        /// <returns>Описатель задачи</returns>
        public CreateTasksResult CreateTasks(BaseParams @params)
        {
            var bankDocImportDomain = this.container.ResolveDomain<BankDocumentImport>();
            var ids = @params.Params.GetAs<long[]>("packetIds");
            var descriptors = new List<TaskDescriptor>();

            var description = "Отменяются реестры ({0} шт.)";

            using (this.container.Using(bankDocImportDomain))
            {
                description = description.FormatUsing(bankDocImportDomain.GetAll().Count(x => ids.Contains(x.Id)));

                var args = DynamicDictionary.Create();
                args.SetValue("packetIds", ids);

                descriptors.Add(
                    new TaskDescriptor(
                        "Отмена подтверждения оплат",
                        BankDocumentImportTaskCancelExecutor.Id,
                        new BaseParams {Params = args})
                    {
                        Description = description,
                        Dependencies = new[]
                        {
                            new Dependency
                            {
                                Key = this.TaskCode,
                                Scope = DependencyScope.InsideGlobalTasks
                            }
                        }
                    });
            }

            return new CreateTasksResult(descriptors.ToArray());
        }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string TaskCode => "BankDocumentImportTaskCancel";
    }
}