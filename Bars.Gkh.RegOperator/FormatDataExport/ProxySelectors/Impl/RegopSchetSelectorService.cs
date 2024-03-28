namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Селектор Расчетные счета фонда капитального ремонта
    /// </summary>
    public class RegopSchetSelectorService : BaseProxySelectorService<RegopSchetProxy>
    {
        /// <inheritdoc />
        protected override ICollection<RegopSchetProxy> GetAdditionalCache()
        {
            var regopCalcAccountRepository = this.Container.ResolveRepository<RegopCalcAccount>();
            var specialCalcAccountRepository = this.Container.ResolveRepository<SpecialCalcAccount>();

            using (this.Container.Using(regopCalcAccountRepository, specialCalcAccountRepository))
            {
                return this.GetProxies(regopCalcAccountRepository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds),
                        specialCalcAccountRepository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds))
                    .ToList();
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, RegopSchetProxy> GetCache()
        {
            var regopCalcAccountRepository = this.Container.ResolveRepository<RegopCalcAccount>();
            var specialCalcAccountRepository = this.Container.ResolveRepository<SpecialCalcAccount>();
            var calcAccountRealityObjectRepository = this.Container.ResolveRepository<CalcAccountRealityObject>();

            using (this.Container.Using(regopCalcAccountRepository, specialCalcAccountRepository, calcAccountRealityObjectRepository))
            {
                var roQuery = this.FilterService.GetFiltredQuery<RealityObject>();

                var calcRoQuery = calcAccountRealityObjectRepository.GetAll()
                    .Where(x => roQuery.Any(y => y.Id == x.RealityObject.Id))
                    .Select(x => x.Account);

                var regopCalcAccountQuery = regopCalcAccountRepository.GetAll()
                    .Where(x => calcRoQuery.Any(y => y == x));
                var specialCalcAccountQuery = specialCalcAccountRepository.GetAll()
                    .Where(x => calcRoQuery.Any(y => y == x));

                return this.GetProxies(regopCalcAccountQuery, specialCalcAccountQuery)
                    .ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<RegopSchetProxy> GetProxies(IQueryable<RegopCalcAccount> regopCalcQuery,
            IQueryable<SpecialCalcAccount> specialCalcQuery)
        {
            return ContragentRschetSelectorService.GetProxies(regopCalcQuery, specialCalcQuery)
                .Where(x => x.IsRegopAccount)
                .Select(x => new RegopSchetProxy
                {
                    Id = x.Id,
                    TypeAccount = x.RegopAccountType,
                    ContragentId = x.ContragentId,
                    ContragentRschetId = x.Id,
                    Status = x.CloseDate.HasValue ? 2 : 1,
                })
                .ToList();
        }
    }
}