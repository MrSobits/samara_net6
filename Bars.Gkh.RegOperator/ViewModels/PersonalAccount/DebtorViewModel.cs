namespace Bars.Gkh.RegOperator.ViewModels.PersonalAccount
{
    using B4;
    using DomainService.PersonalAccount;
    using Entities.PersonalAccount;

    public class DebtorViewModel : BaseViewModel<Debtor>
    {
        public override IDataResult List(IDomainService<Debtor> domainService, BaseParams baseParams)
        {
            var service = Container.Resolve<IDebtorService>();

            try
            {
                int totalCount;
                var result = service.GetList(baseParams, true, out totalCount);
                return new ListDataResult(result, totalCount);
            }
            finally 
            {
                Container.Release(service);
            }
        }
    }
}