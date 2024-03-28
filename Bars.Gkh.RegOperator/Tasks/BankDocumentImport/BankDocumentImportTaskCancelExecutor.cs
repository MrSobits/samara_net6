namespace Bars.Gkh.RegOperator.Tasks.BankDocumentImport
{
    using Bars.Gkh.RegOperator.DomainService.BankDocumentImport;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using B4;
    using B4.Modules.Tasks.Common.Service;
    using B4.Utils;
    using Castle.Windsor;
    using Domain;
    using Domain.Repository;
    using Domain.ValueObjects;
    using DomainModelServices;
    using DomainModelServices.PersonalAccount;
    using Entities;
    using Entities.ValueObjects;
    using Enums;
    using Gkh.Domain;
    using Gkh.Enums;
    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    public class BankDocumentImportTaskCancelExecutor : ITaskExecutor
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
        /// 
        /// </summary>
        public IPersonalAccountRecalcEventManager RecalcEventManager { get; set; }

        /// <summary>
        /// 
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

            try
            {

                if (packetIds.IsEmpty())
                {
                    return new BaseDataResult(false, "Необходимо выбрать записи для отмены подтверждения");
                }

                var bdImports =
                   this.BankDocumentImportDomain.GetAll()
                       .WhereContains(x => x.Id, packetIds)
                       .Where(x => (x.PersonalAccountDeterminationState == PersonalAccountDeterminationState.Defined && x.PaymentConfirmationState == PaymentConfirmationState.WaitingCancellation)
                                   || (x.PersonalAccountDeterminationState == PersonalAccountDeterminationState.PartiallyDefined && x.PaymentConfirmationState == PaymentConfirmationState.WaitingCancellation)
                       ).ToList();

                return this.BankDocImportService.CancelPayments(bdImports, indicator);
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
        }
    }
}    
