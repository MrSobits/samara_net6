namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities;

    public class BuilderFeedbackViewModel : BaseViewModel<BuilderFeedback>
    {
        public override IDataResult List(IDomainService<BuilderFeedback> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var builderId = baseParams.Params.GetAs<long>("builderId");

            var data = domain.GetAll()
                .Where(x => x.Builder.Id == builderId)
                .Select(x => new
                {
                    x.Id,
                    x.Content,
                    x.DocumentName,
                    x.FeedbackDate,
                    x.OrganizationName,
                    x.File
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}