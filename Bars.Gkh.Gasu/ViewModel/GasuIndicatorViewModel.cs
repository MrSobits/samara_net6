namespace Bars.Gkh.Gasu.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;

    public class GasuIndicatorViewModel : BaseViewModel<GasuIndicator>
    {
        public override IDataResult List(IDomainService<GasuIndicator> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            
            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    x.Periodicity,
                    x.EbirModule,
                    UnitMeasure = x.UnitMeasure.ShortName
                })
                .Filter(loadParam, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}