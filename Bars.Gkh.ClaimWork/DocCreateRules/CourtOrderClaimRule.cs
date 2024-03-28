namespace Bars.Gkh.Modules.ClaimWork.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Modules.ClaimWork.DomainService.Lawsuit;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities.Owner;

    /// <summary>
    /// Правило создания документа - заявления о выдаче судебного приказа
    /// </summary>
    public class CourtOrderClaimRule : DocumentClwCreateRule
    {
        /// <summary>
        /// Идентификатр реализации
        /// </summary>
        public override string Id => "CourtOrderClaimRule";

        /// <summary>
        /// Краткое описание
        /// </summary>
        public override string Description => "Создание заявления о выдаче судебного приказа";

        /// <summary>
        /// Карточка, которую нужно открыть после создания дкоумента
        /// </summary>
        public override string ActionUrl => "lawsuit";

        /// <summary>
        /// Тип документа результата, тоесть того который должен получится в резултате формирвоания
        /// </summary>
        public override ClaimWorkDocumentType ResultTypeDocument => ClaimWorkDocumentType.CourtOrderClaim;

        /// <inheritdoc />
        public override IDataResult CreateDocument(BaseClaimWork claimWork)
        {
            var courtOrderDomain = this.Container.ResolveDomain<CourtOrderClaim>();
            var petitionDomain = this.Container.ResolveDomain<PetitionToCourtType>();
            var claimWorkInfoServices = this.Container.ResolveAll<IClaimWorkInfoService>();
            var autoSelectors = this.Container.ResolveAll<ILawsuitAutoSelector>();
            var jurInstitutionRealObjDomain = this.Container.ResolveDomain<JurInstitutionRealObj>();
            var indOwnerDomain = this.Container.ResolveDomain<LawsuitIndividualOwnerInfo>();

            using (this.Container.Using(courtOrderDomain, petitionDomain, claimWorkInfoServices, autoSelectors, jurInstitutionRealObjDomain))
            {
                var indOwners = new List<LawsuitIndividualOwnerInfo>();

                var debtBaseTariffSum = claimWork.GetValue("CurrChargeBaseTariffDebt").ToDecimal();
                var debtDecisionTariffSum = claimWork.GetValue("CurrChargeDecisionTariffDebt").ToDecimal();
                var debtSum = claimWork.GetValue("CurrChargeDebt").ToDecimal();
                var penaltyDebt = claimWork.GetValue("CurrPenaltyDebt").ToDecimal();

                var petitionCodeOne = petitionDomain.GetAll().FirstOrDefault(x => x.Code == "1");

                var petitionCodeFive = petitionDomain.GetAll().FirstOrDefault(x => x.Code == "5");

                var courtOrder = courtOrderDomain.GetAll().OrderByDescending(x=> x.Id).FirstOrDefault(x => x.ClaimWork.Id == claimWork.Id);

                var roId = claimWork.RealityObject?.Id ?? 0;
                DateTime? debtEndDate = null;
                if (courtOrder != null)
                {
                    debtEndDate = courtOrder.DebtEndDate;

                    indOwners = indOwnerDomain.GetAll().Where(x => x.Lawsuit.Id == courtOrder.Id).ToList();
                }

             //   if (courtOrder == null)
             //  {
                this.Container.InTransaction(() =>
                    {
                        var jurInstitutionRealObj = jurInstitutionRealObjDomain.GetAll()
                        .Where(x => x.RealityObject.Id == roId)
                        .Select(x => x.JurInstitution)
                        .FirstOrDefault();

                        courtOrder = this.CreateCourtOrderClaim(
                            claimWork,
                            0,
                            0,
                            0,
                            0,
                            petitionCodeFive,
                            jurInstitutionRealObj,
                            autoSelectors, debtEndDate);
                        courtOrderDomain.Save(courtOrder);

                        this.CreateDocumentDetail(courtOrder);

                        foreach (var owner in indOwners)
                        {
                            var newOwner = new LawsuitIndividualOwnerInfo()
                            {
                                Name = owner.Name,
                                OwnerType = owner.OwnerType,
                                PersonalAccount = owner.PersonalAccount,
                                StartPeriod = owner.StartPeriod,
                                EndPeriod = owner.EndPeriod,
                                AreaShareNumerator = owner.AreaShareNumerator,
                                AreaShareDenominator = owner.AreaShareDenominator,
                                DebtBaseTariffSum = owner.DebtBaseTariffSum,
                                DebtDecisionTariffSum = owner.DebtDecisionTariffSum,
                                PenaltyDebt = owner.PenaltyDebt,
                                Lawsuit = courtOrder,
                                SharedOwnership = owner.SharedOwnership,
                                Underage = owner.Underage,
                                ClaimNumber = owner.ClaimNumber,
                                SNILS = owner.SNILS,
                                JurInstitution = owner.JurInstitution,

                                Surname = owner.Surname,
                                FirstName = owner.FirstName,
                                SecondName = owner.SecondName,
                                BirthDate = owner.BirthDate,
                                BirthPlace = owner.BirthPlace,
                                LivePlace = owner.LivePlace,
                                DocIndCode = owner.DocIndCode,
                                DocIndName = owner.DocIndName,
                                DocIndSerial = owner.DocIndSerial,
                                DocIndNumber = owner.DocIndNumber,
                                DocIndDate = owner.DocIndDate,
                                DocIndIssue = owner.DocIndIssue
                            };

                            indOwnerDomain.Save(newOwner);
                        }
                    });
              //  }

                return new BaseDataResult(courtOrder);
            }
        }

        /// <inheritdoc />
        public override IEnumerable<DocumentClw> FormDocument(IEnumerable<BaseClaimWork> claimWorks, bool fillDebts = true)
        {
            var lawSuitDomain = this.Container.ResolveDomain<CourtOrderClaim>();
            var petitionDomain = this.Container.ResolveDomain<PetitionToCourtType>();
            var claimWorkInfoServices = this.Container.ResolveAll<IClaimWorkInfoService>();
            var autoSelectors = this.Container.ResolveAll<ILawsuitAutoSelector>();
            var jurInstitutionRealObjDomain = this.Container.ResolveDomain<JurInstitutionRealObj>();

            try
            {
                var claimWorkIds = claimWorks.Select(x => x.Id).ToArray();
                var claimWorkRoIds = claimWorks
                    .Where(x => x.RealityObject != null)
                    .Select(x => x.RealityObject.Id).ToArray();

                var claimWorkWithDoc = lawSuitDomain.GetAll()
                    .Where(x => claimWorkIds.Contains(x.ClaimWork.Id))
                    .Select(x => x.ClaimWork.Id)
                    .ToArray();

                var jurInstitutionsForAllClaimworks =
                    jurInstitutionRealObjDomain.GetAll()
                        .Where(x => claimWorkRoIds.Contains(x.RealityObject.Id))
                        .AsEnumerable()
                        .GroupBy(x => x.RealityObject.Id, x => x.JurInstitution)
                        .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                var petitionCodeOne = petitionDomain.GetAll().FirstOrDefault(x => x.Code == "1");

                var petitionCodeFive = petitionDomain.GetAll().FirstOrDefault(x => x.Code == "5");

                var result = new List<CourtOrderClaim>();

                foreach (var claimWork in claimWorks.Where(x => !claimWorkWithDoc.Contains(x.Id)))
                {
                    var jurInstitution = jurInstitutionsForAllClaimworks.Get(claimWork.RealityObject?.Id ?? 0);

                    var debtBaseTariffSum = claimWork.GetValue("CurrChargeBaseTariffDebt").ToDecimal();
                    var debtDecisionTariffSum = claimWork.GetValue("CurrChargeDecisionTariffDebt").ToDecimal();
                    var debtSum = claimWork.GetValue("CurrChargeDebt").ToDecimal();
                    var penaltyDebt = claimWork.GetValue("CurrPenaltyDebt").ToDecimal();

                    var lawsuit = this.CreateCourtOrderClaim(claimWork,
                        debtBaseTariffSum,
                        debtDecisionTariffSum,
                        debtSum,
                        penaltyDebt,
                        petitionCodeFive,
                        jurInstitution,
                        autoSelectors, null);
                    result.Add(lawsuit);
                }

                return result;
            }
            finally
            {
                this.Container.Release(lawSuitDomain);
                this.Container.Release(petitionDomain);
                this.Container.Release(claimWorkInfoServices);
                autoSelectors.ForEach(x => this.Container.Release(x));
                this.Container.Release(jurInstitutionRealObjDomain);
            }
        }

        private CourtOrderClaim CreateCourtOrderClaim(
            BaseClaimWork claimWork,
            decimal debtBaseTariffSum,
            decimal debtDecisionTariffSum,
            decimal debtSum,
            decimal penaltyDebt,
            PetitionToCourtType petitionCodeFive,
            JurInstitution jurInstitution,
            ILawsuitAutoSelector[] autoSelectors, DateTime? debtEndDate)
        {
            var lawsuit = new CourtOrderClaim
            {
                ClaimWork = claimWork,
                DebtBaseTariffSum = debtBaseTariffSum,
                DebtDecisionTariffSum = debtDecisionTariffSum,
                DebtSum = debtSum,
                PenaltyDebt = penaltyDebt,
                CbDebtSum = debtSum,
                CbPenaltyDebt = penaltyDebt,
                WhoConsidered = LawsuitConsiderationType.NotSet,
                CbDocumentType = LawsuitCollectionDebtDocumentType.NotSet,
                CbReasonStoppedType = LawsuitCollectionDebtReasonStoppedType.NotSet,
                DocumentType = ClaimWorkDocumentType.CourtOrderClaim,
                LawsuitDocType = LawsuitDocumentType.Disposal,
                ResultConsideration = LawsuitResultConsideration.NotSet,
                DocumentDate = DateTime.Now,
                PetitionType = petitionCodeFive,
                JurInstitution = jurInstitution,
                IsLimitationOfActions = debtEndDate.HasValue? true:false,
                DebtStartDate = debtEndDate.HasValue ? debtEndDate.Value.AddDays(1) : debtEndDate,
                DateLimitationOfActions = debtEndDate.HasValue? debtEndDate.Value.AddDays(1): debtEndDate,
                /*CourtOrderClaim specific*/
                ObjectionArrived = YesNo.No
            };

            autoSelectors.ForEach(x => x.TrySetAll(lawsuit));

            return lawsuit;
        }
    }
}