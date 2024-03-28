namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Селектор Протоколов общего собрания собственников, которыми принято решение о формирования фонда капитального ремонта
    /// </summary>
    public class KapremDecisionsSelectorService : BaseProxySelectorService<KapremDecisionsProxy>
    {
        protected readonly Lazy<IDictionary<long, int?>> FundFormDecisionCache;

        public KapremDecisionsSelectorService()
        {
            this.FundFormDecisionCache = new Lazy<IDictionary<long, int?>>(this.InitFundFormDecisionCache, LazyThreadSafetyMode.PublicationOnly);
        }

        /// <inheritdoc />
        protected override ICollection<KapremDecisionsProxy> GetAdditionalCache()
        {
            var roDecisionProtocolRepository = this.Container.ResolveRepository<RealityObjectDecisionProtocol>();
            var govDecisionRepository = this.Container.ResolveRepository<GovDecision>();

            using (this.Container.Using(roDecisionProtocolRepository, govDecisionRepository))
            {
                var roDecisionProtocolQuery = roDecisionProtocolRepository.GetAll()
                    .WhereContainsBulked(x => x.ExportId, this.AdditionalIds);
                var govDecisionQuery = govDecisionRepository.GetAll()
                    .WhereContainsBulked(x => x.ExportId, this.AdditionalIds);
                return this.GetProxies(roDecisionProtocolQuery, govDecisionQuery);
            }
        }

        protected override IDictionary<long, KapremDecisionsProxy> GetCache()
        {
            var roDecisionProtocolRepository = this.Container.ResolveRepository<RealityObjectDecisionProtocol>();
            var govDecisionRepository = this.Container.ResolveRepository<GovDecision>();
            var chargePeriodRepository = this.Container.Resolve<IChargePeriodRepository>();

            using (this.Container.Using(roDecisionProtocolRepository, govDecisionRepository, chargePeriodRepository))
            {
                var roQuery = this.FilterService.GetFiltredQuery<RealityObject>();

                var roDecisionProtocolQuery = roDecisionProtocolRepository.GetAll()
                    .Where(x => roQuery.Any(r => r.Id == x.RealityObject.Id))
                    .Where(x => x.RealityObject.TypeHouse == TypeHouse.ManyApartments || x.RealityObject.TypeHouse == TypeHouse.BlockedBuilding);
                var govDecisionQuery = govDecisionRepository.GetAll()
                    .Where(x => roQuery.Any(r => r.Id == x.RealityObject.Id))
                    .Where(x => x.RealityObject.TypeHouse == TypeHouse.ManyApartments || x.RealityObject.TypeHouse == TypeHouse.BlockedBuilding);
                return this.GetProxies(roDecisionProtocolQuery, govDecisionQuery)
                    .ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<KapremDecisionsProxy> GetProxies(IQueryable<RealityObjectDecisionProtocol> roDecisionProtocolQuery,
            IQueryable<GovDecision> govDecisionQuery)
        {
            var monthlyFeeAmountDecisionRepository = this.Container.ResolveRepository<MonthlyFeeAmountDecision>();
            var calcAccountService = this.Container.Resolve<ICalcAccountService>();
            using (this.Container.Using(monthlyFeeAmountDecisionRepository, calcAccountService))
            {
                var roQuery = this.FilterService.GetFiltredQuery<RealityObject>();
                var rschetDict = calcAccountService.GetRobjectsAllAccounts(roQuery, this.FilterService.ExportDate);

                var monthlyFeeDict = monthlyFeeAmountDecisionRepository.GetAll()
                    .Where(x => roDecisionProtocolQuery.Any(y => y == x.Protocol))
                    .Select(x => new
                    {
                        x.Protocol.ExportId,
                        x.IsChecked,
                        x.Decision
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.ExportId)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                var roDecisionProtocols = roDecisionProtocolQuery
                    .Select(x => new
                    {
                        Id = x.ExportId,
                        RoId = x.RealityObject.Id,
                        RoConditionHouse = x.RealityObject.ConditionHouse,
                        x.ProtocolDate,
                        x.DateStart,
                        x.DocumentNum,
                        x.File,
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var accounts = rschetDict.Get(x.RoId);
                        var rschet = accounts?.Where(y => y.DateOpen <= x.DateStart)
                                .FirstOrDefault()
                            ?? accounts?.FirstOrDefault();
                        var contragentRschetId = rschet?.TypeAccount != TypeCalcAccount.Regoperator
                            ? rschet?.Id
                            : default(long?);
                        var regopSchetId = rschet?.TypeAccount == TypeCalcAccount.Regoperator
                            || rschet?.TypeOwner == TypeOwnerCalcAccount.Regoperator
                            ? rschet?.Id
                            : default(long?);
                        var monthlyFee = monthlyFeeDict.Get(x.Id);
                        var formationVariant = this.FundFormDecisionCache.Value.Get(x.Id);
                        var type = (monthlyFee?.IsChecked ?? false) ? 3 : 1;
                        return new KapremDecisionsProxy
                        {
                            Id = x.Id,
                            HouseId = x.RoId,
                            BasisReason = 1,
                            Type = type,
                            StartDate = x.DateStart,
                            FundType = formationVariant,
                            CrPayment = type == 3
                                ? monthlyFee?.Decision?.Where(y => y.From <= this.FilterService.ExportDate)
                                    .OrderByDescending(y => y.From)
                                    .Select(y => (decimal?) y.Value)
                                    .FirstOrDefault()
                                : default(decimal?),
                            ProtocolId = this.FilterService.Provider != FormatDataExportProviderType.RegOpCr ? x.Id : (long?) null,
                            ProtocolNumber = x.DocumentNum,
                            ProtocolDate = x.ProtocolDate,
                            ContragentRschetId = contragentRschetId,
                            RegopSchetId = regopSchetId,

                            File = x.File,
                            FileType = 1 // Протокол собрания собственников
                        };
                    });

                return govDecisionQuery
                    .Select(x => new
                    {
                        Id = x.ExportId,
                        RoId = x.RealityObject.Id,
                        RoConditionHouse = x.RealityObject.ConditionHouse,
                        x.ProtocolDate,
                        x.DateStart,
                        DocumentNum = x.ProtocolNumber,
                        File = x.ProtocolFile,
                        x.MaxFund,
                        State = x.State.Name
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var accounts = rschetDict.Get(x.RoId);
                        var rschet = accounts?.Where(y => y.DateOpen <= x.DateStart)
                                .FirstOrDefault()
                            ?? accounts?.FirstOrDefault();
                        var contragentRschetId = rschet?.TypeAccount != TypeCalcAccount.Regoperator
                            ? rschet?.Id
                            : default(long?);
                        var regopSchetId = rschet?.TypeAccount == TypeCalcAccount.Regoperator
                            || rschet?.TypeOwner == TypeOwnerCalcAccount.Regoperator
                                ? rschet?.Id
                                : default(long?);
                        var type = x.MaxFund != 0 ? 3 : 1;
                        return new KapremDecisionsProxy
                        {
                            Id = x.Id,
                            HouseId = x.RoId,
                            BasisReason = 2,
                            Type = type,
                            StartDate = x.DateStart,
                            FundType = 2, // регоп
                            CrPayment = type == 3 ? x.MaxFund : default(decimal?),
                            DocName = "Протокол решения органа гос власти",
                            DocKind = "Протокол",
                            DocNumber = x.DocumentNum,
                            DocDate = x.ProtocolDate,
                            NpaId = x.Id,
                            ContragentRschetId = contragentRschetId,
                            RegopSchetId = regopSchetId,

                            File = x.File,
                            FileType = 2
                        };
                    })
                    .Union(roDecisionProtocols)
                    .ToList();
            }
        }

        private IDictionary<long, int?> InitFundFormDecisionCache()
        {
            var crFundFormationDecisionRepository = this.Container.ResolveRepository<CrFundFormationDecision>();
            using (this.Container.Using(crFundFormationDecisionRepository))
            {
                return crFundFormationDecisionRepository.GetAll()
                    .Select(x => new
                    {
                        x.Protocol.ExportId,
                        x.Decision
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.ExportId, x => this.GetMethodFormFundCr(x.Decision))
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());
            }
        }

        private int? GetMethodFormFundCr(CrFundFormationDecisionType? type)
        {
            switch (type)
            {
                case CrFundFormationDecisionType.SpecialAccount:
                    return 1;
                case CrFundFormationDecisionType.RegOpAccount:
                    return 2;
                default:
                    return null;
            }
        }
    }
}