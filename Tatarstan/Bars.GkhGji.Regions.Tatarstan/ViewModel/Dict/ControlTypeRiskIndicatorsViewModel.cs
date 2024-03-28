namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class ControlTypeRiskIndicatorsViewModel : BaseViewModel<ControlTypeRiskIndicators>
    {
        public override IDataResult List(IDomainService<ControlTypeRiskIndicators> domainService, BaseParams baseParams)
        {
            var controlTypeId = baseParams.Params.GetAsId("controlTypeId");

            return domainService.GetAll()
                .Where(x => x.ControlType.Id == controlTypeId)
                .Select(x => new
                {
                    x.Id,
                    x.ControlType,
                    x.Name,
                    x.ErvkId
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}