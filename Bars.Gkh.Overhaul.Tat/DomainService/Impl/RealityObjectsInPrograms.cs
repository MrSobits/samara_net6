namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;
    using Gkh.Entities;
    using Overhaul.DomainService;

    public class RealityObjectsInPrograms : IRealityObjectsInPrograms
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<ShortProgramRecord> ShortProgramRecordDomain { get; set; }
        public IDomainService<ShortProgramRealityObject> ShortProgramRoDomain { get; set; }
        public IDomainService<DpkrCorrectionStage2> PublishedProgramDomain { get; set; }

        public IQueryable<RealityObject> GetInShortProgram(int year)
        {
            return ShortProgramRoDomain.GetAll()
                .WhereIf(year > 0, x => x.ProgramVersion.VersionDate.Year == year)
                .Select(x => x.RealityObject);
        }

        public IQueryable<RealityObject> GetInPublishedProgram()
        {
            return PublishedProgramDomain.GetAll()
                .Select(x => x.RealityObject);
        }

        public IQueryable<RealityObject> GetInPublishedProgramByMunicipality(long[] muIds)
        {
            return PublishedProgramDomain.GetAll()
                .Where(x => muIds.Contains(x.RealityObject.Municipality.Id))
                .Select(x => x.RealityObject);
        }

        public IQueryable<RecordObject> GetShortProgramRecordData()
        {
            return ShortProgramRecordDomain.GetAll()
                .Select(x => new RecordObject
                {
                    Id = x.ShortProgramObject.RealityObject.Id,
                    TotalCost = x.TotalCost
                });
        }
    }
}