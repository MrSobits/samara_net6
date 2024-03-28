using Bars.Gkh.Gasu.DomainService;

namespace Bars.Gkh.Gasu.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;

    public class GasuIndicatorValueViewModel : BaseViewModel<GasuIndicatorValue>
    {

        public IGasuIndicatorService gasuIndicator { get; set; }
        public override IDataResult List(IDomainService<GasuIndicatorValue> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var modules = gasuIndicator.GetAvailableModulesEbir();

            var year = loadParam.Filter.GetAs("year", 0);
            var month = loadParam.Filter.GetAs("month", 0);

            var data = domainService.GetAll()
                .Where(x => modules.Contains(x.GasuIndicator.EbirModule)) // получаем тольк оте записи которые должен видеть пользователь по настройке прав на модули
                .Where(x => x.Month == month && x.Year == year)
                .Select(x => new
                {
                    x.Id,
                    x.Month,
                    x.Year,
                    x.Value,
                    x.PeriodStart,
                    Municipality = x.Municipality.Name,
                    GasuIndicator = x.GasuIndicator.Id,
                    GasuIndicatorName = x.GasuIndicator.Name,
                    x.GasuIndicator.Periodicity,
                    UnitMeasure = x.GasuIndicator.UnitMeasure.ShortName,
                    x.GasuIndicator.EbirModule
                })
                .Filter(loadParam, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).OrderBy(x => x.Municipality).Paging(loadParam).ToList(), totalCount);
        }
    }
}