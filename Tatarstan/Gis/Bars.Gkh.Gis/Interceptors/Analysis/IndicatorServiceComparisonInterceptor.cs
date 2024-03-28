namespace Bars.Gkh.Gis.Interceptors.Analysis
{
    using System.Linq;
    using B4;
    using Entities.IndicatorServiceComparison;

    public class IndicatorServiceComparisonInterceptor : EmptyDomainInterceptor<IndicatorServiceComparison>
    {
        public override IDataResult BeforeCreateAction(IDomainService<IndicatorServiceComparison> service, IndicatorServiceComparison entity)
        {
            var exists = service.GetAll()
                .Where(x => x.Service.Id == entity.Service.Id)
                .Select(x => x.GisTypeIndicator)
                .ToArray();

            return exists.Contains(entity.GisTypeIndicator) 
                ? Failure("Данный индикатор для данной услуги уже существует") 
                : Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<IndicatorServiceComparison> service, IndicatorServiceComparison entity)
        {
            var exists = service.GetAll()
                .Where(x => x.Service.Id == entity.Service.Id)
                .Select(x => x.GisTypeIndicator)
                .ToArray();

            return exists.Contains(entity.GisTypeIndicator) 
                ? Failure("Данный индикатор для данной услуги уже существует") 
                : Success();
        }
    }
}