namespace Bars.GkhGji.Regions.Khakasia.ViewModel.AppealCits
{
    using System.Linq;
    using Bars.B4;
    using Bars.GkhGji.Regions.Khakasia.Entities.AppealCits;
    public class CheckTimeChangeViewModel : BaseViewModel<CheckTimeChange>
    {
        public override IDataResult List(IDomainService<CheckTimeChange> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var appealId = loadParam.Filter.GetAs<long>("appealCitizensId", 0, true);

            var data = domainService.GetAll()
                .Where(x => x.AppealCits.Id == appealId)
                .Select(x => new
                {
                    x.Id,
                    x.NewValue,
                    x.OldValue,
                    CreateDate = x.ObjectCreateDate,
                    UserName = x.User.Name
                })
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}
