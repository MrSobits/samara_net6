namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class PrescriptionOfficialReportViolationViewModel : BaseViewModel<PrescriptionOfficialReportViolation>
    {
        public override IDataResult List(IDomainService<PrescriptionOfficialReportViolation> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("OfficialReport", 0L);
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);

            var data = domain.GetAll()
             .Where(x => x.PrescriptionOfficialReport.Id == id)
            .Select(x => new
            {
                x.Id,
               // ViolationGjiPin = x.PrescriptionViol.InspectionViolation.Violation.CodePin,
                ViolationGji = x.PrescriptionViol.InspectionViolation.Violation.Name,
                ViolationGjiPin = x.PrescriptionViol.InspectionViolation.Violation.NormativeDocNames,
                x.PrescriptionViol.Action,
                Description = x.PrescriptionViol.Description ?? x.PrescriptionViol.InspectionViolation.Description,
                DatePlanRemoval = x.PrescriptionViol.DatePlanRemoval.HasValue ? x.PrescriptionViol.DatePlanRemoval:x.PrescriptionViol.InspectionViolation.DatePlanRemoval,
                x.PrescriptionViol.DatePlanExtension,
                x.PrescriptionViol.NotificationDate,
                x.PrescriptionViol.InspectionViolation.DateFactRemoval,
            })
            .AsQueryable()
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}