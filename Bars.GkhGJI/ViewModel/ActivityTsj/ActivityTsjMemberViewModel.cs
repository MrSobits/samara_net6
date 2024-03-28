namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActivityTsjMemberViewModel : BaseViewModel<ActivityTsjMember>
    {
        public override IDataResult List(IDomainService<ActivityTsjMember> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var activityTsjId = baseParams.Params.ContainsKey("activityTSJ")
                       ? baseParams.Params["activityTSJ"].ToInt()
                       : 0;

            var data = domainService.GetAll()
                .Where(x => x.ActivityTsj.Id == activityTsjId)
                .Select(x => new
                {
                    x.Id,
                    x.Year,
                    x.File,
                    x.State                    
                })
                .OrderByDescending(x => x.Year)
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}