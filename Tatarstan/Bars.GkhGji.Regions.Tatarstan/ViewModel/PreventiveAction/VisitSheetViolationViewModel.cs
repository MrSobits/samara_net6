namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.PreventiveAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    /// <summary>
    /// View-модель для <see cref="VisitSheetViolationInfo"/>
    /// </summary>
    public class VisitSheetViolationViewModel : BaseViewModel<VisitSheetViolation>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<VisitSheetViolation> domainService, BaseParams baseParams)
        {
            var objectId = baseParams.Params.GetAsId("objectId");

            return domainService.GetAll()
                .Where(x => x.ViolationInfo.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    ViolationId = x.Violation.Id,
                    x.Violation.NormativeDocNames,
                    x.Violation.Name,
                    x.IsThreatToLegalProtectedValues
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}