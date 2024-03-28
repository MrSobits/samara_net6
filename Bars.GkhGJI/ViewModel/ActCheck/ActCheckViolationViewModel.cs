namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActCheckViolationViewModel : BaseViewModel<ActCheckViolation>
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

            var objectId = baseParams.Params.ContainsKey("objectId")
                                   ? baseParams.Params["objectId"].ToLong()
                                   : 0;

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var forSelect = baseParams.Params.ContainsKey("forSelect");

            var actRemViolDict = this.Container.Resolve<IDomainService<ViolationActionsRemovGji>>();

            var data = domainService.GetAll()
                .WhereIf(objectId > 0, x => x.ActObject.Id == objectId)
                .WhereIf(documentId > 0, x => x.Document.Id == documentId)
                .WhereIf(documentId == 0 && objectId == 0, x => x.Id == 0)
                .WhereIf(forSelect, x => x.InspectionViolation.DateFactRemoval == null)
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    InspectionViolationId = x.InspectionViolation.Id,
                    InspectionDescription = x.InspectionViolation.Description,
                    DatePlanRemoval = x.DatePlanRemoval.HasValue ? x.DatePlanRemoval : x.InspectionViolation.DatePlanRemoval,
                    x.InspectionViolation.DateFactRemoval,
                    x.InspectionViolation.DateCancel,
                    InspectionAction = x.InspectionViolation.Action,
                    ViolationGjiName = x.InspectionViolation.Violation.Name,
                    ViolationGjiId = x.InspectionViolation.Violation.Id,
                    ViolationGjiPin = x.InspectionViolation.Violation.CodePin,
                    Municipality = x.InspectionViolation.RealityObject?.Municipality.Name,
                    RealityObject = x.InspectionViolation.RealityObject?.Address,
                    CodesPin = x.InspectionViolation.Violation.NormativeDocNames,
                    ActionsRemovViolName = actRemViolDict.GetAll()
                                            .Where(y => y.ViolationGji.Id == x.InspectionViolation.Violation.Id)
                                            .Select(y => y.ActionsRemovViol.Name).FirstOrDefault()
                })
                .AsQueryable()
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
                .Filter(loadParam, this.Container);

            return new ListDataResult(data.ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<ActCheckViolation> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params["id"].To<long>());

            if (obj.InspectionViolation != null)
            {
                var violation = this.Container.Resolve<IDomainService<ViolationGji>>().Load(obj.InspectionViolation.Violation.Id);

                obj.InspectionViolation.ViolationGji = violation.Name;
            }

            return new BaseDataResult(obj);
        }
    }
}