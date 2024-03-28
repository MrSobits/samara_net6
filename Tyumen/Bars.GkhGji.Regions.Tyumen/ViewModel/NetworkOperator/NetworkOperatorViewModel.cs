namespace Bars.GkhGji.Regions.Tyumen.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tyumen.Entities;

    public class NetworkOperatorViewModel : BaseViewModel<NetworkOperator>
    {
        public override IDataResult List(IDomainService<NetworkOperator> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Contragent.Name,
                    x.Contragent.Inn,
                    x.Contragent.Kpp,
                    x.Description,
                    x.Contragent
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}