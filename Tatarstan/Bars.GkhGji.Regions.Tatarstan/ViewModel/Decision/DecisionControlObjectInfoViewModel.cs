namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Decision
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;

    public class DecisionControlObjectInfoViewModel : BaseViewModel<DecisionControlObjectInfo>
    {
        public override IDataResult List(IDomainService<DecisionControlObjectInfo> domain, BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAs<long>("documentId");

            return domain
                .GetAll()
                .Where(x => x.Decision.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    InspGjiRealityObject = x.InspGjiRealityObject.RealityObject.Address,
                    ControlObjectType = x.ControlObjectKind.ControlObjectType.Name,
                    ControlObjectKind = x.ControlObjectKind.Name
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
        
        public override IDataResult Get(IDomainService<DecisionControlObjectInfo> domainService, BaseParams baseParams)
        {
            var record = domainService.Get(baseParams.Params.GetAsId());

            return record != null
                ? new BaseDataResult(new
                {
                    record.Id,
                    ControlObjectKind = new { record.ControlObjectKind.Id, record.ControlObjectKind.Name },
                    InspGjiRealityObject = new { record.InspGjiRealityObject.Id, record.InspGjiRealityObject.RealityObject.Address }
                })
                : new BaseDataResult();
        }
    }
}