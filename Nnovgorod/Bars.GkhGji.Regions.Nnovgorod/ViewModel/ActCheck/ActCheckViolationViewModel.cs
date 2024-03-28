namespace Bars.GkhGji.Regions.Nnovgorod.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Nnovgorod.Entities;

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

            var actRemViolDict = this.Container.Resolve<IDomainService<ViolationActionsRemovGji>>();
            
            var actCheckViolationQuery = domainService.GetAll()
                .WhereIf(objectId > 0, x => x.ActObject.Id == objectId)
                .WhereIf(documentId > 0, x => x.Document.Id == documentId)
                .WhereIf(forSelect, x => x.InspectionViolation.DateFactRemoval == null);

            var serviceInspectionGjiViolWording = Container.Resolve<IDomainService<InspectionGjiViolWording>>();

            var violationWordingsDict = serviceInspectionGjiViolWording.GetAll()
                .Where(x => actCheckViolationQuery.Select(y => y.InspectionViolation.Id).Contains(x.InspectionViolation.Id))
                .Select(x => new { violId = x.InspectionViolation.Id, x.Wording })
                .AsEnumerable()
                .GroupBy(x => x.violId)
                .ToDictionary(x => x.Key, x => string.Join("; ", x.Select(y => y.Wording)));

            var data = actCheckViolationQuery
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    InspectionViolationId = x.InspectionViolation.Id,
                    x.DatePlanRemoval,
                    x.InspectionViolation.DateFactRemoval,
                    ViolationGjiName = x.InspectionViolation.Violation.Name,
                    ViolationGjiId = x.InspectionViolation.Violation.Id,
                    ViolationGjiPin = x.InspectionViolation.Violation.CodePin,
                    Municipality = x.InspectionViolation.RealityObject.Municipality.Name,
                    RealityObject = x.InspectionViolation.RealityObject.Address,
                    ViolationWording = violationWordingsDict.ContainsKey(x.InspectionViolation.Id) ? violationWordingsDict[x.InspectionViolation.Id] : string.Empty,
                    ActionsRemovViolName = actRemViolDict.GetAll().Where(y => y.ViolationGji.Id == x.InspectionViolation.Violation.Id).Select(y => y.ActionsRemovViol.Name).FirstOrDefault()
                })
                .AsQueryable()
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
                .Filter(loadParam, this.Container);
            
            return new ListDataResult(data.Order(loadParam).Paging(loadParam), data.Count());
        }
    }
}