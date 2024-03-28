namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.DataResult;
    using Entities;

    public class ShareFinancingCeoViewModel : BaseViewModel<ShareFinancingCeo>
    {
        public override IDataResult List(IDomainService<ShareFinancingCeo> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var queryable = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Share,
                    CommonEstateObject = x.CommonEstateObject.Name
                })
                .Filter(loadParam, Container);

            var summary = queryable.AsEnumerable().Sum(x => x.Share);

            return new ListSummaryResult(queryable.Order(loadParam).Paging(loadParam).ToList(), queryable.Count(), new { Share = summary });
        }
    }
}