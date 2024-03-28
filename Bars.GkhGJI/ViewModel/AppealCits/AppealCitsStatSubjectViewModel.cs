namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class AppealCitsStatSubjectViewModel : BaseViewModel<AppealCitsStatSubject>
    {
        public override IDataResult List(IDomainService<AppealCitsStatSubject> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var appealCitizensId = baseParams.Params.ContainsKey("appealCitizensId") ? baseParams.Params["appealCitizensId"].ToLong() : 0;
            var data = domainService.GetAll()
                .WhereIf(appealCitizensId > 0, x => x.AppealCits.Id == appealCitizensId)
                .Select(x => new
                    {
                        x.Id,
                        Subject = x.Subject.Name,
                        x.Subject.SSTUCode,
                        x.Subject.SSTUName,
                        Subsubject = x.Subsubject.Name,
                        Feature = x.Feature.Name
                    }).Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}
