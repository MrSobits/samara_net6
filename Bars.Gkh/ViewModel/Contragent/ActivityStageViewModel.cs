namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class ActivityStageViewModel : BaseViewModel<ActivityStage>
    {
        public override IDataResult List(IDomainService<ActivityStage> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var entityId = baseParams.Params.GetAs<long>("entityId");
            var type = baseParams.Params.GetAs<ActivityStageOwner>("entityType");

            var data = domain.GetAll()
                .Where(x => x.EntityType == type && x.EntityId == entityId)
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}