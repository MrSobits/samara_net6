namespace Bars.Gkh.RegOperator.ViewModels.PersonalAccount
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities.Owner;

	public class PersonalAccountOwnerInformationViewModel : BaseViewModel<PersonalAccountOwnerInformation>
    {
        public override IDataResult List(IDomainService<PersonalAccountOwnerInformation> domainService, BaseParams baseParams)
        {
            var accId = baseParams.Params.GetAs("accId", 0L);
            if (accId == 0)
            {
                return BaseDataResult.Error("Не передан идентификатор ЛС");
            }

            var loadParam = GetLoadParam(baseParams);
	        var query = domainService.GetAll()
		        .Where(x => x.BasePersonalAccount.Id == accId)
		        .Select(
			        x => new
			        {
				        x.Id,
				        x.DocumentNumber,
				        x.AreaShare,
				        x.StartDate,
				        x.EndDate,
				        x.Owner,
				        OwnerName = x.Owner.Name,
                        x.File
			        })
		        .Filter(loadParam, Container);

            return new ListDataResult(query.Order(loadParam).Paging(loadParam), query.Count());
        }
    }
}