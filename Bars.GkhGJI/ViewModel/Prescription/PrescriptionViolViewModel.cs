namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class PrescriptionViolViewModel : BaseViewModel<PrescriptionViol>
    {
        public override IDataResult List(IDomainService<PrescriptionViol> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            /* documentId - идентификаторы предписаний по которым нужно вытащить нарушения 
             * realityObjId - идентификатор дома По которому необходимо получить нарушения */

            var documentId = baseParams.Params.GetAs("documentId", 0L);
            var roExist = baseParams.Params.ContainsKey("realityObjId");
            var realityObjId = baseParams.Params.GetAs("realityObjId", 0L);
            var documentIds = baseParams.Params.GetAs("documentIds", new long[0]);

            var query = domain.GetAll();

            if (documentIds.Any())
            {
                query = query.Where(x => documentIds.Contains(x.Document.Id));
            }
            else
            {
                query = query.Where(x => documentId == x.Document.Id)
                             .WhereIf(roExist && realityObjId > 0, x => x.InspectionViolation.RealityObject.Id == realityObjId)
                    // получаем нарушения по дому
                             .WhereIf(roExist && realityObjId == 0, x => x.InspectionViolation.RealityObject == null);
                    // чтобы получить нарушения котоыре забиты не по дому
            }

            var data = query
                        .Select(x => new
                        {
                            x.Id,
                            InspectionViolationId = x.InspectionViolation.Id,
                            Municipality = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Municipality.Name : null,
                            RealityObject = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Address : null,
                            RealityObjectId = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Id : 0,
                            DatePlanRemoval = x.DatePlanRemoval ?? x.InspectionViolation.DatePlanRemoval,
                            x.DatePlanExtension,
                            x.NotificationDate,
                            x.InspectionViolation.DateFactRemoval,
                            x.InspectionViolation.SumAmountWorkRemoval,
                            ViolationGji = x.InspectionViolation.Violation.Name,
                            ViolationGjiPin = x.InspectionViolation.Violation.CodePin,
                            x.Action,
                            x.Description,
                            x.InspectionViolation.DateCancel
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