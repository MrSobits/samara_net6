namespace Bars.Gkh.RegOperator.Distribution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Overhaul.Utils;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;
    using ConfigSections.RegOperator;

    public partial class TransferCrDistribution
    {
        private List<PersAccProxy> ListByPaymentDocument(
            IEnumerable<AccountPaymentInfoSnapshot> accountInfos,
            decimal distribSum,
            Dictionary<long, string> dictStates)
        {
            var internalAccountInfos = accountInfos.ToArray();

            var calcAmount = this.Container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.PaymentDocumentRegistryConfig;

            var accountIds = internalAccountInfos.Select(x => x.AccountId).ToArray();

            var periodId = internalAccountInfos.Select(x => x.Snapshot.Period.Id).FirstOrDefault();

            var periodSummarydict = this.persAccSummaryDomain.GetAll()
                .Where(x => accountIds.Contains(x.PersonalAccount.Id))
                .Where(x => x.Period.Id == periodId)
                .ToDictionary(x => x.PersonalAccount.Id);

            var chargeSum = internalAccountInfos
                .SafeSum(
                    x =>
                    {
                        var docSum = this.GetSum(calcAmount, x, periodSummarydict.Get(x.AccountId));
                        return docSum.ZeroIfBelowZero();
                    })
                .RegopRoundDecimal(2);

            var penaltySum = internalAccountInfos
                .SafeSum(x =>
                {
                    var docSum = this.GetPenaltySum(calcAmount, x, periodSummarydict.Get(x.AccountId));
                    return docSum.ZeroIfBelowZero();
                })
                .RegopRoundDecimal(2);

            var chargeToDistribute = Math.Min(chargeSum, distribSum);

            distribSum -= chargeToDistribute;

            var penaltyToDistribute = Math.Min(penaltySum, distribSum);

            //а че делать то?!
            if (chargeToDistribute <= 0 && penaltyToDistribute <= 0)
            {
                return new List<PersAccProxy>();
            }

            var persaccProxies = internalAccountInfos
                .Select(x => new PersAccProxy
                {
                    Id = x.AccountId,
                    AccountOwner = x.Snapshot.Payer,
                    OwnerType = x.Snapshot.OwnerType,
                    PersonalAccountNum = x.AccountNumber,
                    RoomAddress = x.RoomAddress,
                    Debt = this.GetSum(calcAmount, x, periodSummarydict.Get(x.AccountId)) + this.GetPenaltySum(calcAmount, x, periodSummarydict.Get(x.AccountId)),
                    RoPayAccountNum = x.Snapshot.Return(z => z.PaymentReceiverAccount),
                    State = dictStates.Get(x.AccountId)
                })
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, z => z.First());

            Utils.MoneyAndCentDistribution(internalAccountInfos,
                x =>
                {
                    var charge = this.GetSum(calcAmount, x, periodSummarydict.Get(x.AccountId));

                    if (charge <= 0)
                    {
                        return 0m;
                    }

                    return chargeToDistribute * charge / chargeSum;
                },
                chargeToDistribute,
                (x, y) =>
                {
                    var proxy = persaccProxies.Get(x.AccountId);
                    proxy.Sum = y;
                    return proxy;
                },
                (proxy, coin) =>
                {
                    if (proxy.Sum > 0)
                    {
                        proxy.Sum += coin;
                        return true;
                    }
                    else
                    {
                        proxy.Sum = 0;
                        return false;
                    }
                });

            Utils.MoneyAndCentDistribution(internalAccountInfos,
                x =>
                {
                    var penalty = this.GetPenaltySum(calcAmount, x, periodSummarydict.Get(x.AccountId));

                    if (penalty <= 0)
                    {
                        return 0m;
                    }

                    return penaltyToDistribute * penalty / penaltySum;
                },
                penaltyToDistribute,
                (x, y) =>
                {
                    var proxy = persaccProxies.Get(x.AccountId);
                    proxy.SumPenalty = y;
                    return proxy;
                },
                (proxy, coin) =>
                {
                    if (proxy.SumPenalty > 0)
                    {
                        proxy.SumPenalty += coin;
                        return true;
                    }
                    else
                    {
                        proxy.SumPenalty = 0;
                        return false;
                    }
                });

            return persaccProxies.Values.ToList();
        }

        private decimal GetSum(PaymentDocumentRegistryConfig caclAmount, AccountPaymentInfoSnapshot accountInfo, PersonalAccountPeriodSummary periodSummary)
        {
            var amountType = accountInfo.Snapshot.OwnerType == PersonalAccountOwnerType.Individual
                ? caclAmount.CalcAmountIndividual
                : caclAmount.CalcAmountLegal;

            switch (amountType)
            {
                case CalcAmountType.ChargeAndRecalc:
                    return accountInfo.BaseTariffSum + accountInfo.DecisionTariffSum;

                case CalcAmountType.SaldoOut:
                    return periodSummary.BaseTariffDebt + periodSummary.DecisionTariffDebt + periodSummary.GetBaseTariffDebt() + periodSummary.GetPenaltyDebt();
            }

            throw new ArgumentException(@"Не заполнен тип расчета суммы распределения", nameof(caclAmount));
        }

        private decimal GetPenaltySum(PaymentDocumentRegistryConfig caclAmount, AccountPaymentInfoSnapshot accountInfo, PersonalAccountPeriodSummary periodSummary)
        {
            var amountType = accountInfo.Snapshot.OwnerType == PersonalAccountOwnerType.Individual
                ? caclAmount.CalcAmountIndividual
                : caclAmount.CalcAmountLegal;

            switch (amountType)
            {
                case CalcAmountType.ChargeAndRecalc:
                    return accountInfo.PenaltySum;

                case CalcAmountType.SaldoOut:

                    return periodSummary.PenaltyDebt + periodSummary.GetPenaltyDebt();
            }

            throw new ArgumentException(@"Не заполнен тип расчета суммы распределения", nameof(caclAmount));
        }
    }
}