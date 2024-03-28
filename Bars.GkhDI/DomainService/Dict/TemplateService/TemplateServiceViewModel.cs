namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhDi.Enums;

    using Entities;

    public class TemplateServiceViewModel : BaseViewModel<TemplateService>
    {
        public override IDataResult List(IDomainService<TemplateService> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var kindTemplateService = baseParams.Params.GetAs<long>("kindTemplateService");
            var isTemplateService = baseParams.Params.GetAs("isTemplateService", false);
            var typeGroupServiceDi = baseParams.Params.GetAs<long>("typeGroupServiceDi");

            var data = domainService
                .GetAll()
                .WhereIf(isTemplateService, x => x.KindServiceDi == kindTemplateService.To<KindServiceDi>())
                .WhereIf(typeGroupServiceDi != 0L, x => x.TypeGroupServiceDi == typeGroupServiceDi.To<TypeGroupServiceDi>())
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    x.Characteristic,
                    UnitMeasureName = x.UnitMeasure.Name,
                    x.UnitMeasure,
                    x.TypeGroupServiceDi,
                    x.KindServiceDi,
                    x.CommunalResourceType,
                    x.HousingResourceType,
                    x.Changeable,
                    x.IsMandatory,
                    x.IsConsiderInCalc, 
                    x.ActualYearStart, 
                    x.ActualYearEnd
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}