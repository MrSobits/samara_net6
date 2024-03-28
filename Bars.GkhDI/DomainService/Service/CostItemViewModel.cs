namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;

    using Entities;

    public class CostItemViewModel : BaseViewModel<CostItem>
    {
        public override IDataResult List(IDomainService<CostItem> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var baseServiceId = baseParams.Params.GetAs<long>("baseServiceId");

            var data = domainService.GetAll()
                .Where(x => x.BaseService.Id == baseServiceId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.BaseService,
                    x.Count,
                    x.Sum,
                    x.Cost 
                }).Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}