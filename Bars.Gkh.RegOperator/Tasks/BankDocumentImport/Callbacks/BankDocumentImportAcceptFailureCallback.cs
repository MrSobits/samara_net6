namespace Bars.Gkh.RegOperator.Tasks.BankDocumentImport
{
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.DomainService.BankDocumentImport;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Хендлер неуспешного подтверждения оплат
    /// </summary>
    public class BankDocumentImportAcceptFailureCallback : ITaskCallback
    {
        /// <summary>
        /// Идентификатор коллбека
        /// </summary>
        public static string Id => nameof(BankDocumentImportAcceptFailureCallback);

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Интерфейс сервиса для документов, загруженных из банка
        /// </summary>
        public IBankDocumentImportService BankDocumentImportService { get; set; }

        /// <inheritdoc />
        public CallbackResult Call(long taskId, BaseParams baseParams, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var ids = baseParams.Params.GetAs<long[]>("packetIds");
            var documentDomain = this.Container.ResolveDomain<BankDocumentImport>();

            using (this.Container.Using(documentDomain))
            {
                var documents = documentDomain.GetAll().WhereContains(x => x.Id, ids);

                foreach (var document in documents)
                {
                    this.BankDocumentImportService.SetPaymentsNotDistributed(document);
                }
            }

            return new CallbackResult(true);
        }
    }
}