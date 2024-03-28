namespace Bars.Gkh.RegOperator.ViewModels.PersonalAccount
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities.PersonalAccount;

    public class PersonalAccountChangeViewModel : BaseViewModel<PersonalAccountChange>
    {
        public override IDataResult List(IDomainService<PersonalAccountChange> domainService, BaseParams baseParams)
        {
            var accId = baseParams.Params.GetAs<long>("accId", ignoreCase: true);
            var loadParams = baseParams.GetLoadParam();

            var data = domainService.GetAll()
                .WhereIf(accId > 0, x => x.PersonalAccount.Id == accId)
                .Filter(loadParams, Container)
                .Order(loadParams);

            return new ListDataResult(data.Paging(loadParams), data.Count());
        }
    }
}