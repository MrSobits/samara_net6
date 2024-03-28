namespace Bars.Gkh.RegOperator.Tasks.BankDocumentImport
{
	using System.Collections.Generic;
	using B4;
	using B4.DataAccess;
	using B4.IoC;
	using B4.Modules.Tasks.Common.Contracts;
    using B4.Modules.Tasks.Common.Contracts.Result;
    using B4.Modules.Tasks.Common.Service;
	using B4.Utils;
	using Bars.Gkh.Domain;
	using Castle.Windsor;
	using Entities;

	/// <summary>
    /// Провайдер для BankDocumentImport для отмены отдельных платежей в документе
    /// </summary>
    public class BankDocumentImportCancelSelectedTaskProvider : ITaskProvider
    {
		private readonly IWindsorContainer container;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="container">Контейнер</param>
		public BankDocumentImportCancelSelectedTaskProvider(IWindsorContainer container)
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
			var bankDocImportDomain = container.ResolveDomain<BankDocumentImport>();
			var id = baseParams.Params.GetAsId("bankDocumentImportId");
		    var importedPaymentIds = baseParams.Params.GetAs<long[]>("importedPaymentIds", ignoreCase: true);
            var descriptors = new List<TaskDescriptor>();

			using (container.Using(bankDocImportDomain))
			{
                var bankImportDoc = bankDocImportDomain.Get(id);
                descriptors.Add(new TaskDescriptor(
                            "Отмена оплат",
                            BankDocumentImportCancelSelectedTaskExecutor.Id,
                            baseParams)
                {
                    Description = string.Format("Дата операции {0}. Дата реестра {1}. Номер реестра {2}. Количество оплат {3}",
                                bankImportDoc.ImportDate.ToShortDateString(),
                                bankImportDoc.DocumentDate != null ? bankImportDoc.DocumentDate.ToDateTime().ToShortDateString() : "",
                                bankImportDoc.DocumentNumber.ToStr(),
                                importedPaymentIds.Length)
                });
            }

			return new CreateTasksResult(descriptors.ToArray());
        }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string TaskCode { get { return "BankDocumentImportInternalCancel"; } }
    }
}
