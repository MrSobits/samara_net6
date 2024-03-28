namespace Bars.Gkh.ViewModel.Suggestion
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities.Suggestion;

    public class TransitionViewModel: BaseViewModel<Transition>
    {
        public override IDataResult List(IDomainService<Transition> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var rubricId = baseParams.Params.GetAs<long>("rubricId");

            var data = domainService.GetAll()
                .Where(x => x.Rubric.Id == rubricId)
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
