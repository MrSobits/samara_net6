namespace Bars.GkhGji.Regions.Saha.ViewModel
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;

    using Entities;

    public class AppealCitsExecutantViewModel : BaseViewModel<AppealCitsExecutant>
    {
        public override IDataResult List(IDomainService<AppealCitsExecutant> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");

            var data = domainService.GetAll()
                .WhereIf(appealCitizensId > 0, x => x.AppealCits.Id == appealCitizensId)
                .Select(x => new
                {
                    x.Id,
                    Author = x.Author.Fio,
                    Executant = x.Executant.Fio,
                    x.PerformanceDate,
                    IsResponsible = x.IsResponsible ? "Да" : "Нет",
                    x.State
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<AppealCitsExecutant> domain, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domain.GetAll().FirstOrDefault(x => x.Id == id);

            if (obj != null)
            {

                var executantZjiName = string.Empty;
                var zonalInspectionInspectorDomain = Container.Resolve<IDomainService<ZonalInspectionInspector>>();

                using (Container.Using(zonalInspectionInspectorDomain))
                {
                    var zjiNames = zonalInspectionInspectorDomain.GetAll()
                        .Where(x => x.Inspector.Id == obj.Executant.Id)
                        .Select(x => x.ZonalInspection.Name)
                        .ToList();

                    executantZjiName = zjiNames.Aggregate("", (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));
                }

                return new BaseDataResult(
                new
                {
                    obj.Id,
                    obj.AppealCits,
                    obj.Author,
                    obj.Executant,
                    obj.IsResponsible,
                    obj.OrderDate,
                    obj.PerformanceDate,
                    obj.Description,
                    obj.State,
                    ExecutantZji = executantZjiName
                });
            }

            return new BaseDataResult();
        }
    }
}