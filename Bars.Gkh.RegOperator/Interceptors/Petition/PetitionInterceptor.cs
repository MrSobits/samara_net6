namespace Bars.Gkh.RegOperator.Interceptors
{
	using Bars.B4;
	using Bars.Gkh.Modules.ClaimWork.Entities;
	using Bars.Gkh.RegOperator.DomainService.Petition;
	using Bars.Gkh.RegOperator.Entities;

	public class PetitionInterceptor : EmptyDomainInterceptor<Petition>
    {
		//public IPetitionService PetitionService { get; set; }
		public IDomainService<DebtorClaimWork> DebtorClaimWorkDomain { get; set; }

	    public override IDataResult BeforeUpdateAction(IDomainService<Petition> service, Petition entity)
        {
			if (entity.DocumentDate != null)
			{
				var debtorClaimWork = this.DebtorClaimWorkDomain.Get(entity.ClaimWork.Id);
				if (debtorClaimWork != null)
				{
					/*var chargeDebt = this.PetitionService.GetChargeDebt(debtorClaimWork.PersonalAccount, entity.DocumentDate.Value);
					entity.DebtSum = chargeDebt;*/
				}
			}

			return base.BeforeUpdateAction(service, entity);
        }
    }
}