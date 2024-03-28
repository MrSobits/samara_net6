namespace Bars.Gkh.RegOperator.Tasks.BankDocumentImport
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainService.BankDocumentImport;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    public class BankDocumentImportTaskCheckExecutor : ITaskExecutor
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Доменный сервис для документов из банка
        /// </summary>
        public IDomainService<BankDocumentImport> BankDocumentImportDomain { get; set; }

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Сервис для документов из банка
        /// </summary>
        public IBankDocumentImportService BankDocImportService { get; set; }

        /// <summary>
        /// </summary>
        public string ExecutorCode { get; private set; }

        /// <summary>
        /// Метод для выполнения задачи
        /// </summary>
        /// <param name="params">Параметры запроса</param>
        /// <param name="ctx">Контекст</param>
        /// <param name="indicator">Индикатор</param>
        /// <param name="ct">Признак отмены</param>
        /// <returns></returns>
        public IDataResult Execute(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var packetIds = @params.Params.GetAs<long[]>("packetIds", ignoreCase: true);
            IDataResult result = new BaseDataResult();

            try
            {
                if (packetIds.IsEmpty())
                {
                    return new BaseDataResult(false, "Необходимо выбрать записи для отмены подтверждения");
                }

                this.Container.InTransaction(
                    () =>
                        {
                            result =
                                this.BankDocImportService.CheckPayments(
                                    this.BankDocumentImportDomain.GetAll()
                                        .WhereContains(x => x.Id, packetIds)
                                        .Where(
                                            x =>
                                            x.PaymentConfirmationState == PaymentConfirmationState.Distributed
                                            || x.PaymentConfirmationState == PaymentConfirmationState.PartiallyDistributed)
                                        .ToList());
                        });
            }
            catch (ValidationException e)
            {
                return BaseDataResult.Error(" message: {0}\r\n stacktrace: {1}".FormatUsing(e.Message, e.StackTrace));
            }
            catch (InvalidOperationException e)
            {
                return BaseDataResult.Error(" message: {0}\r\n stacktrace: {1}".FormatUsing(e.Message, e.StackTrace));
            }
            catch (Exception e)
            {
                return BaseDataResult.Error(" message: {0} {1}\r\n stacktrace: {2}".FormatUsing(e.Message, e.InnerException, e.StackTrace));
            }

            return new BaseDataResult(result.Success, result.Message);
        }
    }
}