namespace Bars.GkhGji.Regions.Samara.ViewModel
{
    using System.Linq;

    using B4;

    using Entities;

    public class AppealCitsTesterViewModel : BaseViewModel<AppealCitsTester>
    {
        public override IDataResult List(IDomainService<AppealCitsTester> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var appealCitizensId = loadParams.Filter.GetAs("appealCitizensId", 0L);

            var data = domainService.GetAll()
                .Where(x => x.AppealCits.Id == appealCitizensId)
                .Select(x => new
                    {
                        x.Id,
                        Tester = x.Tester.Fio 
                    })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}