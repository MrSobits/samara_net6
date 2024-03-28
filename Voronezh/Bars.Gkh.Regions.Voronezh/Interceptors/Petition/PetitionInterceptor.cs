namespace Bars.Gkh.Regions.Voronezh.Interceptors
{
	using Bars.B4;
	using Bars.Gkh.Modules.ClaimWork.Entities;
	using Bars.Gkh.RegOperator.DomainService.Petition;
	using Bars.Gkh.RegOperator.Entities;
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

	using Bars.B4.DataAccess;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    public class PetitionInterceptor : EmptyDomainInterceptor<Petition>
    {
		public IDomainService<DebtorClaimWork> DebtorClaimWorkDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<Petition> service, Petition entity)
        {
            bool canUpdate = false;
            var lawsuit = this.Container.ResolveDomain<Lawsuit>().GetAll().FirstOrDefault(x => x.ClaimWork.Id ==  entity.ClaimWork.Id);
            if (lawsuit != null)
            {
                entity.DebtSum = lawsuit.DebtSum;
                entity.DebtBaseTariffSum = lawsuit.DebtBaseTariffSum;
                entity.Duty = lawsuit.Duty * 2;
             
            }
            try
            {
                var debtor = this.GetDebtor(entity);
                if (debtor.DebtorState != DebtorState.PaidDebt)
                {
                    var oldState = debtor.DebtorState;
                    debtor.DebtorState = DebtorState.PetitionFormed;
                    debtor.DebtorStateHistory = oldState;
                    canUpdate = true;
                }
                if (canUpdate)
                {
                    DebtorClaimWorkDomain.Update(debtor);
                }

            }
            catch
            {
                
            }
        
	        return base.BeforeCreateAction(service, entity);
            // //Автогенерация номера заявления
            // var stringBidNumberList = service.GetAll().Where(x => x.BidNumber != null).Select(x => x.BidNumber).ToList();
            // //var intBidNumberList = new List<int>();
            // long autoGenNum = 0;
            // foreach (string stringBidNumber in stringBidNumberList)
            // {
            //     int intBid;
            //     string tempStringBidNumber = Regex.Replace(stringBidNumber, @"(\D*)$",""); //Срезаем хвост из символов
            //     Int32.TryParse(tempStringBidNumber, out intBid);
            //     if (intBid > autoGenNum)
            //     {
            //         autoGenNum = intBid;
            //     }
            // }
            // autoGenNum++;
            // string resultBidNum = autoGenNum.ToString()+"/и";
            // entity.BidNumber = resultBidNum;
            // entity.BidDate = DateTime.Today;
            return this.Success();
        }
        public override IDataResult BeforeUpdateAction(IDomainService<Petition> service, Petition entity)
        {
            bool canUpdate = false;
            try
            {
                var debtor = this.GetDebtor(entity);
                if (debtor.DebtorState != DebtorState.PaidDebt)
                {
                    if (entity.ResultConsideration == LawsuitResultConsideration.NotSet)
                    {
                        var oldState = debtor.DebtorState;
                        debtor.DebtorState = DebtorState.PetitionFormed;
                        debtor.DebtorStateHistory = oldState;
                        canUpdate = true;
                    }
                    else if (entity.ResultConsideration == LawsuitResultConsideration.Satisfied || entity.ResultConsideration == LawsuitResultConsideration.PartiallySatisfied)
                    {
                        if (entity.ConsiderationDate.HasValue && entity.ConsiderationDate.Value <= DateTime.Now.AddMonths(-2))
                        {
                            var oldState = debtor.DebtorState;
                            debtor.DebtorState = DebtorState.ROSPStartRequired;
                            debtor.DebtorStateHistory = oldState;
                            canUpdate = true;
                        }
                    }
                }
                if (canUpdate)
                {
                    DebtorClaimWorkDomain.Update(debtor);
                }

            }
            catch
            {

            }
           
            // //Автогенерация номера заявления
            // var stringBidNumberList = service.GetAll().Where(x => x.BidNumber != null).Select(x => x.BidNumber).ToList();
            // //var intBidNumberList = new List<int>();
            // long autoGenNum = 0;
            // foreach (string stringBidNumber in stringBidNumberList)
            // {
            //     int intBid;
            //     string tempStringBidNumber = Regex.Replace(stringBidNumber, @"(\D*)$",""); //Срезаем хвост из символов
            //     Int32.TryParse(tempStringBidNumber, out intBid);
            //     if (intBid > autoGenNum)
            //     {
            //         autoGenNum = intBid;
            //     }
            // }
            // autoGenNum++;
            // string resultBidNum = autoGenNum.ToString()+"/и";
            // entity.BidNumber = resultBidNum;
            // entity.BidDate = DateTime.Today;
            return this.Success();
        }

        private DebtorClaimWork GetDebtor(Petition entity)
        {
            return this.DebtorClaimWorkDomain.Load(entity.ClaimWork.Id);
        }
    }
}