namespace Bars.Gkh.RegOperator.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using FastMember;
    using Gkh.Domain.CollectionExtensions;
    using NHibernate.Linq;
    using Overhaul.DomainService;
    using Entities;
    using System.Linq.Dynamic.Core;
    using Castle.Windsor;

    public class RegopOverhaulToGasuExportService : IOverhaulToGasuExportService
    {
        public IWindsorContainer Container { get; set; }

        public List<OverhaulToGasuProxy> GetData(DateTime startDate, long programId)
        {
            var result = new List<OverhaulToGasuProxy>();
            var date = startDate.ToShortDateString();

            var chargeAccOperDomain = Container.ResolveDomain<RealityObjectChargeAccountOperation>();
            var transferDomain = Container.Resolve<ITransferDomainService>();
            var municipalityDomain = Container.ResolveDomain<Municipality>();
            var roPayAccountDomain = Container.ResolveDomain<RealityObjectPaymentAccount>();

            try
            {
                var municipalities = municipalityDomain.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.Okato
                    })
                    .ToList();

                var collection = chargeAccOperDomain.GetAll()
                    .Where(x => x.Period.StartDate >= startDate)
                    .Select(x => new
                    {
                        MuId = x.Account.RealityObject.Municipality.Id,
                        x.Account.RealityObject.Municipality.Okato,
                        x.Period.StartDate,
                        x.ChargedTotal,
                        x.SaldoOut
                    })
                    .ToList()
                    .GroupBy(x => x.MuId)
                    .ToDictionary(x => x.Key, x =>
                    {
                        var charge = x.SafeSum(y => y.ChargedTotal);
                        var saldoOut = x.OrderByDescending(y => y.StartDate).Select(y => y.SaldoOut).FirstOrDefault();

                        return charge == 0 
                            ? 0M.RegopRoundDecimal(2).ToFormatedString('.')
                            : ((charge - saldoOut) / charge).RegopRoundDecimal(2).ToFormatedString('.');
                    });

                var transfers = transferDomain.GetAll<RealityObjectTransfer>()
                    .Fetch(x => x.Operation)
                    .ThenFetch(x => x.CanceledOperation)
                    .Where(
                        x =>
                            x.Operation.Reason == "Оплата акта выполненных работ" &&
                            x.Operation.CanceledOperation == null)
                    .Select(x => new
                    {
                        x.SourceGuid,
                        x.Amount
                    })
                    .ToList()
                    .GroupBy(x => x.SourceGuid)
                    .ToDictionary(x => x.Key, y => y.SafeSum(x => x.Amount));

                var baseWallProp = new List<string> {"BaseTariffPaymentWallet", "DecisionPaymentWallet"};
                var subsidyWallProp = new List<string>
                {
                    "TargetSubsidyWallet",
                    "FundSubsidyWallet",
                    "RegionalSubsidyWallet",
                    "StimulateSubsidyWallet"
                };

                foreach (var municipality in municipalities)
                {
                    var query =
                        roPayAccountDomain.GetAll().Where(x => x.RealityObject.Municipality.Id == municipality.Id);

                    var baseGuids = GetWalletGuids(query, baseWallProp);
                    var subsidyGuids = GetWalletGuids(query, subsidyWallProp);

                    var baseSum = baseGuids.SafeSum(x => transfers.Get(x));
                    var subsidySum = subsidyGuids.SafeSum(x => transfers.Get(x));

                    var value = collection.Get(municipality.Id);
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        value = 0M.RegopRoundDecimal(2).ToFormatedString('.');
                    }

                    result.Add(new OverhaulToGasuProxy
                    {
                        DateStart = date,
                        FactOrPlan = (int)FactOrPlan.Fact,
                        Okato = municipality.Okato,
                        Value = value,
                        IndexId = "390010611",
                        UnitId = "744"
                    });

                    result.Add(new OverhaulToGasuProxy
                    {
                        DateStart = date,
                        FactOrPlan = (int) FactOrPlan.Fact,
                        Okato = municipality.Okato,
                        Value = baseSum.RegopRoundDecimal(2).ToFormatedString('.'),
                        IndexId = "390010649",
                        UnitId = "383"
                    });

                    result.Add(new OverhaulToGasuProxy
                    {
                        DateStart = date,
                        FactOrPlan = (int) FactOrPlan.Fact,
                        Okato = municipality.Okato,
                        Value = subsidySum.RegopRoundDecimal(2).ToFormatedString('.'),
                        IndexId = "390010650",
                        UnitId = "383"
                    });
                }
            }
            finally
            {
                Container.Release(chargeAccOperDomain);
                Container.Release(transferDomain);
                Container.Release(municipalityDomain);
                Container.Release(roPayAccountDomain);
            }

            return result;
        }

        private List<string> GetWalletGuids(IQueryable<RealityObjectPaymentAccount> paymentAccounts, IEnumerable<string> walletProps)
        {
            var query = new StringBuilder("new(");

            var counter = 0;
            foreach (var walletProp in walletProps)
            {
                counter++;

                query.Append(walletProp).Append(".WalletGuid as ").Append("w_" + walletProp);
                if (counter != walletProps.Count())
                {
                    query.Append(", ");
                }
            }

            query.Append(")");

            var guids = new List<string>();

            var queryString = query.ToString().TrimEnd(',');
            var result = paymentAccounts.Select(queryString);

            foreach (var item in result)
            {
                var accessor = ObjectAccessor.Create(item);

                foreach (var walletProp in walletProps)
                {
                    guids.Add(accessor["w_" + walletProp].ToStr());
                }
            }

            return guids;
        }
    }
}