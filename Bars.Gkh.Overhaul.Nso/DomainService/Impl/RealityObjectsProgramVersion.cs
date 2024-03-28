namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Castle.Windsor;

    public class RealityObjectsProgramVersion : IRealityObjectsProgramVersion
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }
        
        public IQueryable<RealityObject> GetMainVersionRealityObjects()
        {
            return VersionRecordDomain.GetAll()
                .Where(x => x.ProgramVersion.IsMain && x.ProgramVersion.IsMain)
                .Select(x => x.RealityObject);
        }
    }
}