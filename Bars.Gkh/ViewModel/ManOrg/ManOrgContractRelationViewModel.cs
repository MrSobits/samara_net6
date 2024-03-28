namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;

    public class ManOrgContractRelationViewModel : BaseViewModel<ManOrgContractRelation>
    {
        public override IDataResult List(IDomainService<ManOrgContractRelation> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var contractId = baseParams.Params.GetAs<long>("contractId");

            var data = domain.GetAll()
                .Where(x => x.Parent.Id == contractId)
                .Select(x => new
                {
                    x.Children.Id,
                    ManagingOrganization = x.Children.ManagingOrganization.Contragent.Name,
                    x.Children.StartDate,
                    x.Children.EndDate
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            data = data
                .OrderIf(loadParams.Order.Length == 0, true, x => x.StartDate);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}