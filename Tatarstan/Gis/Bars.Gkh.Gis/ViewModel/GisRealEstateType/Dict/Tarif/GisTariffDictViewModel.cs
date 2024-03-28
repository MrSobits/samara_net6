namespace Bars.Gkh.Gis.ViewModel.Dict.Tarif
{
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.Gkh.Utils;

    using Entities.Dict;
    using Gkh.Domain;

    /// <summary>
    /// Справочник тарифов
    /// </summary>
    public class GisTariffDictViewModel : BaseViewModel<GisTariffDict>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<GisTariffDict> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var isNotOrder = loadParams.Order.IsEmpty();
            return domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Municipality.Name,
                    Service = x.Service.Name,
                    Contragent = x.Contragent.Name,
                    ContragentInn = x.Contragent.Inn,
                    x.TariffKind,
                    x.ZoneCount,
                    x.TariffValue,
                    x.TariffValue1,
                    x.TariffValue2,
                    x.TariffValue3,
                    x.StartDate,
                    x.EndDate,
                    x.EiasUploadDate
                })
                .OrderIf(isNotOrder, true, x => x.Municipality)
                .OrderThenIf(isNotOrder, true, x => x.Service)
                .ToListDataResult(loadParams);
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<GisTariffDict> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var tariff = domainService.Get(id);

            if (tariff == null)
            {
                return BaseDataResult.Error("Не удалось получить нормативный параметр");
            }

            var result = DynamicDictionary.Writer(tariff).Execute();
            result.Add("UnitMeasure", tariff.Service?.UnitMeasure?.Name);

            return new BaseDataResult(result);
        }
    }
}