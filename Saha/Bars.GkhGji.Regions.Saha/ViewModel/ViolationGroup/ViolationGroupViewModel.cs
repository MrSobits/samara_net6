namespace Bars.GkhGji.Regions.Saha.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Saha.Entities;

    public class DocumentViolGroupViewModel : BaseViewModel<DocumentViolGroup>
    {
        public override IDataResult List(IDomainService<DocumentViolGroup> domainService, BaseParams baseParams)
        {
            var violPiontDomain = Container.Resolve<IDomainService<DocumentViolGroupPoint>>();

            try
            {

                var loadParam = baseParams.GetLoadParam();

                var roId = baseParams.Params.GetAs<long>("roId");

                var documentId = baseParams.Params.GetAs<long>("documentId");

                var query = domainService.GetAll()
                    .WhereIf(roId > 0, x => x.RealityObject.Id == roId)
                    .WhereIf(documentId > 0, x => x.Document.Id == documentId);

                var violPoints =
                    violPiontDomain.GetAll()
                                   .Where(x => query.Any(y => y.Id == x.ViolGroup.Id))
                                   .Select(
                                       x =>
                                       new
                                       {
                                           x.ViolStage.InspectionViolation.Violation.CodePin,
                                           ViolStageId = x.ViolStage.Id,
                                           violGroupId = x.ViolGroup.Id
                                       })
                                   .AsEnumerable()
                                   .GroupBy(x => x.violGroupId)
                                   .ToDictionary(
                                        x => x.Key,
                                        y => new
                                        {
                                            PointCodes = y.Select(z => z.CodePin)
                                                        .Distinct()
                                                        .Aggregate((str, result) => !string.IsNullOrEmpty(result) ? result + "," + str : str)
                                                        
                                           /* Для получения списка идентификаторы через запятую ненужны если будут нужны раскомментировать
                                            ,
                                            PointIds = y.Select(z => z.ViolStageId.ToString())
                                                        .Distinct()
                                                        .Aggregate((violId, result) => !string.IsNullOrEmpty(result) ? result + "," + violId : violId) */
                                        });

                var data = query
                    .Select(x => new
                    {
                        x.Id,
                        Document = x.Document.Id,
                        Municipality = x.RealityObject != null ? x.RealityObject.Municipality.Name : null,
                        RealityObject = x.RealityObject != null ? x.RealityObject.Address : null,
                        x.DatePlanRemoval,
                        x.DateFactRemoval,
                        x.Description,
                        x.Action
                    })
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        x.Document,
                        x.Municipality,
                        x.RealityObject,
                        x.DatePlanRemoval,
                        x.DateFactRemoval,
                        x.Description,
                        x.Action,
                        PointCodes = violPoints.ContainsKey(x.Id) ? violPoints[x.Id].PointCodes : null
                    })
                    .AsQueryable()
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
                    .Filter(loadParam, this.Container);
            
                return new ListDataResult(data.Order(loadParam).Paging(loadParam), data.Count());
            }
            finally
            {
                Container.Release(violPiontDomain);
            }

        }

        public override IDataResult Get(IDomainService<DocumentViolGroup> domainService, BaseParams baseParams)
        {
            var violPiontDomain = Container.Resolve<IDomainService<DocumentViolGroupPoint>>();

            try
            {
                var id = baseParams.Params.GetAs<long>("id");

                var violPoints =
                    violPiontDomain.GetAll()
                                   .Where(x => x.ViolGroup.Id == id)
                                   .Select(
                                       x =>
                                       new
                                       {
                                           x.ViolStage.InspectionViolation.Violation.CodePin,
                                           ViolStageId = x.ViolStage.Id,
                                           violGroupId = x.ViolGroup.Id
                                       })
                                   .AsEnumerable()
                                   .GroupBy(x => x.violGroupId)
                                   .ToDictionary(
                                        x => x.Key,
                                        y => new
                                        {
                                            PointCodes = y.Select(z => z.CodePin)
                                                        .Distinct()
                                                        .Aggregate((str, result) => !string.IsNullOrEmpty(result) ? result + "," + str : str),

                                            PointIds = y.Select(z => z.ViolStageId.ToString())
                                                        .Distinct()
                                                        .Aggregate((violId, result) => !string.IsNullOrEmpty(result) ? result + "," + violId : violId)
                                        });
                

                var obj =
                    domainService.GetAll()
                                    .Where(x => x.Id == id)
                                    .Select(
                                        x =>
                                        new DocViolGroupProxy
                                        {
                                            Id = x.Id,
                                            Document = x.Document.Id,
                                            Description = x.Description,
                                            Action = x.Action,
                                            DatePlanRemoval = x.DatePlanRemoval,
                                            DateFactRemoval = x.DateFactRemoval,
                                        })
                                    .FirstOrDefault();

                obj.PointCodes = violPoints.ContainsKey(id) ? violPoints[id].PointCodes : null;
                obj.PointIds = violPoints.ContainsKey(id) ? violPoints[id].PointIds : null;

                return new BaseDataResult(obj);
            }
            finally
            {
                Container.Release(violPiontDomain);
            }
        }

        private class DocViolGroupProxy
        {
            public long Id { get; set; }

            public long Document { get; set; }

            public string Description { get; set; }

            public string Action { get; set; }

            public DateTime? DatePlanRemoval { get; set; }

            public DateTime? DateFactRemoval { get; set; }

            public string PointCodes { get; set; }

            public string PointIds { get; set; }
        }
    }
}