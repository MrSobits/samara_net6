namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;
    using Gkh.Entities;
    using Overhaul.DomainService;

    public class RealityObjectsProgramVersion : IRealityObjectsProgramVersion
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }
        
        public IQueryable<RealityObject> GetMainVersionRealityObjects()
        {
            return VersionRecordDomain.GetAll()
                .Where(x => x.ProgramVersion.IsMain && x.ProgramVersion.State.FinalState)
                .Select(x => x.RealityObject);
        }
    }
}