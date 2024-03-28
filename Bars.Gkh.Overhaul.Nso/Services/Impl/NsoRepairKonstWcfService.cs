namespace Bars.Gkh.Overhaul.Nso.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Overhaul.Services.DataContracts;


    public class NsoRepairKonstWcfService : IRepairKonstWcfService
    {
        public IDomainService<PublishedProgramRecord> PublishedProgramRecordDomain { get; set; }
        
        public RepairKonstProxy[] GetRepairKonstWcfService(long roId)
        {
            return PublishedProgramRecordDomain.GetAll()
                .Where(x => x.Stage2.Stage3Version.RealityObject.Id == roId)
                .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                .Select(x => new RepairKonstProxy
                {
                    NameOoi = x.CommonEstateobject,
                    YearPublic = x.PublishedYear
                })
                .ToArray();
        }
    }
}