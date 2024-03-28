namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Информация о размере фонда КР по помещениям (crfundsizepremises.csv)
    /// </summary>
    public class CrFundSizePremisesSelectorService : BaseProxySelectorService<CrFundSizePremisesProxy>
    {
        /// <inheritdoc />
        protected override ICollection<CrFundSizePremisesProxy> GetAdditionalCache()
        {
            var repository = this.Container.ResolveRepository<PersonalAccountPeriodSummary>();

            using (this.Container.Using(repository))
            {
                return this.GetProxies(repository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds));
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, CrFundSizePremisesProxy> GetCache()
        {
            var periodSummaryPeriodRepository = this.Container.ResolveRepository<PersonalAccountPeriodSummary>();
            var chargePeriodRepository = this.Container.Resolve<IChargePeriodRepository>();

            using (this.Container.Using(periodSummaryPeriodRepository, chargePeriodRepository))
            {
                var objectCrQuery = this.FilterService.GetFiltredQuery<ObjectCr>();

                var currentPeriodId = chargePeriodRepository.GetCurrentPeriod()?.Id ?? 0;
                var query = periodSummaryPeriodRepository.GetAll()
                    .Where(x => x.Period.Id == currentPeriodId)
                    .Where(x => objectCrQuery.Any(y => y.RealityObject.Id == x.PersonalAccount.Room.RealityObject.Id));

                return this.GetProxies(query).ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<CrFundSizePremisesProxy> GetProxies(IQueryable<PersonalAccountPeriodSummary> query)
        {
            return query
                .Select(x => new
                {
                    PersAccId = x.PersonalAccount.Id,
                    RoId = (long?) x.PersonalAccount.Room.RealityObject.Id,
                    x.SaldoIn,
                    x.Penalty,
                    x.ChargedByBaseTariff,
                    x.SaldoOut,
                    ChargeByDecision = (x.ChargeTariff - x.ChargedByBaseTariff) + x.RecalcByDecisionTariff + x.DecisionTariffChange,
                    ChargePenalty = x.Penalty + x.RecalcByPenalty + x.PenaltyChange,
                    x.PenaltyPayment,
                    x.TariffPayment,
                    x.TariffDecisionPayment
                })
                .AsEnumerable()
                .Select(x => new CrFundSizePremisesProxy
                {
                    Id = x.PersAccId,
                    CrFundSizeId = x.RoId,
                    PersAccountId = x.PersAccId,
                    OverpaymentOrDebtOnStartPeriod = x.SaldoIn,
                    Contribution = x.ChargedByBaseTariff + x.ChargeByDecision,
                    Penalty = x.ChargePenalty,
                    OverpaymentOrDebtOnEndPeriod = x.TariffPayment + x.TariffDecisionPayment,
                    PaidContribution = x.PenaltyPayment,
                    PaidPenalty = x.SaldoOut - x.ChargedByBaseTariff - x.ChargeByDecision - x.Penalty
                })
                .ToList();
        }
    }
}