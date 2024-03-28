namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using B4;

    using Bars.B4.Utils;

    using Entities.Suggestion;

    public class SugTypeProblemViewModel : BaseViewModel<SugTypeProblem>
    {
        public override IDataResult List(IDomainService<SugTypeProblem> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var rubricId = baseParams.Params.GetAs<long>("rubricId");

            var data = domainService.GetAll()
                .Where(x => x.Rubric.Id == rubricId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.RequestTemplate,
                    x.ResponceTemplate
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}