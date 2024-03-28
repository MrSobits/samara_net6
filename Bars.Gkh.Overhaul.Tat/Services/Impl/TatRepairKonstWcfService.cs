namespace Bars.Gkh.Overhaul.Tat.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Services.DataContracts;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class TatRepairKonstWcfService : IRepairKonstWcfService
    {
        public IDomainService<DpkrCorrectionStage2> DpkrCorrectionStage2Domain { get; set; }

        public RepairKonstProxy[] GetRepairKonstWcfService(long roId)
        {
            return DpkrCorrectionStage2Domain.GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                .Select(x => new RepairKonstProxy
                {
                    YearPublic = x.PlanYear,
                    NameOoi = x.Stage2.CommonEstateObject.Name
                })
                .ToArray();
        }
    }
}