namespace Bars.GkhDi.ViewModels
{
    using System.Linq;
    using B4;

    using Entities;

    public class DisclosureInfoEmptyFieldsViewModel : BaseViewModel<DisclosureInfoEmptyFields>
    {
        public override IDataResult List(IDomainService<DisclosureInfoEmptyFields> domainService, BaseParams baseParams)
        {
            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");
            if (disclosureInfoId == 0)
            {
                return BaseDataResult.Error("Объект ManOrg не найден");
            }

            var loadParams = this.GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Where(x => x.ManOrg.Id == disclosureInfoId)
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}
