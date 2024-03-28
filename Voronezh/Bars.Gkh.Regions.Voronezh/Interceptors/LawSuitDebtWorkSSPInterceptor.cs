using Bars.B4;
using Bars.B4.DataAccess;
using Bars.Gkh.Modules.ClaimWork.Enums;
using Bars.B4.IoC;
using Bars.Gkh.Modules.ClaimWork.Interceptors;
using Bars.Gkh.RegOperator.Entities;
using Bars.Gkh.Repositories;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Bars.Gkh.Regions.Voronezh.Interceptors
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Regions.Voronezh.Controllers;
    using Bars.Gkh.Regions.Voronezh.Entities;
    using Bars.Gkh.RegOperator.Entities.Owner;

    public class LawSuitDebtWorkSSPInterceptor : EmptyDomainInterceptor<LawSuitDebtWorkSSP>
    {
        public override IDataResult BeforeCreateAction(IDomainService<LawSuitDebtWorkSSP> service, LawSuitDebtWorkSSP entity)
        {
            if (entity.CbSize == LawsuitCollectionDebtType.FullRepaid) return this.Success();
            
            var debtor = this.GetDebtor(entity.Lawsuit);
            bool canUpdate = false;
            if (entity.CbFactInitiated == LawsuitFactInitiationType.Initiated)
            {
                var oldState = debtor.DebtorState;
                debtor.DebtorState = DebtorState.StartedEnforcement;
                debtor.DebtorStateHistory = oldState;
                canUpdate = true;
                debtor.SubContractDate = entity.CbDateInitiated;
            }

            if (canUpdate)
            {
                this.DebtorClaimWorkDomain.Update(debtor);
            }

            if (entity.Lawsuit.ResultConsideration == LawsuitResultConsideration.CourtOrderIssued && string.IsNullOrEmpty(entity.CbNumberDocument))
            {
                entity.CbDocumentType = LawsuitCollectionDebtDocumentType.CourtOrder;
                entity.CbNumberDocument = entity.Lawsuit.ConsiderationNumber;
                entity.CbDateDocument = entity.Lawsuit.ConsiderationDate;
            } 

            if (entity.CbSize == LawsuitCollectionDebtType.NotRepaiment)
            {
                decimal areaShared = entity.LawsuitOwnerInfo.AreaShare>0? entity.LawsuitOwnerInfo.AreaShare:1;

                decimal accDebt = entity.Lawsuit.DebtSum.HasValue ? entity.Lawsuit.DebtSum.Value : 0;
                if (entity.CbDebtSum == null )
                {
                    entity.CbDebtSum = 0;
                }
                if (entity.CbSumRepayment == null)
                {
                    entity.CbSumRepayment = areaShared > 0 ? accDebt * areaShared : accDebt;
                }
            }
            if (entity.CbSize == LawsuitCollectionDebtType.PartiallyRepaiment)
            {
                decimal areaShared = entity.LawsuitOwnerInfo.AreaShare > 0 ? entity.LawsuitOwnerInfo.AreaShare : 1;
                decimal accDebt = entity.Lawsuit.DebtSum.HasValue ? entity.Lawsuit.DebtSum.Value : 0;
                if (entity.CbDebtSum == null)
                {
                    entity.CbDebtSum = 0;
                }
                if (entity.CbSumRepayment == null)
                {
                    entity.CbSumRepayment = (areaShared > 0 ? accDebt * areaShared : accDebt) - entity.CbDebtSum;
                }
            }
            //RloiId
            var flatClwDomain = this.Container.Resolve<IDomainService<FlattenedClaimWork>>();
            var ownersDomain = this.Container.Resolve<IDomainService<LawsuitOwnerInfo>>();

            var ownersCount = ownersDomain.GetAll()
                .Where(x => x.Lawsuit == entity.Lawsuit)
                .Where(x => x.AreaShareDenominator != 1)
                .Count();
            if (ownersCount > 0)
            {
                var flatClw = flatClwDomain.GetAll()
                    .Where(x => x.Archived == false)
                    .Where(x => x.RloiId != null)
                    .Where(x => x.RloiId == entity.LawsuitOwnerInfo.Id).FirstOrDefault();
                if (flatClw != null)
                {
                    entity.CbNumberDocument = flatClw.CourtClaimNum;
                    entity.CbDateDocument = flatClw.CourtClaimDate;
                }

            }
            else
            {
                //entity.CbNumberDocument = entity.Lawsuit.BidNumber;
                //entity.CbDateDocument = entity.Lawsuit.BidDate;
            }
            try
            {
                Municipality munROSP = entity.Lawsuit.JurInstitution != null ? entity.Lawsuit.JurInstitution.Municipality: null;
                if (munROSP != null)
                {
                    var jurInstitutionDomain = this.Container.Resolve<IDomainService<JurInstitution>>();
                    JurInstitution rosp = jurInstitutionDomain.GetAll()
                    .Where(x => x.Municipality == munROSP && x.JurInstitutionType == JurInstitutionType.Bailiffs).FirstOrDefault();
                    if (rosp != null)
                    {
                        entity.CbStationSsp = rosp;
                    }
                }

            }
            catch
            { }

            return this.Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<LawSuitDebtWorkSSP> service, LawSuitDebtWorkSSP entity)
        {

            var debtor = this.GetDebtor(entity.Lawsuit);
            bool canUpdate = false;
            if (entity.CbFactInitiated == LawsuitFactInitiationType.Initiated)
            {
                var oldState = debtor.DebtorState;
                debtor.DebtorState = DebtorState.StartedEnforcement;
                debtor.DebtorStateHistory = oldState;
                canUpdate = true;
                debtor.SubContractDate = entity.CbDateInitiated;
            }

            if (canUpdate)
            {
                this.DebtorClaimWorkDomain.Update(debtor);
            }

            if (entity.Lawsuit.ResultConsideration == LawsuitResultConsideration.CourtOrderIssued && string.IsNullOrEmpty(entity.CbNumberDocument))
            {
                entity.CbDocumentType = LawsuitCollectionDebtDocumentType.CourtOrder;
                entity.CbNumberDocument =  entity.Lawsuit.ConsiderationNumber;
                entity.CbDateDocument = entity.Lawsuit.ConsiderationDate;
            }

            if (entity.CbSize == LawsuitCollectionDebtType.NotRepaiment)
            {
                decimal areaShared = entity.LawsuitOwnerInfo.AreaShare > 0 ? entity.LawsuitOwnerInfo.AreaShare : 1;
                decimal accDebt = entity.Lawsuit.DebtSum.HasValue ? entity.Lawsuit.DebtSum.Value : 0;
                if (entity.CbDebtSum == null)
                {
                    entity.CbDebtSum = 0;
                }
                if (entity.CbSumRepayment == null)
                {
                    entity.CbSumRepayment = areaShared > 0 ? accDebt * areaShared : accDebt;
                }
            }
            if (entity.CbSize == LawsuitCollectionDebtType.PartiallyRepaiment)
            {
                decimal areaShared = entity.LawsuitOwnerInfo.AreaShare > 0 ? entity.LawsuitOwnerInfo.AreaShare : 1;
                decimal accDebt = entity.Lawsuit.DebtSum.HasValue ? entity.Lawsuit.DebtSum.Value : 0;
                if (entity.CbDebtSum == null)
                {
                    entity.CbDebtSum = 0;
                }
                if (entity.CbSumRepayment == null)
                {
                    entity.CbSumRepayment = (areaShared > 0 ? accDebt * areaShared : accDebt) - entity.CbDebtSum;
                }
            }
            //RloiId
            if (string.IsNullOrEmpty(entity.CbNumberDocument))
            {
                var flatClwDomain = this.Container.Resolve<IDomainService<FlattenedClaimWork>>();
                var ownersDomain = this.Container.Resolve<IDomainService<LawsuitOwnerInfo>>();

                var ownersCount = ownersDomain.GetAll()
                    .Where(x => x.Lawsuit == entity.Lawsuit)
                    .Where(x => x.AreaShareDenominator != 1) 
                    .Count();
                if (ownersCount > 0)
                {
                    var flatClw = flatClwDomain.GetAll()
                        .Where(x => x.Archived == false)
                        .Where(x => x.RloiId != null)
                        .Where(x => x.RloiId == entity.LawsuitOwnerInfo.Id).FirstOrDefault();
                    if (flatClw != null)
                    {
                        entity.CbNumberDocument = flatClw.CourtClaimNum;
                        entity.CbDateDocument = flatClw.CourtClaimDate;
                    }

                }
                else
                {
                    entity.CbNumberDocument = entity.Lawsuit.BidNumber;
                    entity.CbDateDocument = entity.Lawsuit.BidDate;
                }
            }
            try
            {
                if (entity.CbStationSsp == null)
                { 
                    Municipality munROSP = entity.Lawsuit.JurInstitution != null ? entity.Lawsuit.JurInstitution.Municipality : null;
                    if (munROSP != null)
                    {
                        var jurInstitutionDomain = this.Container.Resolve<IDomainService<JurInstitution>>();
                        JurInstitution rosp = jurInstitutionDomain.GetAll()
                        .Where(x => x.Municipality == munROSP && x.JurInstitutionType == JurInstitutionType.Bailiffs).FirstOrDefault();
                        if (rosp != null)
                        {
                            entity.CbStationSsp = rosp;
                        }
                    }
                }

            }
            catch
            { }

            return this.Success();
        }

        public IDomainService<DebtorClaimWork> DebtorClaimWorkDomain { get; set; }

        private DebtorClaimWork GetDebtor(Lawsuit entity)
        {
            return this.DebtorClaimWorkDomain.Load(entity.ClaimWork.Id);
        }


    }
}
