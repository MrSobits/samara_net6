﻿namespace Bars.GkhGji.Regions.Smolensk.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    public class ActCheckViolationViewModel : GkhGji.ViewModel.ActCheckViolationViewModel
    {
        public override IDataResult List(IDomainService<ActCheckViolation> domainService, BaseParams baseParams)
        {
            /*
             * Если методу передан objectId то значи необходимо получить все нарушения 
             * Объекта Акта проверки . То есть все нарушения по дому проверки
             *
             * Если передан documentId, то необходимо получить все нарушения Акта проверки
             * на которые можно создавать предписания и протоколы.
             *
             * Если передан forSelect то необходимо получить все возможные нарушения для массового выбора чтобы 
             * создавать Предписания и Пртоколы только по тем нарушениям у которых не выставлена дата фактического устранения   
            */

            var loadParam = baseParams.GetLoadParam();

            var objectId = baseParams.Params.GetAs<long>("objectId");

            var documentId = baseParams.Params.GetAs<long>("documentId");

            var forSelect = baseParams.Params.ContainsKey("forSelect");

            var ids = baseParams.Params.GetAs("Id", string.Empty);
            var listIds = !string.IsNullOrEmpty(ids) ? ids.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var actRemViolDict = this.Container.Resolve<IDomainService<ViolationActionsRemovGji>>();
            
            var actCheckViolationQuery = domainService.GetAll()
                .WhereIf(objectId > 0, x => x.ActObject.Id == objectId)
                .WhereIf(documentId > 0, x => x.Document.Id == documentId)
                .WhereIf(documentId == 0 && objectId == 0, x => x.Id == 0)
                .WhereIf(forSelect, x => x.InspectionViolation.DateFactRemoval == null)
                .WhereIf(listIds.Length > 0, x => listIds.Contains(x.Id));

            var serviceInspectionGjiViolWording = Container.Resolve<IDomainService<InspectionGjiViolWording>>();

            var violationWordingsDict = serviceInspectionGjiViolWording.GetAll()
                .Where(x => actCheckViolationQuery.Select(y => y.InspectionViolation.Id).Contains(x.InspectionViolation.Id))
                .Select(x => new { violId = x.InspectionViolation.Id, x.Wording })
                .AsEnumerable()
                .GroupBy(x => x.violId)
                .ToDictionary(x => x.Key, x => string.Join("; ", x.Select(y => y.Wording)));

            var data = actCheckViolationQuery
                .Select(x => new
                {
                    x.Id,
                    InspectionViolationId = x.InspectionViolation.Id,
                    x.DatePlanRemoval,
                    x.InspectionViolation.DateFactRemoval,
                    ViolationGjiName = x.InspectionViolation.Violation.Name,
                    ViolationGjiId = x.InspectionViolation.Violation.Id,
                    ViolationGjiPin = x.InspectionViolation.Violation.CodePin,
                    Municipality = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Municipality.Name : null,
                    RealityObject = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Address : null
                })
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    x.InspectionViolationId,
                    x.DatePlanRemoval,
                    x.DateFactRemoval,
                    x.ViolationGjiName,
                    x.ViolationGjiId,
                    x.ViolationGjiPin,
                    x.Municipality,
                    x.RealityObject,
                    ViolationWording = violationWordingsDict.ContainsKey(x.InspectionViolationId) ? violationWordingsDict[x.InspectionViolationId] : string.Empty,
                    ActionsRemovViolName = actRemViolDict.GetAll().Where(y => y.ViolationGji.Id == x.ViolationGjiId).Select(y => y.ActionsRemovViol.Name).FirstOrDefault()
                })
                .AsQueryable()
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
                .Filter(loadParam, this.Container);

            int totalCount = data.Any() ? data.Count() : 0;

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}