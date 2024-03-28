using System;

namespace Bars.GkhGji.Regions.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ProtocolViolationViewModel : Bars.GkhGji.ViewModel.ProtocolViolationViewModel
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

            var violFeatureDict = this.Container.Resolve<IDomainService<ViolationFeatureGji>>().GetAll()
                .Select(x => new
                {
                    violId = x.ViolationGji.Id,
                    x.FeatureViolGji.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.violId)
                .ToDictionary(x => x.Key, x => string.Join("; ", x.Select(y => y.Name).Distinct()));

	        var data = domainService.GetAll()
		        .Where(x => x.Document.Id == documentId)
		        .WhereIf(realityObjId > 0, x => x.InspectionViolation.RealityObject.Id == realityObjId)
		        .Select(x => new
		        {
			        x.Id,
			        InspectionViolationId = x.InspectionViolation.Id,
			        Municipality = x.InspectionViolation.RealityObject.Municipality.Name,
			        RealityObject = x.InspectionViolation.RealityObject.Address,
			        RealityObjectId =
				        x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Id : 0,
			        x.DatePlanRemoval,
			        x.InspectionViolation.DateFactRemoval,
			        ViolationGji = x.InspectionViolation.Violation.Name,
			        ViolationGjiPin = x.InspectionViolation.Violation.CodePin,
			        CodesPin = x.InspectionViolation.Violation.NormativeDocNames,
			        x.Description,
			        violationId = (long?) x.InspectionViolation.Violation.Id
		        });
			
            var partResult = data
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
                    x.CodesPin,
                    Features =
                        x.violationId.HasValue && violFeatureDict.ContainsKey(x.violationId.Value)
                            ? violFeatureDict[x.violationId.Value]
                            : string.Empty,
                })
                .ToList();

            var actCheckReality = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var ids = partResult.Select(v => v.RealityObjectId).ToArray();
            var actDescriptions = (
                from v in actCheckReality.GetAll()
                where ids.Contains(v.RealityObject.Id)
                select new {v.RealityObject.Id, ActDescription = v.Description}
                ).ToArray()
                .Distinct(v => v.Id)
                .ToDictionary(k => k.Id, v => v.ActDescription);

	        var result = partResult.Select(x => new
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
		        x.CodesPin,
		        x.Features,
		        ActDescription =
			        actDescriptions.ContainsKey(x.RealityObjectId)
				        ? actDescriptions[x.RealityObjectId] ?? string.Empty
				        : string.Empty
	        })
		    .AsQueryable()
		    .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
		    .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
		    .Filter(loadParam, Container);

			var totalCount = result.Count();

			return new ListDataResult(result.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}