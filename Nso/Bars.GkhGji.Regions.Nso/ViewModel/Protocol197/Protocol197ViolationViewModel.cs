namespace Bars.GkhGji.Regions.Nso.ViewModel
{
    using System.Linq;
    using B4.DataAccess;
    using B4;
    using B4.Utils;
    using Entities;
    using GkhGji.Entities;
    using Gkh.Domain;
    using Gkh.Utils;

	public class Protocol197ViolationViewModel : BaseViewModel<Protocol197Violation>
    {
        public override IDataResult List(IDomainService<Protocol197Violation> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var documentId = baseParams.Params.GetAs<long>("documentId");
            var forSelect = baseParams.Params.ContainsKey("forSelect");

            var violFeatureDict = Container.ResolveDomain<ViolationFeatureGji>().GetAll()
                .Select(x => new
                {
                    violId = x.ViolationGji.Id,
                    x.FeatureViolGji.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.violId)
                .ToDictionary(x => x.Key, z => z.Select(x => x.Name).Distinct().AggregateWithSeparator("; "));

            var data = domainService.GetAll()
                .WhereIf(documentId > 0, x => x.Document.Id == documentId)
                .WhereIf(forSelect, x => x.InspectionViolation.DateFactRemoval == null)
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    InspectionViolationId = x.InspectionViolation.Id,
                    x.DatePlanRemoval,
                    x.DateFactRemoval,
                    ViolationGjiName = x.InspectionViolation.Violation.Name,
                    ViolationGjiId = x.InspectionViolation.Violation.Id,
                    ViolationGjiPin = x.InspectionViolation.Violation.CodePin,
                    CodesPin = x.InspectionViolation.Violation.NormativeDocNames,
					x.Description,
                    Features = violFeatureDict.Get(x.InspectionViolation.Violation.Id) ?? ""
                })
                .AsQueryable()
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }

		public override IDataResult Get(IDomainService<Protocol197Violation> domainService, BaseParams baseParams)
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
                obj.DateFactRemoval,
                InspectionViolationId = obj.InspectionViolation.Id,
                ViolationGjiName = obj.InspectionViolation.Violation.Name,
                ViolationGjiId = obj.InspectionViolation.Violation.Id,
                ViolationGjiPin = obj.InspectionViolation.Violation.CodePin,
                CodesPin = obj.InspectionViolation.Violation.NormativeDocNames,
                Features = violFeature,
				obj.Description
            });
        }
    }
}