namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.EntityExtensions;

    /// <summary>
    /// Информация о размере фонда КР по дому (crfundsize.csv)
    /// </summary>
    public class CrFundSizeSelectorService : BaseProxySelectorService<CrFundSizeProxy>
    {
        /// <inheritdoc />
        protected override ICollection<CrFundSizeProxy> GetAdditionalCache()
        {
            var repository = this.Container.ResolveRepository<RealityObjectPaymentAccount>();

            using (this.Container.Using(repository))
            {
                return this.GetProxies(repository.GetAll().WhereContainsBulked(x => x.RealityObject.Id, this.AdditionalIds));
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, CrFundSizeProxy> GetCache()
        {
            var repository = this.Container.ResolveRepository<RealityObjectPaymentAccount>();

            using (this.Container.Using(repository))
            {
                var roQuery = this.FilterService.GetFiltredQuery<RealityObject>();
                var query = repository.GetAll()
                    .Where(x => roQuery.Any(y => y.Id == x.RealityObject.Id));

                return this.GetProxies(query).ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<CrFundSizeProxy> GetProxies(IQueryable<RealityObjectPaymentAccount> paymentAccountQuery)
        {
            var periodRepository = this.Container.Resolve<IChargePeriodRepository>();
            var chargeRepository = this.Container.ResolveRepository<RealityObjectChargeAccount>();
            var transferRepository = this.Container.ResolveRepository<RealityObjectTransfer>();
            using (this.Container.Using(periodRepository, chargeRepository, transferRepository))
            {
                var period = periodRepository.GetCurrentPeriod();
                var firstDayPeriod = new DateTime(period.StartDate.Year, period.StartDate.Month, 1);
                var roQuery = this.FilterService.GetFiltredQuery<RealityObject>();

                var chargeAccountDict = chargeRepository.GetAll()
                    .Where(x => roQuery.Any(y => y.Id == x.RealityObject.Id))
                    .Select(x => new
                    {
                        x.RealityObject.Id,
                        x.Operations,
                        x.PaidTotal
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key,
                        x => (decimal?) x.SelectMany(y => y.Operations).Select(y => y.ChargedTotal).SafeSum() - (x.FirstOrDefault()?.PaidTotal ?? 0));

                var paymentQuery = paymentAccountQuery
                    .Select(x => new RoPaymentAccountDto
                    {
                        Id = x.Id,
                        RealityObjectId = x.RealityObject.Id,
                        BaseTariffPaymentWalletGuid = x.BaseTariffPaymentWallet.WalletGuid,
                        DecisionPaymentWalletGuid = x.DecisionPaymentWallet.WalletGuid,
                        PenaltyPaymentWalletGuid = x.PenaltyPaymentWallet.WalletGuid,
                        MoneyLocked = x.MoneyLocked
                    })
                    .OrderBy(x => x.Id);

                var count = paymentAccountQuery.Count();
                var take = 1000;
                var result = new List<CrFundSizeProxy>(count);
                for (var skip = 0; skip < count; skip += take)
                {
                    var payments = paymentQuery.Skip(skip).Take(take).ToList();

                    var guids = payments.Aggregate(new HashSet<string>(),
                        (g, a) =>
                        {
                            g.Add(a.BaseTariffPaymentWalletGuid);
                            g.Add(a.DecisionPaymentWalletGuid);
                            g.Add(a.PenaltyPaymentWalletGuid);
                            return g;
                        });

                    var inTransferQuery = transferRepository.GetAll()
                        .WhereContainsBulked(x => x.TargetGuid, guids, guids.Count);

                    var outTransferQuery = transferRepository.GetAll()
                        .WhereContainsBulked(x => x.SourceGuid, guids, guids.Count);

                    var debtCache = this.GetDebtSum(inTransferQuery, outTransferQuery);
                    var creditCache = this.GetCreditSum(inTransferQuery, outTransferQuery);

                    var paymentAccountPart = payments
                        .Select(x =>
                        {
                            var debt = debtCache.Get(x.Id);
                            var credit = creditCache.Get(x.Id);

                            return new CrFundSizeProxy
                            {
                                Id = x.RealityObjectId,
                                HouseId = x.RealityObjectId,
                                Period = firstDayPeriod,
                                State = 1,
                                FundOnStartPeriod = debt - x.MoneyLocked - credit,
                                AmountFund = credit,
                                AmountDept = chargeAccountDict.Get(x.RealityObjectId)
                            };
                        });

                    result.AddRange(paymentAccountPart);
                }

                return result;
            }
        }

        private Dictionary<long, decimal> GetDebtSum(IQueryable<RealityObjectTransfer> inTransferQuery, IQueryable<RealityObjectTransfer> outTransferQuery)
        {
            return this.GetInSum(inTransferQuery, true)
                .Union(this.GetOutSum(outTransferQuery, true))
                .GroupBy(x => x.Id, x => x.Amount)
                .ToDictionary(x => x.Key, x => x.Sum());
        }

        private Dictionary<long, decimal> GetCreditSum(IQueryable<RealityObjectTransfer> inTransferQuery, IQueryable<RealityObjectTransfer> outTransferQuery)
        {
            return this.GetInSum(inTransferQuery)
                .Union(this.GetOutSum(outTransferQuery))
                .GroupBy(x => x.Id, x => x.Amount)
                .ToDictionary(x => x.Key, x => x.Sum());
        }

        private ICollection<AmountDto> GetInSum(IQueryable<RealityObjectTransfer> transfers, bool checkLoan = false)
        {
            return transfers.WhereIf(!checkLoan, x => x.Operation.CanceledOperation.GetNullableId() == null)
                .WhereIf(checkLoan, x => x.Operation.CanceledOperation.GetNullableId() == null || x.IsLoan)
                .Select(x => new AmountDto
                {
                    Id = x.Owner.Id,
                    Amount = x.Amount
                })
                .ToList();
        }

        private ICollection<AmountDto> GetOutSum(IQueryable<RealityObjectTransfer> transfers, bool checkLoan = false)
        {
            return transfers.WhereIf(!checkLoan, x => x.Operation.CanceledOperation.GetNullableId() != null)
                .WhereIf(checkLoan, x => x.Operation.CanceledOperation.GetNullableId() != null || x.IsReturnLoan)
                .Select(x => new AmountDto
                {
                    Id = x.Owner.Id,
                    Amount = x.Operation.CanceledOperation.GetNullableId() != null || x.IsReturnLoan ? -1 * x.Amount : x.Amount
                })
                .ToList();
        }

        private class RoPaymentAccountDto
        {
            public long Id { get; set; }
            public long RealityObjectId { get; set; }
            public string BaseTariffPaymentWalletGuid { get; set; }
            public string DecisionPaymentWalletGuid { get; set; }
            public string PenaltyPaymentWalletGuid { get; set; }
            public decimal MoneyLocked { get; set; }
        }

        private class AmountDto
        {
            public long Id { get; set; }
            public decimal Amount { get; set; }
        }
    }
}