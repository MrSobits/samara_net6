namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator.Debtor;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис расчета долга
    /// </summary>
    public partial class DebtorCalcService
    {
        private PaymentPenalties GetCurrentPenaltyParameter(long roId, DateTime date)
        {
            IEnumerable<Tuple<DateTime, CrFundFormationDecisionType>> foundDecision;

            if (!this.roFundDecision.TryGetValue(roId, out foundDecision))
            {
                return null;
            }

            var roFund = foundDecision.FirstOrDefault(x => x.Item1 <= date);

            if (roFund == null)
            {
                return null;
            }

            return this.penaltyParams
                .Where(x => x.DateStart <= date)
                .Where(x => !x.DateEnd.HasValue || x.DateEnd >= date)
                .FirstOrDefault(x => x.DecisionType == roFund.Item2);
        }

        private Dictionary<long, decimal> GetPaymentSums(BasePersonalAccountDto[] accountData)
        {
            return accountData.GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.Balance));
        }

        private Dictionary<long, Dictionary<DateTime, decimal>> CalculateCharges()
        {
            return this.summaries
                .Where(x => x.Charge != 0 || x.Change != 0) //берем периоды с непустыми начислениями
                .Select(x => new
                {
                    x.AccountId,
                    Amount = x.Charge + x.Change,
                    EndDate = x.Period.EndDate ?? DateTime.Today
                })
                .Union(this.recalcHistory
                    .Select(x => new
                    {
                        x.AccountId,
                        Amount = x.RecalcSum,
                        EndDate = x.RecalcPeriod.EndDate ?? DateTime.Today
                    }))
                .GroupBy(x => x.AccountId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.EndDate)
                        .ToDictionary(y => y.Key, y => y.SafeSum(z => z.Amount)));
        }

        private Dictionary<long, SumData> CalculateAccountDebts()
        {
            return this.summaries
                .Where(x => x.Period.Id == this.period.Id)
                .ToDictionary(x => x.AccountId,
                    x =>
                        new SumData(
                            x.Debt - x.Payment + x.Recalc + x.Charge + x.Change,
                            x.DebtBaseTariff - x.TariffPayment + x.RecalcByBaseTariff + x.ChargedByBaseTariff + x.BaseTariffChange,
                            x.DebtDecisionTariff - x.TariffDecisionPayment + x.RecalcByDecisionTariff + x.Charge - x.ChargedByBaseTariff + x.DecisionTariffChange,
                            x.PenaltyDebt - x.PenaltyPayment + x.RecalcByPenalty + x.Penalty + x.PenaltyChange)
                );
        }

        private Dictionary<long, DateTime> GetLastPaymentDates(BasePersonalAccountDto[] accountData)
        {
            var walletGuids = accountData.SelectMany(
                    x => new[]
                    {
                        x.BaseWalletGuid,
                        x.DecisionWalletGuid
                    })
                .ToArray();

            return this.TransferDomain.GetAll()
                .Where(x => x.IsAffect && !x.Operation.IsCancelled)
                .WhereContainsBulked(x => x.Owner.Id, this.accountIds, this.BulkSize)
                .WhereContainsBulked(x => x.TargetGuid, walletGuids, this.BulkSize)
                .Select(
                    x => new
                    {
                        AccountId = x.Owner.Id,
                        x.OperationDate
                    })
                .AsEnumerable()
                .GroupBy(x => x.AccountId, x => x.OperationDate)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y).First());
        }

        private class DebtorTariffSumChecker
        {
            private decimal? baseTariffDebtSum;
            private decimal? decisionTariffDebtSum;
            private decimal? tariffDebtSum;
            private readonly DebtorSumCheckerType debtorSumCheckerType;
            private readonly DebtorLogicalOperands debtorLogicalOperands;

            public DebtorTariffSumChecker(DebtorConfig config)
            {
                this.debtorSumCheckerType = config.DebtorRegistryConfig.DebtorSumCheckerType;
                this.debtorLogicalOperands = config.DebtorRegistryConfig.DebtOperand;
                this.baseTariffDebtSum = config.DebtorRegistryConfig.DebtSumBaseTariff.ToDecimal();
                this.decisionTariffDebtSum = config.DebtorRegistryConfig.DebtSumDecisionTariff.ToDecimal();
                this.tariffDebtSum = config.DebtorRegistryConfig.DebtSum.ToDecimal();

                if (this.debtorSumCheckerType == DebtorSumCheckerType.BaseAdnDecisionTariff)
                {
                    this.ParamMessage = $"Сумма задолженности по базовому тарифу: {this.baseTariffDebtSum}, "
                        + $"по тарифу решения: {this.decisionTariffDebtSum}. ";
                }
                else
                {
                    this.ParamMessage = $"Сумма задолженности: {this.tariffDebtSum}. ";
                }
            }

            public bool Check(Debtor debtor, out string message)
            {
                if (this.debtorSumCheckerType == DebtorSumCheckerType.BaseAdnDecisionTariff)
                {
                    return this.BaseAndDecisionTariffCheck(debtor, out message);
                }

                return this.DefaultCheck(debtor, out message);
            }

            public string ParamMessage { get; }

            private bool DefaultCheck(Debtor debtor, out string message)
            {
                if (!this.tariffDebtSum.HasValue)
                {
                    message = "Сумма задолженности: Не указано";
                    return true;
                }

                if (debtor.DebtSum >= this.tariffDebtSum)
                {
                    message = $"Сумма задолженности: {debtor.DebtSum}";
                    return true;
                }
                else
                {
                    message = $"Сумма задолженности: {debtor.DebtSum}, требуется: {this.tariffDebtSum.Value.RegopRoundDecimal(2)}";
                    return false;
                }
            }

            private bool BaseAndDecisionTariffCheck(Debtor debtor, out string message)
            {
                if (!this.baseTariffDebtSum.HasValue)
                {
                    message = "Сумма задолженности по базовому тарифу: Не указано";
                    return true;
                }

                if (!this.decisionTariffDebtSum.HasValue)
                {
                    message = "Сумма задолженности по тарифу решения: Не указано";
                    return true;
                }
                bool checkResult;
                switch (this.debtorLogicalOperands)
                {
                    case DebtorLogicalOperands.And:
                        checkResult = debtor.DebtBaseTariffSum > this.baseTariffDebtSum && debtor.DebtDecisionTariffSum > this.decisionTariffDebtSum;
                        break;
                    case DebtorLogicalOperands.Or:
                        checkResult = debtor.DebtBaseTariffSum > this.baseTariffDebtSum || debtor.DebtDecisionTariffSum > this.decisionTariffDebtSum;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (checkResult)
                {
                    message = $"Сумма задолженности по базовому тарифу: {debtor.DebtBaseTariffSum}, "
                        + $"по тарифу решения: {debtor.DebtDecisionTariffSum}";
                    return true;
                }
                else
                {
                    message = $"Сумма задолженности по базовому тарифу: {debtor.DebtBaseTariffSum}, "
                        + $"по тарифу решения: {debtor.DebtBaseTariffSum}. "
                        + $"Требуется по базовому тарифу: {this.baseTariffDebtSum.Value.RegopRoundDecimal(2)}, по тарифу решения: {this.decisionTariffDebtSum.Value.RegopRoundDecimal(2)}";
                    return false;
                }
            }
        }
    }
}