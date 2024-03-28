namespace Bars.Gkh.RegOperator.Distribution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Платеж КР
    /// </summary>
    public partial class TransferCrDistribution
    {
        private List<PersAccProxy> ListByDebt(
            List<DebtProxy> debts,
            decimal distribSum,
            DistributeOn distributeOn,
            Dictionary<long, List<PersonalAccountPeriodSummary>> summaries)
        {
            List<PersAccProxy> proxies;

            var copyDistribSum = distribSum;

            var tmpProxies = debts.ToDictionary(x => x.Id, x => new PersAccProxy(x.PersonalAccount));

            switch (distributeOn)
            {
                case DistributeOn.Charges:
                    this.DistributeTariff(ref copyDistribSum,
                        debts,
                        (x, y) =>
                        {
                            var proxy = tmpProxies[x.Id];
                            proxy.Sum += y;
                            return proxy;
                        },
                        false);

                    this.DistributeByArea(ref copyDistribSum,
                        debts,
                        (x, y) =>
                        {
                            var proxy = tmpProxies[x.Id];
                            proxy.Sum += y;
                            return proxy;
                        });

                    proxies = tmpProxies.Values.ToList();

                    break;

                case DistributeOn.Penalties:

                    this.DistributePenalty(ref copyDistribSum,
                        debts,
                        (x, y) =>
                        {
                            var proxy = tmpProxies[x.Id];
                            proxy.SumPenalty += y;
                            return proxy;
                        },
                        false);

                    this.DistributeByArea(ref copyDistribSum,
                        debts,
                        (x, y) =>
                        {
                            var proxy = tmpProxies[x.Id];
                            proxy.SumPenalty += y;
                            return proxy;
                        });

                    proxies = tmpProxies.Values.ToList();
                    break;

                default:
                    this.DistributeTariff(ref copyDistribSum,
                        debts,
                        (x, y) =>
                        {
                            var proxy = tmpProxies[x.Id];
                            proxy.Sum += y;
                            return proxy;
                        },
                        false);

                    this.DistributePenalty(ref copyDistribSum,
                        debts,
                        (x, y) =>
                        {
                            var proxy = tmpProxies[x.Id];
                            proxy.SumPenalty += y;
                            return proxy;
                        },
                        false);

                    this.DistributeByArea(ref copyDistribSum,
                        debts,
                        (x, y) =>
                        {
                            var proxy = tmpProxies[x.Id];
                            proxy.Sum += y;
                            return proxy;
                        });

                    proxies = tmpProxies.Values.ToList();

                    break;
            }

            switch (distributeOn)
            {
                case DistributeOn.Charges:
                    proxies.ForEach(x =>
                    {
                        var summary = summaries.Get(x.Id);
                        var debt = summary.SafeSum(y =>
                            y.ChargeTariff + y.RecalcByBaseTariff + y.RecalcByDecisionTariff
                            + y.BaseTariffChange + y.DecisionTariffChange
                            - y.TariffPayment - y.TariffDecisionPayment).RegopRoundDecimal(2);
                        x.Debt = debt >= 0M ? debt : 0M;
                    });
                    break;

                case DistributeOn.Penalties:
                    proxies.ForEach(x =>
                    {
                        var summary = summaries.Get(x.Id);
                        var debt = summary.SafeSum(y => y.GetPenaltyDebt()).RegopRoundDecimal(2);
                        x.Debt = debt >= 0M ? debt : 0M;
                    });
                    break;

                default:
                    proxies.ForEach(x =>
                    {
                        var summary = summaries.Get(x.Id);
                        var debt = summary.SafeSum(y => y.GetTotalCharge() - y.GetTotalPayment()).RegopRoundDecimal(2);
                        x.Debt = debt >= 0M ? debt : 0M;
                    });
                    break;
            }

            return proxies;
        }

        private void DistributeTariff(
            ref decimal distribSum,
            List<DebtProxy> debts,
            Func<DebtProxy, decimal, PersAccProxy> proxyCreator,
            bool distributeCent = true)
        {
            if (distribSum <= 0)
            {
                return;
            }

            var sumDebts = debts.Where(y => y.TariffDebt > 0).SafeSum(z => z.TariffDebt).RegopRoundDecimal(2);

            var debtToDistribute = Math.Min(sumDebts, distribSum);

            // если сумма распределения меньше суммы долга, то распределяем копейки
            var distribCent = sumDebts > debtToDistribute || distributeCent;

            distribSum -= debtToDistribute;

            Utils.MoneyAndCentDistribution(
                debts,
                x =>
                {
                    var debt = x.TariffDebt;

                    if (debt <= 0m)
                    {
                        return 0m;
                    }

                    return debtToDistribute * debt / sumDebts;
                },
                debtToDistribute,
                proxyCreator,
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
                },
                distributeCent: distribCent);
        }

        private void DistributePenalty(
            ref decimal distribSum,
            List<DebtProxy> debts,
            Func<DebtProxy, decimal, PersAccProxy> proxyCreator,
            bool distributeCent = true)
        {
            if (distribSum <= 0)
            {
                return;
            }

            var sumDebts = debts.Where(y => y.PenaltyDebt > 0).SafeSum(z => z.PenaltyDebt).RegopRoundDecimal(2);

            var debtToDistribute = Math.Min(sumDebts, distribSum);

            // если сумма распределения меньше суммы долга, то распределяем копейки
            var distribCent = sumDebts > debtToDistribute || distributeCent;

            distribSum -= debtToDistribute;

            Utils.MoneyAndCentDistribution(
                debts,
                x =>
                {
                    var debt = x.PenaltyDebt;

                    if (debt <= 0m)
                    {
                        return 0m;
                    }

                    return debtToDistribute * debt / sumDebts;
                },
                debtToDistribute,
                proxyCreator,
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
                },
                distributeCent: distribCent);
        }

        private void DistributeByArea(
            ref decimal distribSum,
            List<DebtProxy> debts,
            Func<DebtProxy, decimal, PersAccProxy> proxyCreator,
            bool distributeCent = true)
        {
            if (distribSum <= 0)
            {
                return;
            }

            var copy = distribSum;

            var sumArea = debts.SafeSum(x => x.PersonalAccount.Room.Area * x.PersonalAccount.AreaShare).RegopRoundDecimal(3);

            if (sumArea <= 0m)
            {
                throw new ArgumentException("Сумма площадей у выбранных лицевых счетов равна нулю");
            }

            Utils.MoneyAndCentDistribution(
                debts,
                x =>
                {
                    var area = x.PersonalAccount.Room.Area * x.PersonalAccount.AreaShare;

                    if (area <= 0m)
                    {
                        return 0m;
                    }

                    return copy * area / sumArea;
                },
                copy,
                proxyCreator,
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
                },
                distributeCent: distributeCent);
        }
    }
}