namespace Bars.GkhGji.Regions.Tula.ViewModel
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tula.Entities;

    public class ActRemovalViolationViewModel : BaseViewModel<ActRemovalViolation>
    {
        public override IDataResult List(IDomainService<ActRemovalViolation> domainService, BaseParams baseParams)
        {
            /*
             Если методу передан documentId то необходимо получить все Нарушения этого акта
             
             Если передан forSelect то необходимо получить все нарушения возможные для массового выбора чтобы 
             создавать Предписания и Пртоколы тольк опо тем нарушениям у которых невыставлена дата фактического устранения             
            */
            var loadParam = baseParams.GetLoadParam();
            var documentId = baseParams.Params.GetAs<long>("documentId");
            var forSelect = baseParams.Params.Contains("forSelect");

            var serviceInspectionGjiViolWording = Container.Resolve<IDomainService<InspectionGjiViolWording>>();

            var actRemovalViolationQuery = domainService.GetAll()
                .Where(x => x.Document.Id == documentId);

            var violationWordingsDict = serviceInspectionGjiViolWording.GetAll()
                .Where(x => actRemovalViolationQuery.Select(y => y.InspectionViolation.Id).Contains(x.InspectionViolation.Id))
                .Select(x => new { violId = x.InspectionViolation.Id, x.Wording })
                .AsEnumerable()
                .GroupBy(x => x.violId)
                .ToDictionary(x => x.Key, x => string.Join("; ", x.Select(y => y.Wording)));

            var data = domainService.GetAll()
                .WhereIf(documentId > 0, x => x.Document.Id == documentId)
                .WhereIf(forSelect, x => x.InspectionViolation.DateFactRemoval == null)
                .Select(x => new
                {
                    x.Id,
                    x.DatePlanRemoval,
                    InspectionDescription = x.InspectionViolation.Description,
                    x.InspectionViolation.DateFactRemoval,
                    x.InspectionViolation.SumAmountWorkRemoval,
                    InspectionViolationId = x.InspectionViolation.Id,
                    Municipality = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Municipality.Name : null,
                    RealityObject = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Address : null,
                    ViolationGji = x.InspectionViolation.Violation.Name,
                    ViolationGjiPin = x.InspectionViolation.Violation.CodePin
                })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.DatePlanRemoval,
                    x.InspectionDescription,
                    x.DateFactRemoval,
                    x.SumAmountWorkRemoval,
                    x.InspectionViolationId,
                    x.Municipality,
                    x.RealityObject,
                    x.ViolationGji,
                    x.ViolationGjiPin,
                    ViolationWording = violationWordingsDict.ContainsKey(x.InspectionViolationId) ? violationWordingsDict[x.InspectionViolationId] : string.Empty,
                })
                .AsQueryable()
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}