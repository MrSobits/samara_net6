namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class PropertyOwnerDecisionWorkViewModel : BaseViewModel<PropertyOwnerDecisionWork>
    {
        public override IDataResult List(IDomainService<PropertyOwnerDecisionWork> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var propertyOwnerDecisionId = baseParams.Params.GetAs<long>("propertyOwnerDecisionId");

            var data = domainService.GetAll()
                .Where(x => x.Decision.Id == propertyOwnerDecisionId)
                .Select(x => new
                {
                    x.Id,
                    WorkName = x.Work.Name
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}