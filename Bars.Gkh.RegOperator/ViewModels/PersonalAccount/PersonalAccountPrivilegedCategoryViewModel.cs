namespace Bars.Gkh.RegOperator.ViewModels.PersonalAccount
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    /// <summary>
    /// Вьюмодель для PersonalAccountPrivilegedCategory
    /// </summary>
    public class PersonalAccountPrivilegedCategoryViewModel : BaseViewModel<PersonalAccountPrivilegedCategory>
    {
        ///<inheritdoc />
        public override IDataResult List(IDomainService<PersonalAccountPrivilegedCategory> domainService, BaseParams baseParams)
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
                    .Select(x => new { x.Id, x.DateFrom, x.DateTo, x.PrivilegedCategory.Name, x.PrivilegedCategory.Percent })
                    .Filter(loadParam, this.Container);

            return new ListDataResult(result.Order(loadParam).Paging(loadParam), result.Count());
        }
    }
}