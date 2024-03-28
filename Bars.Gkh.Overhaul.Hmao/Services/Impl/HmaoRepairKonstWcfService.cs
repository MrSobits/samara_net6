namespace Bars.Gkh.Overhaul.Hmao.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Services.DataContracts;

    public class HmaoRepairKonstWcfService : IRepairKonstWcfService
    {
        public IDomainService<DpkrCorrectionStage2> DpkrCorrectionStage2Domain { get; set; }
        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        public RepairKonstProxy[] GetRepairKonstWcfService(long roId)
        {
            //return DpkrCorrectionStage2Domain.GetAll().Where(x => x.RealityObject.Id == roId)
            //    .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
            //    .Select(x => new RepairKonstProxy
            //    {
            //        YearPublic = x.PlanYear,
            //        NameOoi = x.Stage2.CommonEstateObject.Name
            //    })
            //    .ToArray();

            return VersionRecordDomain.GetAll().Where(x => x.RealityObject.Id == roId)
               .Where(x => x.ProgramVersion.IsMain)
               .Where(x=> x.Show)
               .Select(x => new RepairKonstProxy
               {
                   YearPublic = x.YearCalculated>0? x.YearCalculated: x.Year,
                   NameOoi = x.CommonEstateObjects
               })
               .ToArray();
        }
    }
}