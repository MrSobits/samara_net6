namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class PrescriptionCancelViolReferenceViewModel : BaseViewModel<PrescriptionCancelViolReference>
    {
        public override IDataResult List(IDomainService<PrescriptionCancelViolReference> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var prescriptonCancelId = loadParams.Filter.GetAs("prescriptonCancelId", 0L);

            var query = domainService.GetAll();

            var data = query
                .WhereIf(prescriptonCancelId > 0, x => x.PrescriptionCancel.Id == prescriptonCancelId)
                .Select(x => new
                        {
                            x.Id,
                            ViolationGjiPin = x.InspectionViol.InspectionViolation.Violation.CodePin,
                            ViolationGji = x.InspectionViol.InspectionViolation.Violation.Name,
                            x.InspectionViol.InspectionViolation.DatePlanRemoval,
                            x.NewDatePlanRemoval
                        })
                        .Filter(loadParams, Container);

            int totalCount = data.Any() ? data.Count() : 0;

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}
