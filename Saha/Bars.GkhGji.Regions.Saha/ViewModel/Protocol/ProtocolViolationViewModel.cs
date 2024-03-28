namespace Bars.GkhGji.Regions.Saha.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Saha.Entities;

    public class ProtocolViolationViewModel : GkhGji.ViewModel.ProtocolViolationViewModel
    {
        public override IDataResult List(IDomainService<ProtocolViolation> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            /*
             * Тут могут быть следующие параметры
             * documentId - идентификатор протокола
             * realityObjId - идентификатор дома
             */
            var documentId = baseParams.Params.GetAs<long>("documentId");
            var realityObjId = baseParams.Params.GetAs<long>("realityObjId");

            var serviceInspectionGjiViolWording = Container.Resolve<IDomainService<InspectionGjiViolWording>>();

            var protocolViolationQuery = domainService.GetAll()
                .Where(x => x.Document.Id == documentId)
                .WhereIf(realityObjId > 0, x => x.InspectionViolation.RealityObject.Id == realityObjId)
                .WhereIf(realityObjId == 0, x => x.InspectionViolation.RealityObject == null);

            var violationWordingsDict = serviceInspectionGjiViolWording.GetAll()
                .Where(x => protocolViolationQuery.Select(y => y.InspectionViolation.Id).Contains(x.InspectionViolation.Id))
                .Select(x => new { violId = x.InspectionViolation.Id, x.Wording })
                .AsEnumerable()
                .GroupBy(x => x.violId)
                .ToDictionary(x => x.Key, x => string.Join("; ", x.Select(y => y.Wording)));

            var data = protocolViolationQuery
                .Select(x => new
                    {
                        x.Id,
                        InspectionViolationId = x.InspectionViolation.Id,
                        Municipality = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Municipality.Name : null,
                        RealityObject = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Address: null,
                        RealityObjectId = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Id : 0,
                        x.DatePlanRemoval,
                        x.InspectionViolation.DateFactRemoval,
                        ViolationGji = x.InspectionViolation.Violation.Name,
                        ViolationGjiPin = x.InspectionViolation.Violation.CodePin,
                        x.Description
                    })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.InspectionViolationId,
                    x.Municipality,
                    x.RealityObject,
                    x.RealityObjectId,
                    x.DatePlanRemoval,
                    x.DateFactRemoval,
                    x.ViolationGji,
                    x.ViolationGjiPin,
                    x.Description,
                    ViolationWording = violationWordingsDict.ContainsKey(x.InspectionViolationId) ? violationWordingsDict[x.InspectionViolationId] : string.Empty,
                })
                .AsQueryable()
                .Filter(loadParam, this.Container);
            
            return new ListDataResult(data.Order(loadParam).Paging(loadParam), data.Count());
        }
    }
}