namespace Bars.GkhGji.Regions.Tula.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tula.Entities;

    public class PrescriptionViolViewModel : GkhGji.ViewModel.PrescriptionViolViewModel
    {
        public override IDataResult List(IDomainService<PrescriptionViol> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            /* documentId - идентификатор предписания
             * realityObjId - идентификатор дома По которому необходимо получить нарушения */

            var documentId = baseParams.Params.GetAs<long>("documentId");
            var realityObjId = baseParams.Params.GetAs<long>("realityObjId");

            var serviceInspectionGjiViolWording = Container.Resolve<IDomainService<InspectionGjiViolWording>>();

            var prescriptionViolationQuery = domain.GetAll()
                .Where(x => x.Document.Id == documentId)
                .WhereIf(realityObjId > 0, x => x.InspectionViolation.RealityObject.Id == realityObjId)
                .WhereIf(realityObjId == 0, x => x.InspectionViolation.RealityObject == null);

            var violationWordingsDict = serviceInspectionGjiViolWording.GetAll()
                .Where(x => prescriptionViolationQuery.Select(y => y.InspectionViolation.Id).Contains(x.InspectionViolation.Id))
                .Select(x => new { violId = x.InspectionViolation.Id, x.Wording })
                .AsEnumerable()
                .GroupBy(x => x.violId)
                .ToDictionary(x => x.Key, x => string.Join("; ", x.Select(y => y.Wording)));

            var data = prescriptionViolationQuery
                .Select(x => new
                {
                    x.Id,
                    InspectionViolationId = x.InspectionViolation.Id,
                    RealityObjectId = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Id : 0,
                    x.DatePlanRemoval,
                    x.InspectionViolation.DateFactRemoval,
                    x.InspectionViolation.SumAmountWorkRemoval,
                    ViolationGji = x.InspectionViolation.Violation.Name,
                    ViolationGjiPin = x.InspectionViolation.Violation.CodePin,
                    NormDocNum = x.InspectionViolation.Violation.NormativeDocNames,
                    Municipality = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Municipality.Name : null,
                    RealityObject = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Address : null,
                    x.Action
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.RealityObject)
                .AsEnumerable()
                .Select(x => new
                    {
                        x.Id,
                        x.InspectionViolationId,
                        x.RealityObjectId,
                        x.DatePlanRemoval,
                        x.DateFactRemoval,
                        x.SumAmountWorkRemoval,
                        x.ViolationGji,
                        x.ViolationGjiPin,
                        x.Municipality,
                        x.RealityObject,
                        x.Action,
                        ViolationWording = violationWordingsDict.ContainsKey(x.InspectionViolationId) ? violationWordingsDict[x.InspectionViolationId] : string.Empty,
                        x.NormDocNum
                    })
                .AsQueryable()
                .Filter(loadParams, this.Container);
            
            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}