namespace Bars.Gkh.RegOperator.ViewModels.PersonalAccount
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    public class PersonalAccountBanRecalcViewModel : BaseViewModel<PersonalAccountBanRecalc>
    {
        public override IDataResult List(IDomainService<PersonalAccountBanRecalc> domainService, BaseParams baseParams)
        {
            var accId = baseParams.Params.GetAs("accId", 0L);
            if (accId == 0)
            {
                return BaseDataResult.Error("Не передан идентификатор ЛС");
            }

            var loadParam = this.GetLoadParam(baseParams);
            var result =
                domainService.GetAll()
                    .Where(x => x.PersonalAccount.Id == accId)
                    .Filter(loadParam, this.Container);

            return new ListDataResult(result.Order(loadParam).Paging(loadParam), result.Count());
        }
    }
}