namespace Bars.Gkh.ViewModel.Dict
{
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Справочник услуг
    /// </summary>
    public class ServiceDictionaryViewModel : BaseViewModel<ServiceDictionary>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ServiceDictionary> domainService, BaseParams baseParams)
        {
            var typeGroupServiceDi = baseParams.Params.GetAs<TypeServiceGis>("typeGroupServiceDi", ignoreCase: true);
            var loadParams = this.GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .WhereIf(typeGroupServiceDi > 0, x => x.TypeService == typeGroupServiceDi)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.IsProvidedForAllHouseNeeds,
                    x.Measure,
                    x.Name,
                    x.TypeCommunalResource,
                    x.TypeService,
                    UnitMeasure = x.UnitMeasure.Name
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Name)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Code)
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}