namespace Bars.Gkh.Gis.ViewModel.Analysis
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities.IndicatorServiceComparison;

    public class IndicatorServiceComparisonViewModel : BaseViewModel<IndicatorServiceComparison>
    {
        public override IDataResult List(IDomainService<IndicatorServiceComparison> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            //var indicatorsDict = domainService.GetAll()
            //    .ToList()
            //    .GroupBy(x => x.Service.Id)
            //    .ToDictionary(x => x.Key,
            //        x => x.Select(y => y.GisTypeIndicator.GetEnumMeta().Display).AggregateWithSeparator(", "));

            var data = domainService.GetAll()
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    Service = x.Service,
                    ServiceName = x.Service.Name,
                    x.GisTypeIndicator,
                    TypeIndicatorName = x.GisTypeIndicator.GetEnumMeta().Display
                })
                .AsQueryable()
                .OrderIf(loadParams.Order.Length == 0, true, x => x.ServiceName)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.TypeIndicatorName)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}