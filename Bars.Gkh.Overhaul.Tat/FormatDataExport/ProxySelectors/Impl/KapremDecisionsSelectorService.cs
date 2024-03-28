namespace Bars.Gkh.Overhaul.Tat.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Селектор 2.22.2 Решения по капитальному ремонту (kapremdecisions.csv)
    /// </summary>
    public class KapremDecisionsSelectorService : BaseProxySelectorService<KapremDecisionsProxy>
    {
        /// <inheritdoc />
        protected override ICollection<KapremDecisionsProxy> GetAdditionalCache()
        {
            var protocolRepository = this.Container.ResolveRepository<BasePropertyOwnerDecision>();
            using (this.Container.Using(protocolRepository))
            {
                var query = protocolRepository.GetAll()
                    .WhereContainsBulked(x => x.Id, this.AdditionalIds);

                return this.GetProxies(query);
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, KapremDecisionsProxy> GetCache()
        {
            var protocolRepository = this.Container.ResolveRepository<BasePropertyOwnerDecision>();
            using (this.Container.Using(protocolRepository))
            {
                var roQuery = this.FilterService.GetFiltredQuery<RealityObject>();

                var query = protocolRepository.GetAll()
                    .Where(x => roQuery.Any(r => r.Id == x.RealityObject.Id))
                    .Where(x => x.RealityObject.TypeHouse == TypeHouse.ManyApartments
                        || x.RealityObject.TypeHouse == TypeHouse.BlockedBuilding)
                    .Where(x => x.PropertyOwnerProtocol.TypeProtocol == PropertyOwnerProtocolType.ResolutionOfTheBoard
                        || x.PropertyOwnerProtocol.TypeProtocol == PropertyOwnerProtocolType.FormationFund);

                return this.GetProxies(query)
                    .ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<KapremDecisionsProxy> GetProxies(IQueryable<BasePropertyOwnerDecision> query)
        {
            var calcAccountService = this.Container.Resolve<ICalcAccountService>();
            using (this.Container.Using(calcAccountService))
            {
                var roQuery = this.FilterService.GetFiltredQuery<RealityObject>();
                var rschetDict = calcAccountService.GetRobjectsAllAccounts(roQuery, this.FilterService.ExportDate);

                return query
                    .Select(x => new
                    {
                        x.Id,
                        RoId = x.RealityObject.Id,
                        x.PropertyOwnerProtocol.TypeProtocol,
                        x.PropertyOwnerDecisionType,
                        x.MethodFormFund,
                        x.MonthlyPayment,
                        ProtocolId = x.PropertyOwnerProtocol.Id,
                        x.PropertyOwnerProtocol.DocumentDate,
                        x.PropertyOwnerProtocol.DocumentNumber,
                        x.PropertyOwnerProtocol.DocumentFile,
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var basisReason = this.GetPropertyOwnerProtocol(x.TypeProtocol);
                        var type = this.GetTypeDecision(x.PropertyOwnerDecisionType);
                        var accounts = rschetDict.Get(x.RoId);
                        var rschet = accounts?.Where(y => y.DateOpen <= x.DocumentDate)
                                .FirstOrDefault()
                            ?? accounts?.FirstOrDefault();
                        var contragentRschetId = rschet?.TypeAccount != TypeCalcAccount.Regoperator
                            ? rschet?.Id
                            : default(long?);
                        var regopSchetId = rschet?.TypeAccount == TypeCalcAccount.Regoperator
                            || rschet?.TypeOwner == TypeOwnerCalcAccount.Regoperator
                                ? rschet?.Id
                                : default(long?);
                        return new KapremDecisionsProxy
                        {
                            Id = x.Id,
                            HouseId = x.RoId,
                            BasisReason = basisReason,
                            Type = type,
                            StartDate = x.DocumentDate,
                            FundType = this.GetMethodFormFundCr(x.MethodFormFund),
                            CrPayment = type == 3 ? x.MonthlyPayment : default(decimal?),
                            ProtocolId = basisReason == 1 ? x.ProtocolId : default(long?),
                            ProtocolNumber = basisReason == 1 ? x.DocumentNumber : default(string),
                            ProtocolDate = basisReason == 1 ? x.DocumentDate : default(DateTime?),
                            DocName = basisReason == 2 ? x.TypeProtocol.GetDisplayName() : default(string),
                            DocKind = basisReason == 2 ? "Решение ОМС" : default(string),
                            DocNumber = basisReason == 2 ? x.DocumentNumber : default(string),
                            DocDate = basisReason == 2 ? x.DocumentDate : default(DateTime?),
                            NpaId = x.TypeProtocol == PropertyOwnerProtocolType.ResolutionOfTheBoard ? x.ProtocolId : default(long?),
                            ContragentRschetId = contragentRschetId,
                            RegopSchetId = regopSchetId,

                            File = x.DocumentFile,
                            FileType = this.GetFileType(x.TypeProtocol, x.MethodFormFund)
                        };
                    })
                    .ToList();
            }
        }

        private int? GetPropertyOwnerProtocol(PropertyOwnerProtocolType? propertyOwnerDecisionType)
        {
            switch (propertyOwnerDecisionType)
            {
                case PropertyOwnerProtocolType.FormationFund:
                    return 1;
                case PropertyOwnerProtocolType.ResolutionOfTheBoard:
                    return 2;
                default:
                    return null;
            }
        }

        private int? GetTypeDecision(PropertyOwnerDecisionType? decisionType)
        {
            switch (decisionType)
            {
                case PropertyOwnerDecisionType.SelectMethodForming:
                    return 1;
                case PropertyOwnerDecisionType.SetMinAmount:
                    return 3;
                default:
                    return null;
            }
        }

        private int? GetMethodFormFundCr(MethodFormFundCr? methodFormFundCr)
        {
            switch (methodFormFundCr)
            {
                case MethodFormFundCr.SpecialAccount:
                    return 1;
                case MethodFormFundCr.RegOperAccount:
                    return 2;
                default:
                    return null;
            }
        }

        private int? GetFileType(PropertyOwnerProtocolType? propertyOwnerDecisionType, MethodFormFundCr? methodFormFundCr)
        {
            if (methodFormFundCr == MethodFormFundCr.SpecialAccount)
            {
                return 3;
            }

            switch (propertyOwnerDecisionType)
            {
                case PropertyOwnerProtocolType.FormationFund:
                    return 1;
                case PropertyOwnerProtocolType.ResolutionOfTheBoard:
                    return 2;
                default:
                    return null;
            }
        }
    }
}
