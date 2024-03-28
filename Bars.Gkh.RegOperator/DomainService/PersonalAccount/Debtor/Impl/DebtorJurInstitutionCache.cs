namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Debtor.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.ClaimWork;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Core;

    using CourtType = Bars.Gkh.Modules.ClaimWork.Enums.CourtType;

    /// <summary>
    /// Сервис для заполнения полей Учреждение в судебной практике и тип суда
    /// </summary>
    public class DebtorJurInstitutionCache : IDebtorJurInstitutionCache, IInitializable
    {
        private readonly List<CourtProceedingConfig> courtConfigs = new List<CourtProceedingConfig>();
        private Dictionary<long, Dictionary<CourtType, JurInstitution>> magistrateJurInstDict;
        private Dictionary<long, JurInstitution> arbitrationJurInstDict;
        private Dictionary<long, JurInstitution> districtJurInstDict;

        public IDomainService<JurInstitution> JurInstitutionDomain { get; set; }

        public IDomainService<JurInstitutionRealObj> JurInstitutionRealObjDomain { get; set; }

        public IGkhConfigProvider ConfigProvider { get; set; }

        /// <inheritdoc />
        public void InitCache(long[] roIds)
        {
            this.magistrateJurInstDict = this.JurInstitutionRealObjDomain.GetAll()
                .Where(x => roIds.Contains(x.RealityObject.Id))
                .Where(x => x.JurInstitution.CourtType == CourtType.Magistrate)
                .Select(x => new
                {
                    x.RealityObject.Id,
                    x.JurInstitution
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y.GroupBy(x => x.JurInstitution.CourtType).ToDictionary(z => z.Key, k => k.Select(x => x.JurInstitution).First()));

            this.arbitrationJurInstDict = this.JurInstitutionDomain.GetAll()
                .Where(x => x.CourtType == CourtType.Arbitration)
                .GroupBy(x => x.Municipality.Id)
                .ToDictionary(x => x.Key, y => y.First());

            this.districtJurInstDict = this.JurInstitutionDomain.GetAll()
                .Where(x => x.CourtType == CourtType.District)
                .GroupBy(x => x.Municipality.Id)
                .ToDictionary(x => x.Key, y => y.First());
        }

        /// <inheritdoc />
        public void InitCache(IQueryable<long> roIdQuery)
        {
            this.magistrateJurInstDict = this.JurInstitutionRealObjDomain.GetAll()
                .Where(x => x.JurInstitution.CourtType == CourtType.Magistrate)
                .Where(x => roIdQuery.Any(id => id == x.RealityObject.Id))
                .Select(x => new
                {
                    x.RealityObject.Id,
                    x.JurInstitution
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y.GroupBy(x => x.JurInstitution.CourtType, x => x.JurInstitution).ToDictionary(z => z.Key, k => k.First()));

            this.arbitrationJurInstDict = this.JurInstitutionDomain.GetAll()
                .Where(x => x.CourtType == CourtType.Arbitration)
                .GroupBy(x => x.Municipality.Id)
                .ToDictionary(x => x.Key, y => y.First());

            this.districtJurInstDict = this.JurInstitutionDomain.GetAll()
                .Where(x => x.CourtType == CourtType.District)
                .GroupBy(x => x.Municipality.Id)
                .ToDictionary(x => x.Key, y => y.First());
        }

        /// <inheritdoc />
        public bool SetJurInstitution(Debtor debtor, BasePersonalAccount account)
        {
            return this.SetJurInstitution(
                debtor,
                account.AccountOwner.OwnerType,
                account.Room.RealityObject.Id,
                account.Room.RealityObject.Municipality.Id,
                account.Room.RealityObject.MoSettlement.IsNotNull() ? account.Room.RealityObject.MoSettlement.Id : account.Room.RealityObject.Municipality.Id);
        }

        /// <inheritdoc />
        public bool SetJurInstitution(Debtor debtor, DebtorCalcService.BasePersonalAccountDto account)
        {
            return this.SetJurInstitution(
                debtor,
                account.OwnerType,
                account.RealityObjectId,
                account.MunicipalityId,
                account.MoSettlementId);
        }

        private bool SetJurInstitution(
            Debtor debtor,
            PersonalAccountOwnerType ownerType,
            long realityObjectId,
            long municipalityId,
            long moSettlementId)
        {
            var courtType = this.courtConfigs.Where(
                    x => ownerType == PersonalAccountOwnerType.Individual
                        ? x.DebtorType == DebtorType.Individual
                        : x.DebtorType == DebtorType.Legal)
                .Where(x => debtor.DebtSum >= x.MinClaimAmount && debtor.DebtSum <= x.MaxClaimAmount)
                .Select(x => x.CourtType)
                .FirstOrDefault();

            if (courtType == Gkh.Enums.ClaimWork.CourtType.NotSet)
            {
                courtType = ownerType == PersonalAccountOwnerType.Individual
                    ? Gkh.Enums.ClaimWork.CourtType.Magistrate
                    : Gkh.Enums.ClaimWork.CourtType.Arbitration;
            }

            JurInstitution jurInst = null;
            switch (courtType)
            {
                case Gkh.Enums.ClaimWork.CourtType.Magistrate:
                    {
                        jurInst = this.magistrateJurInstDict.Get(realityObjectId)
                        ?.Get((CourtType)courtType);
                    }
                    break;

                case Gkh.Enums.ClaimWork.CourtType.Arbitration:
                    {
                        jurInst = this.arbitrationJurInstDict.Get(municipalityId)
                            ?? (municipalityId == 0
                                ? null
                                : this.arbitrationJurInstDict.Get(moSettlementId));
                    }
                    break;

                case Gkh.Enums.ClaimWork.CourtType.District:
                    {
                        jurInst = this.districtJurInstDict.Get(municipalityId)
                            ?? (moSettlementId == 0
                                ? null
                                : this.districtJurInstDict.Get(moSettlementId));
                    }
                    break;
            }

            if (debtor.CourtType != courtType || debtor.JurInstitution != jurInst)
            {
                debtor.CourtType = courtType;
                debtor.JurInstitution = jurInst;
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public void Initialize()
        {
            var courtProceeding = this.ConfigProvider.Get<ClaimWorkConfig>()?
                .Common?
                .CourtProceeding;
            if (courtProceeding != null)
            {
                this.courtConfigs.AddRange(courtProceeding
                    .Where(x => x != null)
                    .Where(x => x.ReasonType == ReasonType.Debtor));
            }
        }
    }
}