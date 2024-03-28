namespace Bars.Gkh.Repair.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Repair.Entities;
    using Bars.Gkh.Repair.Enums;

    using Castle.Windsor;

    public class RepairProgramService : IRepairProgramService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<RepairProgram> RepairProgramDomainService { get; set; }

        public IDomainService<RealityObject> RealityObjectDomainService { get; set; }

        public IDomainService<RepairObject> RepairObjectDomainService { get; set; }

        public IDomainService<RepairProgramMunicipality> RepairProgramMunicipalityDomain{ get; set; }

        public IDataResult ListWithoutPaging(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var forRepairObject = baseParams.Params.GetAs("forRepairObject", false);

            var data = RepairProgramDomainService.GetAll()
                .WhereIf(
                    forRepairObject,
                    x => x.TypeVisibilityProgramRepair != TypeVisibilityProgramRepair.Hidden
                    && x.TypeVisibilityProgramRepair != TypeVisibilityProgramRepair.Print)
                .Select(x => new
                    {
                        x.Id,
                        x.Name
                    })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Name)
                .Order(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        public IDataResult ListAvailableRealtyObjects(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            
            var repairProgramId = baseParams.Params.GetAs("repairProgramId", 0);
            
            var programMunicipalityQuery = RepairProgramMunicipalityDomain.GetAll()
                .Where(x => x.RepairProgram.Id == repairProgramId)
                .Select(x => x.Municipality.Id);

            var objectsInProgramQuery = RepairObjectDomainService.GetAll()
                .Where(x => x.RepairProgram.Id == repairProgramId)
                .Select(x => x.RealityObject.Id);

            var query = RealityObjectDomainService.GetAll()
                .Where(x => programMunicipalityQuery.Contains(x.Municipality.Id))
                .Where(x => !objectsInProgramQuery.Contains(x.Id))
                .Select(x => new
                    {
                        x.Id,
                        x.Address,
                        Municipality = x.Municipality.Name
                    })
                .Filter(loadParams, this.Container);

            var totalCount = query.Count();

            return new ListDataResult(query.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}
