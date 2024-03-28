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
    
    using Castle.Windsor;
    using DomainModelServices.PersonalAccount;
    using Entities;
    using Enums;
    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;
    using System.Collections.Generic;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Authentification;

    /// <summary>
    /// Executor для BankDocumentImport
    /// </summary>
    public class BankDocumentImportTaskExecutor : ITaskExecutor
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
        /// Доменный сервис для документов из банка
        /// </summary>
        public IDomainService<ImportedPayment> ImportedPaymentDomain { get; set; }

        /// <summary>
        /// Доменный сервис для документов из банка
        /// </summary>
        public IDomainService<UnconfirmedPayments> UnconfirmedPaymentsDomain { get; set; }

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
        /// Домен сервис <see cref="LogOperation"/>
        /// </summary>
        public IDomainService<LogOperation> LogOperationDomainService { get; set; }

        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

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

            if (packetIds.IsEmpty())
            {
                return new BaseDataResult(false, "Необходимо выбрать записи для подтверждения");
            }

            var bdImports = this.BankDocumentImportDomain.GetAll()
                            .WhereContains(x => x.Id, packetIds)
                            .Where(x => x.State != PaymentOrChargePacketState.Accepted)
                            .Where(x => x.PaymentConfirmationState == PaymentConfirmationState.WaitingConfirmation)
                            .ToList();

            try
            {
                var dataresult = this.BankDocImportService.AcceptDocuments(bdImports, indicator);
                DateTime ds = DateTime.Now;
                //пытаемся пдтвердить онлайн оплаты
                try
                {
                    string log = "Подтверждаем оплаты.";
                    List <ImportedPaymentProxy> payments = ImportedPaymentDomain.GetAll()
                        .Where(x=> x.BankDocumentImport != null)
                        .Where(x => packetIds.ToList().Contains(x.BankDocumentImport.Id))
                        .Where(x => x.PersonalAccount != null)
                        .Select(x => new ImportedPaymentProxy
                        {
                            accId = x.PersonalAccount.Id,
                            PaymentDate = x.PaymentDate.ToShortDateString(),
                            Sum = x.Sum,
                            Regdata = $"{x.BankDocumentImport.DocumentNumber} от {x.BankDocumentImport.ImportDate.ToShortDateString()} ид {x.BankDocumentImport.Id.ToString()}"
                        }).ToList();

                    log += " Оплат в рестрах - " + payments.Count;

                    var paymentsList = UnconfirmedPaymentsDomain.GetAll()
                        .Where(x => x.IsConfirmed == Gkh.Enums.YesNo.No)
                        .Select(x=> new UnconfirmedPayments
                        {
                            Id = x.Id,
                            Guid = x.Guid,
                            BankBik = x.BankBik,
                            PaymentDate = x.PaymentDate,
                            PersonalAccount = new BasePersonalAccount{Id = x.PersonalAccount.Id },
                            BankName = x.BankName,
                            Description = x.Description,
                            File = x.File,
                            IsConfirmed = x.IsConfirmed,
                            ObjectCreateDate = x.ObjectCreateDate,
                            ObjectEditDate = x.ObjectEditDate,
                            ObjectVersion = x.ObjectVersion,
                            Sum = x.Sum
                        }).ToList();

                    log += ". Неподтвержденных - " + paymentsList.Count;
                    int conf = 0;
                    foreach (UnconfirmedPayments up in paymentsList)
                    {
                        var confPayment = payments.Where(x => x.accId == up.PersonalAccount.Id && decimal.Round(x.Sum, 2) == decimal.Round(up.Sum,2) && x.PaymentDate == up.PaymentDate.Value.ToShortDateString()).FirstOrDefault();
                        if (confPayment != null)
                        {
                            up.Description = $"Оплата подтверждена рееестром {confPayment.Regdata}";
                            up.IsConfirmed = Gkh.Enums.YesNo.Yes;
                            UnconfirmedPaymentsDomain.Update(up);
                            conf++;
                        }
                    }
                    log += ". Сопоставлено - " + conf;
                    this.LogOperationDomainService.Save(new LogOperation
                    {
                        OperationType = Gkh.Enums.LogOperationType.UnconfimmedPayments,
                        StartDate = ds,
                        EndDate = DateTime.Now,
                        Comment = log,
                        // LogFile = file,
                        User = this.UserManager.GetActiveUser()
                    });
                }
                catch(Exception e)
                {
                    this.LogOperationDomainService.Save(new LogOperation
                    {
                        OperationType = Gkh.Enums.LogOperationType.UnconfimmedPayments,
                        StartDate = ds,
                        EndDate = DateTime.Now,
                        Comment = $"При подтверждении оплат произошла ошибка {e.InnerException}",
                       // LogFile = file,
                        User = this.UserManager.GetActiveUser()
                    });
                }

                return dataresult;
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

    public class ImportedPaymentProxy
    {      
        public long accId { get; set; }

        public decimal Sum { get; set; }

        public string Regdata { get; set; }

        public string PaymentDate { get; set; }

    }
}
