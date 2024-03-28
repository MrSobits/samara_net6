namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;

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
                    Municipality = x.InspectionViolation.RealityObject.Municipality.Name,
                    RealityObject = x.InspectionViolation.RealityObject.Address,
                    ViolationGji = x.InspectionViolation.Violation.Name,
                    ViolationGjiPin = x.InspectionViolation.Violation.CodePin,
                    CodesPin = x.InspectionViolation.Violation.NormativeDocNames,
                    x.CircumstancesDescription
                })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        /*public override IDataResult Get(IDomainService<ActRemovalViolation> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAs<long>("id"));

            if (obj.InspectionViolation != null)
            {
                var violation = Container.Resolve<IDomainService<ViolationGji>>().Get(obj.InspectionViolation.Violation.Id);

                obj.InspectionViolation.ViolationGji = violation.Name;
            }

            return new BaseDataResult(obj);
        }*/
    }
}