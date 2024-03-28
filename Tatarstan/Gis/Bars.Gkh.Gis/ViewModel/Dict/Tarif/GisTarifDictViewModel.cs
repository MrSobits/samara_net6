namespace Bars.Gkh.Gis.ViewModel.Dict.Tarif
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities.Dict;
    using Gkh.Domain;

    public class GisTarifDictViewModel : BaseViewModel<GisTarifDict>
    {
        public override IDataResult List(IDomainService<GisTarifDict> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Municipality.Name,
                    Service = x.Service.Name,
                    x.Service.TypeService,
                    Contragent = x.Contragent.Name,
                    x.Value,
                    x.Service.Measure
                })
                .AsQueryable()
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Service)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<GisTarifDict> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var tarif = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            if (tarif == null)
            {
                return new BaseDataResult(false, "Не удалось получить нормативный параметр");
            }

            var obj = new
            {
                tarif.Id,
                tarif.Municipality,
                Service = tarif.Service,
                tarif.Contragent,
                tarif.DateStart,
                tarif.DateEnd,
                tarif.Value,
                tarif.PurchasingVolume,
                tarif.PurchasingCost,
                tarif.Requisites
            };

            return new BaseDataResult(obj);
        }
    }
}