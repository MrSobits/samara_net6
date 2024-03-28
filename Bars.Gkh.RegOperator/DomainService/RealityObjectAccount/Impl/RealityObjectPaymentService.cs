namespace Bars.Gkh.RegOperator.DomainService.RealityObjectAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Utils;
    using B4.Utils.Annotations;

    using Bars.Gkh.RegOperator.Entities.Loan;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Entities;
    using Enums;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;

    public class RealityObjectPaymentService : IRealityObjectPaymentService
    {
        public RealityObjectPaymentService(
            IDomainService<RealityObjectPaymentAccountOperation> paymentOperDomain,
            IDomainService<RealityObjectPaymentAccount> paymentAccDomain,
            IDomainService<RealityObjectSubsidyAccountOperation> subsidyAccOperDomain,
            IDomainService<RealityObjectSubsidyAccount> subsidyAccDomain,
            IDomainService<RealObjLoanPaymentAccOper> roLoanPaymentAccOperDomain,
            IDomainService<PaymentSrcFinanceDetails> paymSrcFinanceDetailsDomain,
            IDomainService<RealObjPaymentAccOperPerfAct> roPaymAccOperPerfActDomain)
        {
            _paymentOperDomain = paymentOperDomain;
            _paymentAccDomain = paymentAccDomain;
            _subsidyAccOperDomain = subsidyAccOperDomain;
            _subsidyAccDomain = subsidyAccDomain;
            _roLoanPaymentAccOperDomain = roLoanPaymentAccOperDomain;
            _paymSrcFinanceDetailsDomain = paymSrcFinanceDetailsDomain;
            _roPaymAccOperPerfActDomain = roPaymAccOperPerfActDomain;
        }

        public IDictionary<long, AccountOperationsInfo> GetDebetOperationsSum(
            long? mrId, long? moId, long[] roIds, DateTime? startDate = null, DateTime? endDate = null)
        {
            startDate = startDate ?? DateTime.MinValue;
            endDate = endDate ?? DateTime.MaxValue;

            // получаю операции по счету оплат только с тем типом,
            // по которым буду считать сумму операции для столбцов дебет (субсидии, пени, взносы)
            var paymAccOpers =
                _paymentOperDomain.GetAll()
                    .WhereIf(mrId != null, x => x.Account.RealityObject.Municipality.Id == mrId)
                    .WhereIf(moId != null, x => x.Account.RealityObject.Municipality.Id == moId)
                    .WhereIf(roIds.Any(), x => roIds.Contains(x.Account.RealityObject.Id))
                    .Where(x => x.OperationType == PaymentOperationType.IncomeByDecisionTariff
                        || x.OperationType == PaymentOperationType.IncomeByMinTariff
                        || x.OperationType == PaymentOperationType.IncomePenalty
                        || x.OperationType == PaymentOperationType.IncomeFundSubsidy
                        || x.OperationType == PaymentOperationType.IncomeRegionalSubsidy
                        || x.OperationType == PaymentOperationType.IncomeStimulateSubsidy
                        || x.OperationType == PaymentOperationType.IncomeGrantInAid
                        || x.OperationType == PaymentOperationType.OtherSources);

            return
                paymAccOpers.Where(x => x.Date >= startDate && x.Date <= endDate)
                    .AsEnumerable()
                    .GroupBy(x => new { x.Account.RealityObject.Id, x.OperationType })
                    .Select(x => new { x.Key, Sum = x.SafeSum(y => y.OperationSum) })
                    .GroupBy(x => x.Key.Id)
                    .ToDictionary(x => x.Key, x => new AccountOperationsInfo
                    {
                        Payments = x.Where(y => y.Key.OperationType == PaymentOperationType.IncomeByDecisionTariff
                            || y.Key.OperationType == PaymentOperationType.IncomeByMinTariff)
                            .SafeSum(y => y.Sum),
                        Penalty = x.Where(y => y.Key.OperationType == PaymentOperationType.IncomePenalty)
                            .SafeSum(y => y.Sum),
                        Subsidy = x.Where(y => y.Key.OperationType == PaymentOperationType.IncomeFundSubsidy
                            || y.Key.OperationType == PaymentOperationType.IncomeRegionalSubsidy
                            || y.Key.OperationType == PaymentOperationType.IncomeStimulateSubsidy
                            || y.Key.OperationType == PaymentOperationType.IncomeGrantInAid)
                            .SafeSum(y => y.Sum),
                        Other = x.Where(y => y.Key.OperationType == PaymentOperationType.OtherSources)
                            .SafeSum(y => y.Sum)
                    });
        }

        public IDictionary<long, AccountOperationsInfo> GetCreditOperationsSum(
            long? mrId,
            long? moId,
            long[] roIds,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTime.MinValue;
            }
            if (!endDate.HasValue)
            {
                endDate = DateTime.MaxValue;
            }

            // вычисляю данные по займам
            var roLoanPaymAccOpers =
                _roLoanPaymentAccOperDomain.GetAll()
                    .WhereIf(mrId != null, x => x.PayAccOperation.Account.RealityObject.Municipality.Id == mrId)
                    .WhereIf(moId != null, x => x.PayAccOperation.Account.RealityObject.Municipality.Id == moId)
                    .WhereIf(roIds.Any(), x => roIds.Contains(x.PayAccOperation.Account.RealityObject.Id))
                    .Where(x => x.PayAccOperation.Date >= startDate && x.PayAccOperation.Date <= endDate)
                    .Where(x => x.PayAccOperation.OperationType == PaymentOperationType.OutcomeAccountPayment
                        || x.PayAccOperation.OperationType == PaymentOperationType.ExpenseLoan
                        || x.PayAccOperation.OperationType == PaymentOperationType.OutcomeLoan
                        || x.PayAccOperation.OperationType == PaymentOperationType.OpeningAcc
                        || x.PayAccOperation.OperationType == PaymentOperationType.CashService
                        || x.PayAccOperation.OperationType == PaymentOperationType.CreditPayment
                        || x.PayAccOperation.OperationType == PaymentOperationType.CreditPercentPayment
                        || x.PayAccOperation.OperationType == PaymentOperationType.UndoPayment
                        || x.PayAccOperation.OperationType == PaymentOperationType.CancelPayment
#warning точно ли OtherSources - операция кредита?
                        || x.PayAccOperation.OperationType == PaymentOperationType.OtherSources)
                    .AsEnumerable()
                    .GroupBy(x => new { x.PayAccOperation.Account.RealityObject.Id, TypeSourceLoan = TypeSourceLoan.FundSubsidy })
                    .Select(x => new { x.Key, Sum = x.SafeSum(y => y.RealityObjectLoan.LoanSum) })
                    .GroupBy(x => x.Key.Id)
                    .ToDictionary(x => x.Key, x => new AccountOperationsInfo
                    {
                        Payments = x.Where(y => y.Key.TypeSourceLoan != TypeSourceLoan.FundSubsidy
                                && y.Key.TypeSourceLoan != TypeSourceLoan.TargetSubsidy
                                && y.Key.TypeSourceLoan != TypeSourceLoan.Penalty
                                && y.Key.TypeSourceLoan != TypeSourceLoan.RegionalSubsidy
                                && y.Key.TypeSourceLoan != TypeSourceLoan.StimulateSubsidy)
                            .SafeSum(y => y.Sum),
                        Penalty = x.Where(y => y.Key.TypeSourceLoan == TypeSourceLoan.Penalty)
                            .SafeSum(y => y.Sum),
                        Subsidy = x.Where(y => y.Key.TypeSourceLoan == TypeSourceLoan.FundSubsidy
                                || y.Key.TypeSourceLoan == TypeSourceLoan.RegionalSubsidy
                                || y.Key.TypeSourceLoan == TypeSourceLoan.StimulateSubsidy
                                || y.Key.TypeSourceLoan == TypeSourceLoan.TargetSubsidy)
                            .SafeSum(y => y.Sum),
                        Other = 0m
                    });

            // в разделе Кредит счета оплат дома, если провалиться внутрь записи, 
            // отбираю там только данные с необходимым типом
            var paymSrcFinanceDetailsQuery =
                _paymSrcFinanceDetailsDomain.GetAll()
                    .Where(
                        x =>
                        x.SrcFinanceType == ActPaymentSrcFinanceType.OwnerFundByMinTarrif
                        || x.SrcFinanceType == ActPaymentSrcFinanceType.OwnerFundByDecisionTariff
                        || x.SrcFinanceType == ActPaymentSrcFinanceType.Penalty
                        || x.SrcFinanceType == ActPaymentSrcFinanceType.FundSubsidy
                        || x.SrcFinanceType == ActPaymentSrcFinanceType.RegionSubsidy
                        || x.SrcFinanceType == ActPaymentSrcFinanceType.StimulSubsidy
                        || x.SrcFinanceType == ActPaymentSrcFinanceType.TargetSubsidy
                        || x.SrcFinanceType == ActPaymentSrcFinanceType.Other);

            // получаю только те типы операций, которые выводятся в Кредите счета оплат
            var perfWorkActPaymQuery =
                _roPaymAccOperPerfActDomain.GetAll()
                    .WhereIf(mrId != null, x => x.PayAccOperation.Account.RealityObject.Municipality.Id == mrId)
                    .WhereIf(moId != null, x => x.PayAccOperation.Account.RealityObject.Municipality.Id == moId)
                    .WhereIf(roIds.Any(), x => roIds.Contains(x.PayAccOperation.Account.RealityObject.Id))
                    .Where(x => x.PayAccOperation.Date >= startDate && x.PayAccOperation.Date <= endDate)
                    .Where(x => x.PayAccOperation.OperationType == PaymentOperationType.OutcomeAccountPayment
                        || x.PayAccOperation.OperationType == PaymentOperationType.ExpenseLoan
                        || x.PayAccOperation.OperationType == PaymentOperationType.OutcomeLoan
                        || x.PayAccOperation.OperationType == PaymentOperationType.OpeningAcc
                        || x.PayAccOperation.OperationType == PaymentOperationType.CashService
                        || x.PayAccOperation.OperationType == PaymentOperationType.CreditPayment
                        || x.PayAccOperation.OperationType == PaymentOperationType.CreditPercentPayment
                        || x.PayAccOperation.OperationType == PaymentOperationType.UndoPayment
                        || x.PayAccOperation.OperationType == PaymentOperationType.CancelPayment)
                    .AsEnumerable();

            // вычисляю данные по другим записям для столбцов Кредит
            var paymSrcFinanceDetailsDict = perfWorkActPaymQuery
                .Join(paymSrcFinanceDetailsQuery,
                    e => e.PerformedWorkActPayment.Id,
                    o => o.ActPayment.Id,
                    (e, o) => new
                    {
                        e.PayAccOperation.Account.RealityObject.Id,
                        o.SrcFinanceType,
                        o.Payment
                    })
                .AsEnumerable()
                .GroupBy(x => new { x.Id, x.SrcFinanceType })
                .Select(x => new { x.Key, Sum = x.SafeSum(y => y.Payment) })
                .GroupBy(x => x.Key.Id)
                .ToDictionary(x => x.Key, x => new AccountOperationsInfo
                {
                    Payments = x.Where(y => y.Key.SrcFinanceType == ActPaymentSrcFinanceType.OwnerFundByMinTarrif
                            || y.Key.SrcFinanceType == ActPaymentSrcFinanceType.OwnerFundByDecisionTariff)
                        .SafeSum(y => y.Sum),
                    Penalty = x.Where(y => y.Key.SrcFinanceType == ActPaymentSrcFinanceType.Penalty)
                        .SafeSum(y => y.Sum),
                    Subsidy = x.Where(y => y.Key.SrcFinanceType == ActPaymentSrcFinanceType.FundSubsidy
                            || y.Key.SrcFinanceType == ActPaymentSrcFinanceType.RegionSubsidy
                            || y.Key.SrcFinanceType == ActPaymentSrcFinanceType.StimulSubsidy
                            || y.Key.SrcFinanceType == ActPaymentSrcFinanceType.TargetSubsidy)
                        .SafeSum(y => y.Sum),
                    Other = x.Where(y => y.Key.SrcFinanceType == ActPaymentSrcFinanceType.Other)
                        .SafeSum(y => y.Sum)
                });

            // объединяю 2 Dictionary с данными для столбцов Кредит
            // если записи с таким ключом (номером счета) нет - добавляю, если есть - прибавляю сумму
            foreach (var item in paymSrcFinanceDetailsDict)
            {
                if (roLoanPaymAccOpers.ContainsKey(item.Key))
                {
                    roLoanPaymAccOpers[item.Key].Payments += item.Value.Payments;
                    roLoanPaymAccOpers[item.Key].Penalty += item.Value.Penalty;
                    roLoanPaymAccOpers[item.Key].Subsidy += item.Value.Subsidy;
                }
                else
                {
                    roLoanPaymAccOpers.Add(item.Key, item.Value);
                }
            }

            return roLoanPaymAccOpers;
        }

        public RealityObjectPaymentAccountOperation CreatePaymentOperation(
            RealityObject ro,
            decimal sum,
            PaymentOperationType opType,
            DateTime operationDate,
            RealityObjectPaymentAccount paymentAccount = null,
            RealityObjectSubsidyAccount subsidyAccount = null)
        {
            ArgumentChecker.NotNull(ro, "Объект недвижимости не найден", "ro");

            var account =
                paymentAccount ??
                _paymentAccDomain.GetAll()
                    .Where(x => x.RealityObject == ro)
                    .FirstOrDefault(x => !x.DateClose.HasValue || x.DateClose >= DateTime.Today);

            if (account == null)
            {
                throw new InvalidOperationException(
                    "Не найден открытый счет дома: {0}, на дату: {1}".FormatUsing(ro.Address, DateTime.Now));
            }

            var paymentOper = new RealityObjectPaymentAccountOperation
            {
                Account = account,
                OperationStatus = OperationStatus.Approved,
                OperationSum = sum,
                OperationType = opType,
                Date = operationDate
            };

            if (opType == PaymentOperationType.IncomeBudgetSubject
                || opType == PaymentOperationType.IncomeRegionalSubsidy
                || opType == PaymentOperationType.IncomeStimulateSubsidy
                || opType == PaymentOperationType.IncomeFundSubsidy)
            {
                subsidyAccount = subsidyAccount ??
                    _subsidyAccDomain.GetAll()
                        .Where(x => x.RealityObject == ro)
                        .FirstOrDefault(x => x.DateOpen <= DateTime.Today);

                if (subsidyAccount == null)
                {
                    throw new InvalidOperationException(
                        "Не найден счет cубсидий дома: {0}, на дату: {1}".FormatUsing(ro.Address, DateTime.Now));
                }

                var subsidyOper = new RealityObjectSubsidyAccountOperation
                {
                    Account = subsidyAccount,
                    OperationSum = sum,
                    OperationType = opType,
                    Date = operationDate
                };

                _subsidyAccOperDomain.Save(subsidyOper);
            }

            _paymentOperDomain.Save(paymentOper);

            return paymentOper;
        }

        public Dictionary<long, RealityObjectPaymentSummary> GetRobjectAccountSummary(IQueryable<RealityObject> query)
        {
            return _paymentOperDomain.GetAll()
                .Where(y => query.Any(x => x.Id == y.Account.RealityObject.Id))
                .Select(x => new
                {
                    RoId = x.Account.RealityObject.Id,
                    Credit =
                        x.OperationType == PaymentOperationType.OutcomeAccountPayment
                        || x.OperationType == PaymentOperationType.ExpenseLoan
                        || x.OperationType == PaymentOperationType.OutcomeLoan
                        || x.OperationType == PaymentOperationType.OpeningAcc
                        || x.OperationType == PaymentOperationType.CashService
                        || x.OperationType == PaymentOperationType.CreditPayment
                        || x.OperationType == PaymentOperationType.CreditPercentPayment
                            ? x.OperationSum
                            : 0,
                    Debt =
                        x.OperationType != PaymentOperationType.OutcomeAccountPayment
                        && x.OperationType != PaymentOperationType.ExpenseLoan
                        && x.OperationType != PaymentOperationType.OutcomeLoan
                        && x.OperationType != PaymentOperationType.CashService
                        && x.OperationType != PaymentOperationType.OpeningAcc
                        && x.OperationType != PaymentOperationType.CreditPayment
                        && x.OperationType != PaymentOperationType.CreditPercentPayment
                        && x.OperationType != PaymentOperationType.BankPercent
                            ? x.OperationSum
                            : 0,
                    PercentSum = x.OperationType == PaymentOperationType.BankPercent
                        ? x.OperationSum
                        : 0
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => new RealityObjectPaymentSummary
                {
                    Credit = y.SafeSum(x => x.Credit),
                    Debt = y.SafeSum(x => x.Debt),
                    PercentSum = y.SafeSum(x => x.PercentSum)
                });
        }

        public class AccountOperationsInfo
        {
            public decimal Payments { get; set; }
            public decimal Penalty { get; set; }
            public decimal Subsidy { get; set; }
            public decimal Other { get; set; }
        }

        private readonly IDomainService<RealityObjectPaymentAccountOperation> _paymentOperDomain;
        private readonly IDomainService<RealityObjectPaymentAccount> _paymentAccDomain;
        private readonly IDomainService<RealityObjectSubsidyAccount> _subsidyAccDomain;
        private readonly IDomainService<RealityObjectSubsidyAccountOperation> _subsidyAccOperDomain;
        private readonly IDomainService<RealObjLoanPaymentAccOper> _roLoanPaymentAccOperDomain;
        private readonly IDomainService<PaymentSrcFinanceDetails> _paymSrcFinanceDetailsDomain;
        private readonly IDomainService<RealObjPaymentAccOperPerfAct> _roPaymAccOperPerfActDomain;
    }
}