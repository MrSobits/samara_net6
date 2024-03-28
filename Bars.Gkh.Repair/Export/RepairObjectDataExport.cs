namespace Bars.Gkh.Repair.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Repair.DomainService;
    using Bars.Gkh.Repair.Entities;
    using Bars.Gkh.Repair.Enums;

    public class RepairObjectDataExport : BaseDataExportService
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
        }

        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var programId = baseParams.Params.GetAs("programId", string.Empty);
            var municipalityId = baseParams.Params.GetAs("municipalityId", string.Empty);

            var programIds = !string.IsNullOrEmpty(programId) ? programId.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
            var municipalityIds = !string.IsNullOrEmpty(municipalityId) ? municipalityId.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];


            var repairObjectQuery = this.RepairObjectService.GetFilteredByOperator()
                .WhereIf(programIds.Length > 0, x => programIds.Contains(x.RepairProgram.Id))
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(y => this.RepairProgramDomain.GetAll().Any(x =>
                        x.Id == y.RepairProgram.Id
                        && x.TypeVisibilityProgramRepair != TypeVisibilityProgramRepair.Hidden
                        && x.TypeVisibilityProgramRepair != TypeVisibilityProgramRepair.Print));

            IQueryable<RepairObjectProxy> resultQuery;

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

                resultQuery = repairObjectQuery
                    .Where(x => repairObjectIdsQuery.Contains(x.Id))
                    .Select(x => new RepairObjectProxy
                    {
                        Id = x.Id,
                        RealityObjName = x.RealityObject.Address,
                        Municipality = x.RealityObject.Municipality.Name,
                        State = x.State,
                        RepairProgramName = x.RepairProgram.Name
                    })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParams.Order.Length == 0, true, x => x.RealityObjName);
            }
            else
            {
                resultQuery = repairObjectQuery
                    .Select(x => new RepairObjectProxy
                    {
                        Id = x.Id,
                        RealityObjName = x.RealityObject.Address,
                        Municipality = x.RealityObject.Municipality.Name,
                        State = x.State,
                        RepairProgramName = x.RepairProgram.Name
                    })
                    .Filter(loadParams, this.Container)
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParams.Order.Length == 0, true, x => x.RealityObjName);
            }

            var buildersDicts = this.RepairWorkDomain.GetAll()
                .Where(x => repairObjectQuery.Any(y => y.Id == x.RepairObject.Id))
                .Select(x => new { x.RepairObject.Id, x.Builder })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => string.Join(", ", x.Select(y => y.Builder).Distinct()));

            var result = resultQuery
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.RepairProgramName,
                    x.RealityObjName,
                    x.Municipality,
                    Builder = buildersDicts.ContainsKey(x.Id) ? buildersDicts[x.Id] : string.Empty
                })
                .ToList();

            if (loadParams.Order.Length > 0)
            {
                result = result.AsQueryable().Order(loadParams).ToList();
            }

            return result;
        }
    }
}