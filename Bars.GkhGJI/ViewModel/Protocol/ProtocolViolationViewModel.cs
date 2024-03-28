namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;

    public class ProtocolViolationViewModel : BaseViewModel<ProtocolViolation>
    {
        public override IDataResult List(IDomainService<ProtocolViolation> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            /*
             * Тут могут быть следующие параметры
             * documentId - идентификатор протокола
             * realityObjId - идентификатор дома
             */
            var documentId = baseParams.Params.GetAs("documentId", 0L);
            var realityObjId = baseParams.Params.GetAs("realityObjId", 0L);

            var data = domainService.GetAll()
                .Where(x => x.Document.Id == documentId)
                .WhereIf(realityObjId > 0, x => x.InspectionViolation.RealityObject.Id == realityObjId) // поулчаем нарушения по дому
                .WhereIf(realityObjId == 0, x => x.InspectionViolation.RealityObject == null) // чтобы получить нарушения котоыре забиты не по дому
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
                        ViolationGji = x.InspectionViolation.Violation.Name,
                        ViolationGjiPin = x.InspectionViolation.Violation.CodePin,
                        InspectionDescription = x.InspectionViolation.Description,
                        CodesPin = x.InspectionViolation.Violation.NormativeDocNames,
                        x.Description
                    })
                .Filter(loadParam, Container);

            int totalCount = data.Any() ? data.Count() : 0;

            data = data
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObject);

            return new ListDataResult(data.Order(loadParam).ToList(), totalCount);
        }
    }
}