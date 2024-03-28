namespace Bars.GkhGji.ViewModel
{
    using B4;
    using B4.Utils;
    using System.Linq;
    using Entities;

    public class InspectionViolStageViewModel : BaseViewModel<InspectionGjiViolStage>
    {
        public override IDataResult List(IDomainService<InspectionGjiViolStage> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            /* documentId - идентификатор предписания
             * realityObjId - идентификатор дома По которому необходимо получить нарушения */

            var documentId = baseParams.Params.GetAs("documentId", 0L);

            var allViols = !baseParams.Params.ContainsKey("realityObjId");
            var realityObjId = baseParams.Params.GetAs("realityObjId", 0L);

            var data = domain.GetAll()
                .Where(x => x.Document.Id == documentId)
                .WhereIf(!allViols && realityObjId > 0, x => x.InspectionViolation.RealityObject.Id == realityObjId) // поулчаем нарушения по дому
                .WhereIf(!allViols && realityObjId == 0, x => x.InspectionViolation.RealityObject == null) // чтобы получить нарушения котоыре забиты не по дому
                .Select(x => new
                {
                    x.Id,
                    InspectionViolationId = x.InspectionViolation.Id,
                    Municipality = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Municipality.Name : null,
                    RealityObject = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Address : null,
                    RealityObjectId = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Id : 0,
                    x.InspectionViolation.DatePlanRemoval,
                    x.InspectionViolation.DateFactRemoval,
                    x.InspectionViolation.Description,
                    ViolationGji = x.InspectionViolation.Violation.Name,
                    ViolationGjiPin = x.InspectionViolation.Violation.CodePin,
                    x.InspectionViolation.Action
                })
                .Filter(loadParams, Container);

            int totalCount = data.Any() ? data.Count() : 0;

            data = data
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.RealityObject);

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}