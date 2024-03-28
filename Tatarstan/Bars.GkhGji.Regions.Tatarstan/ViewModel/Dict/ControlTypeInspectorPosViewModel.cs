namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class ControlTypeInspectorPosViewModel : BaseViewModel<ControlTypeInspectorPositions>
    {
        public override IDataResult List(IDomainService<ControlTypeInspectorPositions> domainService, BaseParams baseParams)
        {
            var controlTypeId = baseParams.Params.GetAsId("controlTypeId");

            return domainService.GetAll()
                .Where(x => x.ControlType.Id == controlTypeId)
                .Select(x => new
                {
                    x.Id,
                    x.ControlType,
                    InspectorPosition = x.InspectorPosition != null ? new { x.InspectorPosition.Id, x.InspectorPosition.Name} : null,
                    x.IsIssuer,
                    x.IsMember,
                    x.ErvkId
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}