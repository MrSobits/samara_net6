namespace Bars.Gkh.Repair.DomainService
{
    using System.Linq;
    using B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Repair.Entities;
    using Bars.Gkh.Repair.Entities.RepairControlDate;

    using Castle.Windsor;

    public class RepairControlDateService : IRepairControlDateService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<RepairControlDate> RepairControlDateDomain { get; set; }
        public IDomainService<RepairProgram> RepairProgramDomain { get; set; }
        public IDomainService<WorkKindCurrentRepair> WorkKindCurrentRepairDomain { get; set; }

        public IDataResult AddWorks(BaseParams baseParams)
        {
            try
            {
                var repairPogramId = baseParams.Params.GetAs("repairPogramId", 0L);

                var objectIds = baseParams.Params.GetAs("objectIds", new long[]{});
                
                var exsistingControlDates = this.RepairControlDateDomain.GetAll()
                    .Where(x => x.RepairProgram.Id == repairPogramId)
                    .Select(x => x.Work.Id)
                    .ToList();

                foreach (var id in objectIds.Where(x => !exsistingControlDates.Contains(x)))
                {
                    var newControlDate = new RepairControlDate
                    {
                        RepairProgram = this.RepairProgramDomain.Load(repairPogramId),
                        Work = this.WorkKindCurrentRepairDomain.Load(id),
                    };

                    this.RepairControlDateDomain.Save(newControlDate);
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = exc.Message
                };
            }
        }
        
    }
}
