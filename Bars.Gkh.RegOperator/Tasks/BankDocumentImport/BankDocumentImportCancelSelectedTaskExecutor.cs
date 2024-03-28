namespace Bars.Gkh.RegOperator.Tasks.BankDocumentImport
{
    using Bars.Gkh.RegOperator.DomainService.BankDocumentImport;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using B4;
    using B4.Modules.Tasks.Common.Service;
    using B4.Utils;

    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;
    using DomainModelServices.PersonalAccount;
    using Entities;
    using Enums;
    using Gkh.Domain;
    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Executor для BankDocumentImport для отмены отдельных платежей в документе
    /// </summary>
    public class BankDocumentImportCancelSelectedTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<BankDocumentImport> BankDocumentImportDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        public IBankDocumentImportService BankDocImportService { get; set; }
        
        public IPersonalAccountRecalcEventManager RecalcEventManager { get; set; }

        public IChargePeriodRepository ChargePeriodRepo { get; set; }

        public string ExecutorCode { get; private set; }

        /// <summary>
        /// Метод для выполнения задачи
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <param name="ctx">Контекст</param>
        /// <param name="indicator">Индикатор</param>
        /// <param name="ct">Признак отмены</param>
        /// <returns></returns>
        public IDataResult Execute(BaseParams baseParams, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var bankDocumentImportId = baseParams.Params.GetAsId("bankDocumentImportId");
            var bankDocumentImport = this.BankDocumentImportDomain.GetAll().FirstOrDefault(x => x.Id == bankDocumentImportId && x.State != PaymentOrChargePacketState.Accepted);
            if (bankDocumentImport == null)
            {
                return new BaseDataResult(false, "Отсутствует документ загруженный из банка");
            }

            var chargePeriod = this.ChargePeriodRepo.GetCurrentPeriod();
            if (chargePeriod == null)
            {
                return new BaseDataResult(false, "Нет открытого периода");
            }

            var importedPaymentIds = baseParams.Params.GetAs<long[]>("importedPaymentIds", ignoreCase: true);
            IDataResult result = new BaseDataResult();

            try
            {
                if (importedPaymentIds.IsEmpty())
                {
                    return new BaseDataResult(false, "Необходимо выбрать записи для подтверждения");
                }

                return this.BankDocImportService.CancelInternalPayments(importedPaymentIds, bankDocumentImport, chargePeriod, indicator);
            }
            catch (ValidationException exception)
            {
                if (exception.EntityType == typeof(RealityObjectPaymentAccount))
                {
                    return BaseDataResult.Error(exception.Message);
                }

                // если это не ошибка на счете оплат дома, то пробрасываем со стек трейсом
                return BaseDataResult.Error(" message: {0}\r\n stacktrace: {1}".FormatUsing(exception.Message, exception.StackTrace));
            }
            catch (InvalidOperationException exception)
            {
                return BaseDataResult.Error(" message: {0}\r\n stacktrace: {1}".FormatUsing(exception.Message, exception.StackTrace));
            }
            catch (Exception exception)
            {
                return BaseDataResult.Error(" message: {0} {1}\r\n stacktrace: {2}".FormatUsing(exception.Message, exception.InnerException, exception.StackTrace));
            }
        }
    }
}
