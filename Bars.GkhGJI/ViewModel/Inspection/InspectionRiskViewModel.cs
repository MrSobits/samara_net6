namespace Bars.GkhGji.ViewModel
{
    using B4;

    using System.Linq;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;

    using Entities;

    /// <inheritdoc />
    public class InspectionRiskViewModel : BaseViewModel<InspectionRisk>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<InspectionRisk> domain, BaseParams baseParams)
        {
            var inspectionId = baseParams.Params.GetAsId("inspectionId");

            return domain.GetAll()
                .Where(x => x.Inspection.Id == inspectionId)
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}