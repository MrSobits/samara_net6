namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActivityTsjProtocolViewModel : BaseViewModel<ActivityTsjProtocol>
    {
        public override IDataResult List(IDomainService<ActivityTsjProtocol> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var activityTsjId = baseParams.Params.ContainsKey("activityTSJ")
                       ? baseParams.Params["activityTSJ"].ToLong()
                       : 0;

            var data = domainService.GetAll()
                .Where(x => x.ActivityTsj.Id == activityTsjId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNum,
                    x.DocumentDate,
                    x.VotesDate,
                    KindProtocolTsjName = x.KindProtocolTsj.Name,
                    x.FileBulletin,
                    x.File
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}