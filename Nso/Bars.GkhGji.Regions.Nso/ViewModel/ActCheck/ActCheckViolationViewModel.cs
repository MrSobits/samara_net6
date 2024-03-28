namespace Bars.GkhGji.Regions.Nso.ViewModel
{
    using System.Linq;
    using System.Text;
    using B4.DataAccess;
    using B4;
    using B4.Utils;
    using Entities;
    using GkhGji.Entities;
    using Gkh.Domain;
    using Gkh.Utils;

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

            var actRemViolDict = Container.ResolveDomain<ViolationActionsRemovGji>();

            var violFeatureDict = Container.ResolveDomain<ViolationFeatureGji>().GetAll()
                .Select(x => new
                {
                    violId = x.ViolationGji.Id,
                    x.FeatureViolGji.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.violId)
                .ToDictionary(x => x.Key, z => z.Select(x => x.Name).Distinct().AggregateWithSeparator("; "));

            var blobDomain = Container.ResolveDomain<ActCheckViolationLongText>();

            var blobs = blobDomain.GetAll()
                .Where(x => x.Violation.ActObject.Id == objectId)
                .Select(x => new
                {
                    x.Violation.Id,
                    x.Description
                })
                .ToList()
                .ToDictionary(x => x.Id, x => Encoding.UTF8.GetString(x.Description));

            var data = domainService.GetAll()
                .WhereIf(objectId > 0, x => x.ActObject.Id == objectId)
                .WhereIf(documentId > 0, x => x.Document.Id == documentId)
                .WhereIf(forSelect, x => x.InspectionViolation.DateFactRemoval == null)
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
                    CodesPin = x.InspectionViolation.Violation.NormativeDocNames,
                    ViolationDescription = blobs.Get(x.Id) ?? "",
                    Municipality =
                        x.InspectionViolation.RealityObject != null
                            ? x.InspectionViolation.RealityObject.Municipality.Name
                            : "",
                    RealityObject =
                        x.InspectionViolation.RealityObject != null
                            ? x.InspectionViolation.RealityObject.Address
                            : "",
                    Features = violFeatureDict.Get(x.InspectionViolation.Violation.Id) ?? "",
                    ActionsRemovViolName =
                        actRemViolDict.GetAll()
                            .Where(y => y.ViolationGji.Id == x.InspectionViolation.Violation.Id)
                            .Select(y => y.ActionsRemovViol.Name)
                            .FirstOrDefault()
                })
                .AsQueryable()
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }

        public override IDataResult Get(IDomainService<ActCheckViolation> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAsId());

            var violFeatDomain = Container.ResolveDomain<ViolationFeatureGji>();

            string violFeature;

            try
            {
                violFeature = Container.ResolveDomain<ViolationFeatureGji>().GetAll()
                    .Where(x => x.ViolationGji.Id == obj.InspectionViolation.Violation.Id)
                    .Select(x => x.FeatureViolGji.Name)
                    .Distinct()
                    .ToList()
                    .AggregateWithSeparator("; ");
            }
            finally
            {
                Container.Release(violFeatDomain);
            }

            return new BaseDataResult(new
            {
                obj.Id,
                obj.DatePlanRemoval,
                InspectionViolationId = obj.InspectionViolation.Id,
                obj.InspectionViolation.DateFactRemoval,
                ViolationGjiName = obj.InspectionViolation.Violation.Name,
                ViolationGjiId = obj.InspectionViolation.Violation.Id,
                ViolationGjiPin = obj.InspectionViolation.Violation.CodePin,
                CodesPin = obj.InspectionViolation.Violation.NormativeDocNames,
                Features = violFeature
            });
        }
    }
}