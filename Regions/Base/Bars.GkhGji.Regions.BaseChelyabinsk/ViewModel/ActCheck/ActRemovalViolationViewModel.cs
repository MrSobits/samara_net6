namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.ActCheck
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

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
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

    }
}