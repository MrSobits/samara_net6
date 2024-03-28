namespace Bars.Gkh.Decisions.Nso.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using Entities;
    using Gkh.Entities;
    using Gkh.Entities.RealEstateType;
    using Overhaul.Domain.Impl;
    using Overhaul.Entities;

    public class PaysizeRepositoryDecisions : PaysizeRepository
    {
        private readonly IUltimateDecisionService _ultDecisionService;
        private readonly IDomainService<RealityObject> _robjectDomain;

        public PaysizeRepositoryDecisions(
            IDomainService<PaysizeRecord> paysizeDomain,
            IDomainService<PaysizeRealEstateType> paysizeRetDomain,
            IDomainService<RealEstateTypeRealityObject> roTypeDomain,
            IDomainService<RealityObject> robjectDomain,
            IUltimateDecisionService ultDecisionService)
            : base(paysizeDomain, paysizeRetDomain, roTypeDomain)
        {
            _robjectDomain = robjectDomain;
            _ultDecisionService = ultDecisionService;
        }

        public override decimal GetRoPaysize(RealityObject robject, DateTime date)
        {
            InitCache();

            if (_roMonthlyFeeCache.ContainsKey(robject.Id))
            {
                var monthlyFee =
                    _roMonthlyFeeCache[robject.Id]
                        .Where(x => x.From <= date)
                        .FirstOrDefault(x => !x.To.HasValue || x.To >= date);

                if (monthlyFee != null)
                {
                    return monthlyFee.Value;
                }
            }

            return base.GetRoPaysize(robject, date);
        }

        public override decimal? GetRoPaysizeByDecision(RealityObject robject, DateTime date)
        {
            InitCache();

            if (_roMonthlyFeeCache.ContainsKey(robject.Id))
            {
                var monthlyFee =
                    _roMonthlyFeeCache[robject.Id]
                        .Where(x => x.From <= date)
                        .FirstOrDefault(x => !x.To.HasValue || x.To >= date);

                if (monthlyFee != null)
                {
                    return monthlyFee.Value;
                }
            }

            return base.GetRoPaysizeByDecision(robject, date);
        }

        protected override void InitCache()
        {
            base.InitCache();

            if (_roMonthlyFeeCache == null)
            {
                _roMonthlyFeeCache =
                    _ultDecisionService.GetActualDecisions<MonthlyFeeAmountDecision>(_robjectDomain.GetAll())
                        .Select(x => new
                        {
                            RoId = x.Protocol.RealityObject.Id,
                            x.Decision
                        })
                        .GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key, z => z.SelectMany(x => x.Decision));
            }
        }


        private Dictionary<long, IEnumerable<PeriodMonthlyFee>> _roMonthlyFeeCache;
    }
}