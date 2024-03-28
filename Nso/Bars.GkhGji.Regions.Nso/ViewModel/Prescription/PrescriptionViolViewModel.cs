namespace Bars.GkhGji.Regions.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class PrescriptionViolViewModel : GkhGji.ViewModel.PrescriptionViolViewModel
    {
        public override IDataResult List(IDomainService<PrescriptionViol> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            /* documentId - идентификатор предписания
             * realityObjId - идентификатор дома По которому необходимо получить нарушения */

            var documentId = baseParams.Params.GetAs<long>("documentId");
            var realityObjId = baseParams.Params.GetAs<long>("realityObjId");

            var violFeatureDict = Container.Resolve<IDomainService<ViolationFeatureGji>>().GetAll()
                .Select(x => new
                {
                    violId = x.ViolationGji.Id,
                    x.FeatureViolGji.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.violId)
                .ToDictionary(x => x.Key, x => string.Join("; ", x.Select(y => y.Name).Distinct()));

            var data = domain.GetAll()
                .Where(x => x.Document.Id == documentId)
                .WhereIf(realityObjId > 0, x => x.InspectionViolation.RealityObject.Id == realityObjId)
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
                    CodesPin = x.InspectionViolation.Violation.NormativeDocNames,
                    Municipality = x.InspectionViolation.RealityObject.Municipality.Name,
                    RealityObject = x.InspectionViolation.RealityObject.Address,
                    x.Action,
                    violationId = (long?) x.InspectionViolation.Violation.Id
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            data = data
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.RealityObject);

            var result = data.Order(loadParams)
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
                    x.CodesPin,
                    Features =
                        x.violationId.HasValue && violFeatureDict.ContainsKey(x.violationId.Value)
                            ? violFeatureDict[x.violationId.Value]
                            : string.Empty,
                })
                .ToList();

            return new ListDataResult(result, totalCount);
        }
    }
}