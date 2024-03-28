namespace Bars.Gkh.Modules.ClaimWork.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Entities.Owner;
    using Entities;
    using Enums;
    using Lawsuit;

    /// <summary>
    /// Правило для создания искового заявления ПИР
    /// </summary>
    public class LawSuitRule : DocumentClwCreateRule
    {
        public override string Id => "LawSuitCreateRule";

        public override string Description => "Создание искового заявления ПИР";      

        public override string ActionUrl => "lawsuit"; 

        public override ClaimWorkDocumentType ResultTypeDocument => ClaimWorkDocumentType.Lawsuit;

        /// <inheritdoc />
        public override IDataResult CreateDocument(BaseClaimWork claimWork)
        {
            var petitionDomain = this.Container.ResolveDomain<Petition>();
            var petitionToCourtDomain = this.Container.ResolveDomain<PetitionToCourtType>();
            var indOwnerDomain = this.Container.ResolveDomain<LawsuitIndividualOwnerInfo>();
            var claimWorkInfoServices = this.Container.ResolveAll<IClaimWorkInfoService>();
            var autoSelectors = this.Container.ResolveAll<ILawsuitAutoSelector>();

            using (this.Container.Using(petitionDomain, petitionToCourtDomain, claimWorkInfoServices, autoSelectors))
            {
                var dict = new DynamicDictionary();
                var indOwners = new List<LawsuitIndividualOwnerInfo>();

                claimWorkInfoServices.ForEach(x => x.GetInfo(claimWork, dict));

                var debtBaseTariffSum = dict.GetAs<decimal>("CurrChargeBaseTariffDebt");
                var debtDecisionTariffSum = dict.GetAs<decimal>("CurrChargeDecisionTariffDebt");
                var debtSum = dict.GetAs<decimal>("CurrChargeDebt");
                var penaltyDebt = dict.GetAs<decimal>("CurrPenaltyDebt");

                var petitionCodeOne = petitionToCourtDomain.GetAll().FirstOrDefault(x => x.Code == "1");

                var lawsuit = petitionDomain.GetAll().OrderByDescending(x => x.Id).FirstOrDefault(x => x.ClaimWork.Id == claimWork.Id);

                //if (lawsuit == null)
                //{
                //    this.Container.InTransaction(() =>
                //    {
                //        lawsuit = this.CreatePetition(claimWork, debtBaseTariffSum, debtDecisionTariffSum, debtSum, penaltyDebt, petitionCodeOne, autoSelectors);
                //        petitionDomain.Save(lawsuit);

                //        this.CreateDocumentDetail(lawsuit);
                //    });
                //}

                DateTime? debtEndDate = null;
                if (lawsuit != null)
                {
                    debtEndDate = lawsuit.DebtEndDate;

                    indOwners = indOwnerDomain.GetAll().Where(x => x.Lawsuit.Id == lawsuit.Id).ToList();
                }

                this.Container.InTransaction(() =>
                {
                    lawsuit = this.CreatePetition(claimWork, debtBaseTariffSum, debtDecisionTariffSum, debtSum, penaltyDebt, petitionCodeOne, autoSelectors, debtEndDate);
                    petitionDomain.Save(lawsuit);

                    this.CreateDocumentDetail(lawsuit);

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
                            Lawsuit = lawsuit,
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

                return new BaseDataResult(lawsuit);
            }
        }

        /// <inheritdoc />
        public override IEnumerable<DocumentClw> FormDocument(IEnumerable<BaseClaimWork> claimWorks, bool fillDebts = true)
        {
            var petitionDomain = this.Container.ResolveDomain<Petition>();
            var petitionToCourt = this.Container.ResolveDomain<PetitionToCourtType>();
            var claimWorkInfoServices = this.Container.ResolveAll<IClaimWorkInfoService>();
            var autoSelectors = this.Container.ResolveAll<ILawsuitAutoSelector>();

            try
            {
                var claimWorkIds = claimWorks.Select(x => x.Id).ToArray();

                var claimWorkWithDoc = petitionDomain.GetAll()
                    .Where(x => claimWorkIds.Contains(x.ClaimWork.Id))
                    .Select(x => x.ClaimWork.Id)
                    .ToArray();

                var petitionCodeOne = petitionToCourt.GetAll().FirstOrDefault(x => x.Code == "1");

                var result = new List<Petition>();

                foreach (var claimWork in claimWorks.Where(x => !claimWorkWithDoc.Contains(x.Id)))
                {
                    var debtBaseTariffSum = 0m;
                    var debtDecisionTariffSum = 0m;
                    var debtSum = 0m;
                    var penaltyDebt = 0m;

                    if (fillDebts)
                    {
                        var dict = new DynamicDictionary();
                        claimWorkInfoServices.ForEach(x => x.GetInfo(claimWork, dict));

                        debtBaseTariffSum = dict.GetAs<decimal>("CurrChargeBaseTariffDebt");
                        debtDecisionTariffSum = dict.GetAs<decimal>("CurrChargeDecisionTariffDebt");
                        debtSum = dict.GetAs<decimal>("CurrChargeDebt");
                        penaltyDebt = dict.GetAs<decimal>("CurrPenaltyDebt");
                    }
                    var lawsuit = this.CreatePetition(claimWork, debtBaseTariffSum, debtDecisionTariffSum, debtSum, penaltyDebt, petitionCodeOne, autoSelectors, null);
                    result.Add(lawsuit);
                }

                return result;
            }
            finally
            {
                this.Container.Release(petitionDomain);
                this.Container.Release(petitionToCourt);
                this.Container.Release(claimWorkInfoServices);
                autoSelectors.ForEach(x => this.Container.Release(x));
            }
        }

        private Petition CreatePetition(
            BaseClaimWork claimWork,
            decimal debtBaseTariffSum,
            decimal debtDecisionTariffSum,
            decimal debtSum,
            decimal penaltyDebt,
            PetitionToCourtType petitionCodeOne,
            ILawsuitAutoSelector[] autoSelectors,
            DateTime? debtEndDate)
        {
            var lawsuit = new Petition
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
                DocumentType = ClaimWorkDocumentType.Lawsuit,
                LawsuitDocType = LawsuitDocumentType.NotSet,
                ResultConsideration = LawsuitResultConsideration.NotSet,
                DocumentDate = DateTime.Now,
                PetitionType = petitionCodeOne,
                IsLimitationOfActions = debtEndDate.HasValue ? true : false,
                DebtStartDate = debtEndDate.HasValue ? debtEndDate.Value.AddDays(1) : debtEndDate,
                DateLimitationOfActions = debtEndDate.HasValue ? debtEndDate.Value.AddDays(1) : debtEndDate
            };

            autoSelectors.ForEach(x => x.TrySetAll(lawsuit));
            return lawsuit;
        }
    }
}