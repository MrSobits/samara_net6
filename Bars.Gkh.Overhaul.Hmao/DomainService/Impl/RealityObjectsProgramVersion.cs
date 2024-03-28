namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class RealityObjectsProgramVersion : IRealityObjectsProgramVersion
    {
        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        public IQueryable<RealityObject> GetMainVersionRealityObjects()
        {
            return VersionRecordDomain.GetAll()
                .Where(x => x.ProgramVersion.IsMain)
                .Select(x => x.RealityObject);
        }
    }
}