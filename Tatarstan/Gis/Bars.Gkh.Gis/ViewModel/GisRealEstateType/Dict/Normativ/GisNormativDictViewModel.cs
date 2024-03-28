namespace Bars.Gkh.Gis.ViewModel.Dict.Normativ
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities.Dict;
    using Gkh.Domain;

    public class GisNormativDictViewModel : BaseViewModel<GisNormativDict>
    {
        public override IDataResult List(IDomainService<GisNormativDict> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Municipality.Name,
                    Service = x.Service.Name,
                    x.Value,
                    x.Measure,
                    x.DateStart,
                    x.DateEnd
                })
                .AsQueryable()
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Service)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<GisNormativDict> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var normativ = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            if (normativ == null)
            {
                return new BaseDataResult(false, "Не удалось получить нормативный параметр");
            }

            var obj = new
            {
                normativ.Id,
                normativ.Municipality,
                Service = normativ.Service,
                normativ.Value,
                normativ.Measure,
                normativ.DateStart,
                normativ.DateEnd,
                normativ.Description,
                normativ.DocumentName,
                normativ.DocumentFile
            };

            return new BaseDataResult(obj);
        }
    }
}