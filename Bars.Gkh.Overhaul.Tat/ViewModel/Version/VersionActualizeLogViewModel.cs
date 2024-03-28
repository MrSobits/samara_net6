namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class VersionActualizeLogViewModel : BaseViewModel<VersionActualizeLog>
    {
        public override IDataResult List(IDomainService<VersionActualizeLog> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var version = loadParams.Filter.GetAs<long>("version");

            var municipalityId = loadParams.Filter.GetAs<long>("municipalityId");

            if (municipalityId == 0)
            {
                return new BaseDataResult(false, "Не задан параметр \"Муниципальное образование\"");   
            }

           var data =
                domainService.GetAll()
                    .Where(x => x.ProgramVersion.Id == version && x.Municipality.Id == municipalityId)
                    .Select(x => new
                    {
                        x.Id,
                        x.UserName,
                        x.ActualizeType,
                        DateAction = x.DateAction.ToUniversalTime(),
                        x.CountActions,
                        x.LogFile
                    })
                    .ToArray()
                    .AsQueryable()
                    .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}