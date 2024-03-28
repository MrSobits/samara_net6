namespace Bars.Gkh.RegOperator.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Domain;
    using Entities.ValueObjects;
    using Gkh.Domain.CollectionExtensions;
    using Entities;
    using DataContracts;
    using Bars.Gkh.Utils;

    public partial class Service
    {
        /// <summary>
        /// Предоставление сведений о начислениях и оплатах
        /// </summary>
        /// <param name="inn">ИНН</param>
        /// <param name="count">Количество</param>
        /// <param name="page">Страница</param>
        /// <param name="month">Месяц</param>
        /// <param name="year">Год</param>
        /// <returns>Ответ сервера по начислениям и оплатам</returns>
        public GetChargePaymentResponse GetChargePaymentRkc(string inn, string count, string page, string month, string year)
        {
            var response = new GetChargePaymentResponse
            {
                Record = new GetChargePaymentRecord
                {
                    FormatVersion = "1.1",
                    Page = page,
                    Count = count
                }
            };

            var monthDict = new Dictionary<string, int>
            {
                {"январь", 1},
                {"февраль", 2},
                {"март", 3},
                {"апрель", 4},
                {"май", 5},
                {"июнь", 6},
                {"июль", 7},
                {"август", 8},
                {"сентябрь", 9},
                {"октябрь", 10},
                {"ноябрь", 11},
                {"декабрь", 12}
            };

            var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            if (ci != null)
            {
                ci.NumberFormat.NumberDecimalSeparator = ".";
                _numberStyle = ci.NumberFormat;
            }

            var persAccDomain = Container.ResolveDomain<BasePersonalAccount>();
            var cashPayCenterDomain = Container.ResolveDomain<CashPaymentCenter>();
            var chargePeriodDomain = Container.ResolveDomain<ChargePeriod>();
            var cashPayCenterPersAccDomain = Container.ResolveDomain<CashPaymentCenterPersAcc>();
            var cashPaymentCenterRealObjDomain = Container.ResolveDomain<CashPaymentCenterRealObj>();
            var summaryDomain = Container.ResolveDomain<PersonalAccountPeriodSummary>();

            try
            {
                var cashPayCenter = cashPayCenterDomain.GetAll().FirstOrDefault(x => x.Contragent.Inn == inn);
                var date = new DateTime(year.ToInt(), monthDict.Get(month), 15);

                var period = chargePeriodDomain.GetAll()
                    .FirstOrDefault(x => x.StartDate <= date && (!x.EndDate.HasValue || x.EndDate >= date));

                if (cashPayCenter == null)
                {
                    response.Result = GetChargePaymentResult.NotCashPaymentCenter;
                    return response;
                }

                if (period == null)
                {
                    response.Result = GetChargePaymentResult.DataNotFound;
                    return response;
                }

                var cachPaymentCenterConnectionType = this.Container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.CachPaymentCenterConnectionType;
                IQueryable<BasePersonalAccount> persAccQuery;

                var loadParam = new LoadParam
                {
                    Start = (page.ToInt() - 1) * count.ToInt(),
                    Limit = count.ToInt()
                };

                if (cachPaymentCenterConnectionType == CachPaymentCenterConnectionType.ByAccount)
                {
                    var cashPayPersAccQuery = cashPayCenterPersAccDomain.GetAll()
                        .Where(x => x.CashPaymentCenter.Id == cashPayCenter.Id)
                        .Where(x =>
                            (x.DateStart <= period.StartDate && (!x.DateEnd.HasValue || period.StartDate <= x.DateEnd)) ||
                            (period.EndDate.HasValue && x.DateStart <= period.EndDate &&
                             (!x.DateEnd.HasValue || period.EndDate <= x.DateEnd)));

                    persAccQuery = persAccDomain.GetAll()
                        .Where(x => cashPayPersAccQuery.Any(y => y.PersonalAccount.Id == x.Id))
                        .Paging(loadParam);
                }
                else
                {
                    var cashPayPersAccQuery = cashPaymentCenterRealObjDomain.GetAll()
                           .Where(x => x.CashPaymentCenter.Id == cashPayCenter.Id)
                           .Where(x =>
                               (x.DateStart <= period.StartDate && (!x.DateEnd.HasValue || period.StartDate <= x.DateEnd)) ||
                               (period.EndDate.HasValue && x.DateStart <= period.EndDate &&
                                (!x.DateEnd.HasValue || period.EndDate <= x.DateEnd)));

                    persAccQuery = persAccDomain.GetAll()
                        .Where(x => cashPayPersAccQuery.Any(y => y.RealityObject.Id == x.Room.RealityObject.Id))
                        .Paging(loadParam);

                }

                var persAccounts = persAccQuery
                    .Select(x => new
                    {
                        x.Id,
                        x.PersAccNumExternalSystems,
                        Owner = cashPayCenter.ShowPersonalData? x.AccountOwner.Name: ""
                    })
                    .ToList();

                if (!persAccounts.Any())
                {
                    response.Result = GetChargePaymentResult.DataNotFound;
                    return response;
                }

                var summary = summaryDomain.GetAll()
                    .Where(x => persAccQuery.Any(y => y.Id == x.PersonalAccount.Id) && x.Period.Id == period.Id)
                    .Select(x => new
                    {
                        x.PersonalAccount.Id,
                        x.SaldoIn,
                        x.SaldoOut,
                        SaldoChange = x.SaldoOut - x.SaldoIn,
                        x.TariffPayment,
                        x.TariffDecisionPayment,
                        x.PenaltyPayment,
                        x.ChargeTariff,
                        x.ChargedByBaseTariff,
                        x.Penalty,
                        x.RecalcByBaseTariff,
                        x.RecalcByDecisionTariff
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.First());

                var paymentDates = GetPaymentDates(persAccQuery, period);

                response.Record.RegChargeSum = FormatDecimal(summary.Values.SafeSum(x => x.ChargeTariff + x.Penalty));
                response.Record.RegPaidSum = FormatDecimal(summary.Values.SafeSum(x => x.TariffPayment + x.TariffDecisionPayment + x.PenaltyPayment));

                var resultPayments = persAccounts
                    .Select(x =>
                    {
                        var rec = new SyncChargePayment
                        {
                            AccNumber = x.PersAccNumExternalSystems,
                            AccountOwnerName = x.Owner,
                            Month = month,
                            Year = year
                        };

                        if (summary.ContainsKey(x.Id))
                        {
                            var tempSummary = summary[x.Id];
                            rec.BalanceIn = FormatDecimal(tempSummary.SaldoIn);
                            rec.BalanceOut = FormatDecimal(tempSummary.SaldoOut);
                            rec.BalanceChange = FormatDecimal(tempSummary.SaldoChange);
                            rec.PaidPenalty = FormatDecimal(tempSummary.PenaltyPayment);
                            rec.PaidSum = FormatDecimal(tempSummary.TariffPayment + tempSummary.TariffDecisionPayment);
                            rec.ChargedSum = FormatDecimal(tempSummary.ChargeTariff);
                            rec.ChargedPenalty = FormatDecimal(tempSummary.Penalty);
                            rec.ForPay = FormatDecimal(tempSummary.ChargeTariff + tempSummary.Penalty);
                            rec.RecalcSum = FormatDecimal(tempSummary.RecalcByBaseTariff + tempSummary.RecalcByDecisionTariff);
                        }

                        if (paymentDates.ContainsKey(x.Id))
                        {
                            rec.PaidDate = paymentDates.Get(x.Id).ToShortDateString();
                        }

                        return rec;
                    })
                    .ToArray();

                response.Record.Payments = resultPayments;
                response.Result = GetChargePaymentResult.NoErrors;
                return response;
            }
            finally
            {
                Container.Release(persAccDomain);
                Container.Release(cashPayCenterDomain);
                Container.Release(chargePeriodDomain);
                Container.Release(cashPayCenterPersAccDomain);
                Container.Release(cashPaymentCenterRealObjDomain);
                Container.Release(summaryDomain);
            }
        }

        private Dictionary<long, DateTime> GetPaymentDates(IQueryable<BasePersonalAccount> query, ChargePeriod period)
        {
            var transferDomain = Container.ResolveDomain<Transfer>();

            try
            {
                var accountWallets = query
                    .Select(x => new
                    {
                        x.Id,
                        Btw = x.BaseTariffWallet.WalletGuid,
                        Dtw = x.DecisionTariffWallet.WalletGuid,
                        Pw = x.PenaltyWallet.WalletGuid
                    })
                    .ToArray();

                var btWallets = accountWallets.ToDictionary(x => x.Btw, z => z.Id);
                var dtWallets = accountWallets.ToDictionary(x => x.Dtw, z => z.Id);
                var pWallets = accountWallets.ToDictionary(x => x.Pw, z => z.Id);

                var walletGuids = accountWallets.SelectMany(x => new[] {x.Btw, x.Dtw, x.Pw}).ToArray();

                var startDate = period.StartDate;
                var endDate = period.GetEndDate();

                Func<string, long> getAccountId = x =>
                {
                    if (btWallets.ContainsKey(x))
                        return btWallets[x];

                    if (dtWallets.ContainsKey(x))
                        return dtWallets[x];

                    if (pWallets.ContainsKey(x))
                        return pWallets[x];

                    return 0L;
                };

                var paymentDates = transferDomain.GetAll()
                    .Where(x => walletGuids.Contains(x.TargetGuid))
                    .Where(x => startDate <= x.PaymentDate.Date && x.PaymentDate.Date <= endDate)
                    .Select(x => new
                    {
                        x.TargetGuid,
                        x.PaymentDate
                    })
                    .AsEnumerable()
                    .GroupBy(x => getAccountId(x.TargetGuid))
                    .ToDictionary(x => x.Key, z => z.Max(x => x.PaymentDate));

                return paymentDates;
            }
            finally
            {
                Container.Release(transferDomain);
            }
        }

        private NumberFormatInfo _numberStyle;

        private string FormatDecimal(decimal d)
        {
            return d.RegopRoundDecimal(2).ToString(_numberStyle);
        }
    }
}