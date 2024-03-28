namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.Protocol197
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class Protocol197ViolationViewModel : BaseViewModel<Protocol197Violation>
    {
        public override IDataResult List(IDomainService<Protocol197Violation> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var documentId = baseParams.Params.GetAs<long>("documentId");
            var forSelect = baseParams.Params.ContainsKey("forSelect");

            var violFeatureDict = this.Container.ResolveDomain<ViolationFeatureGji>().GetAll()
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
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }

		public override IDataResult Get(IDomainService<Protocol197Violation> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAsId());

            var violFeatDomain = this.Container.ResolveDomain<ViolationFeatureGji>();

            string violFeature;

            try
            {
                violFeature = this.Container.ResolveDomain<ViolationFeatureGji>().GetAll()
                    .Where(x => x.ViolationGji.Id == obj.InspectionViolation.Violation.Id)
                    .Select(x => x.FeatureViolGji.Name)
                    .Distinct()
                    .ToList()
                    .AggregateWithSeparator("; ");
            }
            finally
            {
                this.Container.Release(violFeatDomain);
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