namespace Bars.Gkh.Repair.ViewModel
{
    using B4;
    using B4.Modules.FileStorage;
    using B4.Modules.States;
    using B4.Utils;
    using DomainService;
    using Entities;
    using Enums;
    using System.Linq;

    public class RepairObjectViewModel : BaseViewModel<RepairObject>
    {
        public IDomainService<RepairProgram> RepairProgramDomain { get; set; }
        public IDomainService<RepairWork> RepairWorkDomain { get; set; }
        public IRepairObjectService RepairObjectService { get; set; }

        private sealed class RepairObjectProxy
        {
            public long Id { get; set; }
            public State State { get; set; }
            public string RepairProgramName { get; set; }
            public string RealityObjName { get; set; }
            public string Municipality { get; set; }
            public string DocumentName { get; set; }
            public FileInfo Document { get; set; }
            public string Comment { get; set; }
        }

        public override IDataResult List(IDomainService<RepairObject> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var programId = baseParams.Params.GetAs("programId", string.Empty);
            var municipalityId = baseParams.Params.GetAs("municipalityId", string.Empty);

            var programIds = !string.IsNullOrEmpty(programId) ? programId.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
            var municipalityIds = !string.IsNullOrEmpty(municipalityId) ? municipalityId.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];

            var stateId = baseParams.Params.GetAs<long>("stateId");

            var repairObjectQuery = this.RepairObjectService.GetFilteredByOperator()
                .WhereIf(stateId > 0, x => x.State.Id == stateId)
                .WhereIf(programIds.Length > 0, x => programIds.Contains(x.RepairProgram.Id))
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(y => this.RepairProgramDomain.GetAll().Any(x =>
                        x.Id == y.RepairProgram.Id
                        && x.TypeVisibilityProgramRepair != TypeVisibilityProgramRepair.Hidden
                        && x.TypeVisibilityProgramRepair != TypeVisibilityProgramRepair.Print));

            OrderField builderOrder = null;
            if (loadParams.Order.Any(x => x.Name == "Builder"))
            {
                builderOrder = loadParams.Order.FirstOrDefault(x => x.Name == "Builder");
                loadParams.Order = loadParams.Order.Where(x => x.Name != "Builder").ToArray();
            }

            IQueryable<RepairObjectProxy> resultQuery;
            int totalCount;

            if (loadParams.CheckRuleExists("Builder"))
            {
                var repairObjectIdsQuery = this.RepairWorkDomain.GetAll()
                     .Select(x => new
                     {
                         x.RepairObject.Id,
                         RealityObjName = x.RepairObject.RealityObject.Address,
                         Municipality = x.RepairObject.RealityObject.Municipality.Name,
                         x.RepairObject.State,
                         RepairProgramName = x.RepairObject.RepairProgram.Name,
                         x.Builder
                     })
                     .Filter(loadParams, this.Container)
                     .Select(x => x.Id);

                totalCount = repairObjectIdsQuery.Distinct().Count();

                resultQuery = repairObjectQuery
                    .Where(x => repairObjectIdsQuery.Contains(x.Id))
                    .Select(x => new RepairObjectProxy
                    {
                        Id = x.Id,
                        RealityObjName = x.RealityObject.Address,
                        Municipality = x.RealityObject.Municipality.Name,
                        State = x.State,
                        RepairProgramName = x.RepairProgram.Name,
                        Document = x.ReasonDocument,
                        DocumentName = x.ReasonDocument != null ? x.ReasonDocument.Name : string.Empty,
                        Comment = x.Comment
                    })
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParams.Order.Length == 0, true, x => x.RealityObjName);
            }
            else
            {
                var resultQueryWithoutBuilder = repairObjectQuery
                    .Select(x => new RepairObjectProxy
                    {
                        Id = x.Id,
                        RealityObjName = x.RealityObject.Address,
                        Municipality = x.RealityObject.Municipality.Name,
                        State = x.State,
                        RepairProgramName = x.RepairProgram.Name,
                        Document = x.ReasonDocument,
                        DocumentName = x.ReasonDocument != null ? x.ReasonDocument.Name : string.Empty,
                        Comment = x.Comment
                    })
                    .Filter(loadParams, this.Container);

                totalCount = resultQueryWithoutBuilder.Count();

                resultQuery = resultQueryWithoutBuilder
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParams.Order.Length == 0, true, x => x.RealityObjName);
            }

            resultQuery = loadParams.Order.Length == 0
                    ? resultQuery.Paging(loadParams)
                    : resultQuery.Order(loadParams).Paging(loadParams);

            var res = resultQuery.ToArray();

            var repairObjectIds = res.Select(x => x.Id).ToList(); // После пэйждинга ожидаем не более 500 записей

            var buildersDicts = this.RepairWorkDomain.GetAll()
                .Where(x => repairObjectIds.Contains(x.RepairObject.Id))
                .Select(x => new { x.RepairObject.Id, x.Builder })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => string.Join(", ", x.Select(y => y.Builder).Distinct()));

            var result = res
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.RepairProgramName,
                    x.RealityObjName,
                    x.Municipality,
                    Builder = buildersDicts.ContainsKey(x.Id) ? buildersDicts[x.Id] : string.Empty,
                    x.DocumentName,
                    x.Document,
                    x.Comment
                })
                .ToList();

            if (builderOrder != null)
            {
                result = builderOrder.Asc
                             ? result.OrderBy(x => x.Builder).ToList()
                             : result.OrderByDescending(x => x.Builder).ToList();
            }

            return new ListDataResult(result, totalCount);
        }
    }
}