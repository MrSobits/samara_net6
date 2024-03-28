namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class VersionActualizeLogViewModel : BaseViewModel<VersionActualizeLog>
    {
        public override IDataResult List(IDomainService<VersionActualizeLog> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var version = loadParams.Filter.GetAs("version", 0L);

            if (version == 0)
            {
                return new BaseDataResult(false, "Не задана Верси");   
            }

            var data =
                domainService.GetAll()
                    .Where(x => x.ProgramVersion.Id == version)
                    .Select(x => new
                    {
                        x.Id,
                        x.UserName,
                        x.ActualizeType,
                        DateAction = x.DateAction.ToUniversalTime(),
                        x.CountActions,
                        x.LogFile
                    })
                    .ToList()
                    .AsQueryable()
                    .Filter(loadParams, Container);
                    
            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}