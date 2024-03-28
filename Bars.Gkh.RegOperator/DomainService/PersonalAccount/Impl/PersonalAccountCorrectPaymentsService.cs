namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.DataResult;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    /// <summary>
    /// Сервис корректировки оплат ЛС
    /// </summary>
    public class PersonalAccountCorrectPaymentsService : IPersonalAccountCorrectPaymentsService
    {
        #region Public properties
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PersonalAccountPeriodSummary"/>
        /// </summary>
        public IDomainService<PersonalAccountPeriodSummary> PersonalAccountPeriodSummaryDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="BasePersonalAccount"/>
        /// </summary>
        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Transfer"/>
        /// </summary>
        public ITransferDomainService TransferDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="MoneyOperation"/>
        /// </summary>
        public IDomainService<MoneyOperation> MoneyOperationDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PaymentCorrectionSource"/>
        /// </summary>
        public IDomainService<PaymentCorrectionSource> PaymentCorrectionSourceDomain { get; set; }

        /// <summary>
        /// Интерфейс менеджера пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Сессия обновления счетов дома. Агрегирует оплаты по разным ЛС
        /// </summary>
        public IRealtyObjectPaymentSession PaymentSession { get; set; }

        /// <summary>
        /// Интерфейс создания отсечек перерасчета для ЛС
        /// </summary>
        public IPersonalAccountRecalcEventManager RecalcEventManager { get; set; }

        /// <summary>
        /// Репозиторий периода начислений
        /// </summary>
        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        /// <summary>
        /// Менеджер файлов
        /// </summary>
        public IFileManager FileManager { get; set; }
        #endregion

        /// <summary>
        /// Произвести корректировку оплат
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult MovePayments(BaseParams baseParams)
        {
            var parameters = this.ExtractProxyFromArg(baseParams);
            var validateResult = this.Validate(parameters);
            if (!validateResult.Success)
            {
                return validateResult;
            }

            IDataResult result = null;
            this.Container.InTransaction(() =>
                {
                    if (baseParams.Files.Any())
                    {
                        var file = baseParams.Files.FirstOrDefault().Value;
                        var fileInfo = this.FileManager.SaveFile(file);

                        parameters.Document = fileInfo;
                    }

                    result = this.MovePaymentsInternal(parameters);
                });

            return result;
        }

        /// <summary>
        /// Вернуть данные по оплатам/задолженностям
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult GetAccountPaymentInfo(BaseParams baseParams)
        {
            var paId = baseParams.GetLoadParam().Filter.GetAsId("persAccId");

            var persAccSummaries = this.PersonalAccountPeriodSummaryDomain.GetAll()
               .Where(x => x.PersonalAccount.Id == paId)
               .ToList();

            var listResult = new List<WalletInfoProxy>
            {
                WalletInfoProxy.Create(WalletType.BaseTariffWallet, persAccSummaries, x => x.TariffPayment, x => x.GetBaseTariffDebt()),
                WalletInfoProxy.Create(WalletType.DecisionTariffWallet, persAccSummaries, x => x.TariffDecisionPayment, x => x.GetDecisionTariffDebt()),
                WalletInfoProxy.Create(WalletType.PenaltyWallet, persAccSummaries, x => x.PenaltyPayment, x => x.GetPenaltyDebt())
            };

            return new ListSummaryResult(listResult, listResult.Count, new
            {
                Payment = listResult.Sum(x => x.Payment),
                Debt = listResult.Sum(x => x.Debt),
                TakeAmount = 0,
                EnrollAmount = 0
            });
        }

        private IDataResult MovePaymentsInternal(CorrectPaymentPersAccProxy correction)
        {
            var correctionPaymentsSource = new PaymentCorrectionSource(this.ChargePeriodRepository.GetCurrentPeriod())
            {
                DocumentDate = correction.DocumentDate,
                PersonalAccount = correction.PersonalAccount,
                DocumentNumber = correction.DocumentNumber,
                Document = correction.Document,
                OperationDate = correction.OperationDate,
                Reason = correction.Reason,
                User = this.UserManager.GetActiveUser()?.Login ?? "anonymous"
            };

            var correctionInfo = correction.Payments.Select(
                x => new PersonalAccountCorrectionInfo
                {
                    PaymentType = x.PaymentType,
                    TakeAmount = x.TakeAmount,
                    EnrollAmount = x.EnrollAmount
                }).Where(x => x.Amount != 0).ToList();

            try
            {
                var result = correctionPaymentsSource.ApplyCorrection(correctionInfo);

                this.MoneyOperationDomain.Save(result.Operation);
                result.Transfers.ForEach(this.TransferDomain.Save);
                this.PersonalAccountPeriodSummaryDomain.Save(correction.PersonalAccount.GetOpenedPeriodSummary());
                this.PaymentCorrectionSourceDomain.Save(correctionPaymentsSource);
                this.RecalcEventManager.SaveEvents();
                this.PaymentSession.Complete();

                return new BaseDataResult();
            }
            catch
            {
                this.PaymentSession.Rollback();
                throw;
            }
        }


        private CorrectPaymentPersAccProxy ExtractProxyFromArg(BaseParams baseParams)
        {
            var result = (CorrectPaymentPersAccProxy)baseParams.Params.ReadClass(typeof(CorrectPaymentPersAccProxy));
            result.PersonalAccount = this.PersonalAccountDomain.Get(baseParams.Params.GetAsId("persAccId"));

            return result;
        }

        private IDataResult Validate(CorrectPaymentPersAccProxy correctPaymentPersAccProxy)
        {
            if (correctPaymentPersAccProxy.Payments.IsEmpty() || correctPaymentPersAccProxy.Payments.SafeSum(x => x.EnrollAmount) == 0 || correctPaymentPersAccProxy.Payments.SafeSum(x => x.TakeAmount) == 0)
            {
                return BaseDataResult.Error("Необходимо заполнить значения для корректировки оплат");
            }

            if (correctPaymentPersAccProxy.Payments.SafeSum(x => x.TakeAmount) != correctPaymentPersAccProxy.Payments.SafeSum(x => x.TakeAmount))
            {
                return BaseDataResult.Error("Сумма снятия не равна сумме зачисления!");
            }

            if (correctPaymentPersAccProxy.OperationDate.Date > DateTime.Now.Date)
            {
                return BaseDataResult.Error("Дата операции не может быть больше текущей даты");
            }

            var persAccSummaries = this.PersonalAccountPeriodSummaryDomain.GetAll()
                .Where(x => x.PersonalAccount.Id == correctPaymentPersAccProxy.PersonalAccount.Id)
                .ToList();

            var walletGuids = new Dictionary<string, WalletType>
            {
                {correctPaymentPersAccProxy.PersonalAccount.BaseTariffWallet.WalletGuid, WalletType.BaseTariffWallet },
                {correctPaymentPersAccProxy.PersonalAccount.DecisionTariffWallet.WalletGuid, WalletType.DecisionTariffWallet },
                {correctPaymentPersAccProxy.PersonalAccount.PenaltyWallet.WalletGuid, WalletType.PenaltyWallet }
            };

            var paymentsDict = new Dictionary<WalletType, decimal>
            {
                { WalletType.BaseTariffWallet, persAccSummaries.SafeSum(x => x.TariffPayment) },
                { WalletType.DecisionTariffWallet, persAccSummaries.SafeSum(x => x.TariffDecisionPayment) },
                { WalletType.PenaltyWallet, persAccSummaries.SafeSum(x => x.PenaltyPayment) }
            };

            var transferGuids = walletGuids.Keys.ToArray();
            var firstPaymentDict = this.TransferDomain.GetAll<PersonalAccountPaymentTransfer>()
                .Where(x => x.Owner.Id == correctPaymentPersAccProxy.PersonalAccount.Id && transferGuids.Contains(x.TargetGuid))
                .Where(x => !x.Operation.IsCancelled && x.IsAffect)
                .Where(x => x.Reason.ToLower().Contains("оплата") ||
                    x.Reason.ToLower().Contains("корректировка оплат") ||
                    x.Reason.ToLower().Contains("перенос долга") && x.Amount < 0)
                .AsEnumerable()
                .GroupBy(x => x.TargetGuid)
                .ToDictionary(x => walletGuids[x.Key], y => y.OrderBy(x => x.PaymentDate).FirstOrDefault()?.PaymentDate);

            foreach (var paymentTransfer in correctPaymentPersAccProxy.Payments)
            {
                if (paymentTransfer.TakeAmount > 0 && paymentTransfer.EnrollAmount > 0)
                {
                    return BaseDataResult.Error("Необходимо заполнить верные значения корректировки оплат");
                }

                if (paymentTransfer.TakeAmount > 0)
                {
                    if (!firstPaymentDict.ContainsKey(paymentTransfer.PaymentType))
                    {
                        return BaseDataResult.Error($"Отсутствуют оплаты {paymentTransfer.PaymentType.GetDisplayName().ToLower()}");
                    }

                    if (correctPaymentPersAccProxy.OperationDate.Date < firstPaymentDict[paymentTransfer.PaymentType])
                    {
                        return BaseDataResult.Error($"Дата операции не может быть меньше даты первой оплаты {paymentTransfer.PaymentType.GetDisplayName().ToLower()}");
                    }

                    if (paymentTransfer.TakeAmount > paymentsDict[paymentTransfer.PaymentType])
                    {
                        return BaseDataResult.Error($"Сумма корректировки {paymentTransfer.PaymentType.GetDisplayName().ToLower()} превышает сумму оплат");
                    }
                }
            }

            return new BaseDataResult();
        }

        #region Proxies
        private class CorrectPaymentPersAccProxy
        {
            public BasePersonalAccount PersonalAccount { get; set; }

            public string DocumentNumber { get; set; }

            public DateTime? DocumentDate { get; set; }

            public DateTime OperationDate { get; set; }

            public string Reason { get; set; }

            public FileInfo Document { get; set; }

            public IEnumerable<PersonalAccountCorrectionInfo> Payments { get; set; }
        }

        private class WalletInfoProxy
        {
            public WalletType PaymentType { get; set; }
            public decimal Payment { get; set; }
            public decimal Debt { get; set; }
            public decimal? TakeAmount { get; set; }
            public decimal? EnrollAmount { get; set; }

            public static WalletInfoProxy Create(
                WalletType walletType, 
                IList<PersonalAccountPeriodSummary> summaries, 
                Func<PersonalAccountPeriodSummary, decimal> paymentSelector, 
                Func<PersonalAccountPeriodSummary, decimal> debtSelector)
            {
                return new WalletInfoProxy
                {
                    PaymentType = walletType,
                    Payment = summaries.SafeSum(paymentSelector),
                    Debt = summaries.SafeSum(debtSelector)
                };
            }
        }
        #endregion
    }
}