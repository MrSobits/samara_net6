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

    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;
    using Entities;

    public class BankDocumentImportTaskCheckProvider : ITaskProvider
    {
        private readonly IWindsorContainer container;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        public BankDocumentImportTaskCheckProvider(IWindsorContainer container)
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
            var bankDocImportDomain = container.ResolveDomain<BankDocumentImport>();
            var ids = @params.Params.GetAs<long[]>("packetIds");
            var descriptors = new List<TaskDescriptor>();

            using (container.Using(bankDocImportDomain))
            {
                bankDocImportDomain.GetAll()
                                   .Where(
                                       x =>
                                       x.PaymentConfirmationState == PaymentConfirmationState.Distributed
                                       || x.PaymentConfirmationState == PaymentConfirmationState.PartiallyDistributed)
                                   .Where(x => ids.Contains(x.Id))
                                   .ForEach(
                                       x =>
                                           {
                                               var args = DynamicDictionary.Create();
                                               args.SetValue("packetIds", new[] { x.Id });

                                               descriptors.Add(
                                                   new TaskDescriptor(
                                                       "Проверка реестра оплат",
                                                       BankDocumentImportTaskCheckExecutor.Id,
                                                       new BaseParams { Params = args })
                                                       {
                                                           Description =
                                                               string.Format(
                                                                   "Дата операции {0}. Дата реестра {1}. Номер реестра {2}.",
                                                                   x.ImportDate.ToShortDateString(),
                                                                   x.DocumentDate != null
                                                                       ? x.DocumentDate.ToDateTime().ToShortDateString()
                                                                       : "",
                                                                   x.DocumentNumber.ToStr())
                                                       });
                                           });
            }

            return new CreateTasksResult(descriptors.ToArray());
        }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string TaskCode
        {
            get { return "BankDocumentImportTaskCheck"; }
        }
    }
}