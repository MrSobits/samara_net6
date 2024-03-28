namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using Bars.B4;
    using Entities;

    public class TaxSystemViewModel : BaseViewModel<TaxSystem>
    {
        public override IDataResult List(IDomainService<TaxSystem> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.ShortName
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}