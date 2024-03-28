namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.Gkh.Overhaul.Nso.ConfigSections;

    using Castle.Windsor;
    using Entities;
    using Gkh.Entities;
    using Gkh.Utils;
    using Overhaul.DomainService;

    public class NsoRealityObjectsInPrograms : IRealityObjectsInPrograms
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<ShortProgramRecord> ShortProgramRecordDomain { get; set; }
        public IDomainService<PublishedProgramRecord> PublishedProgramRecordDomain { get; set; }
        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        public IQueryable<RealityObject> GetInShortProgram(int year)
        {
            return ShortProgramRecordDomain.GetAll()
                .WhereIf(year > 0, x => x.Year == year)
                .Select(x => x.RealityObject);
        }

        public IQueryable<RealityObject> GetInPublishedProgram()
        {
            return PublishedProgramRecordDomain.GetAll()
                .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                .Select(x => x.Stage2.Stage3Version.RealityObject);
        }

        public IQueryable<RealityObject> GetInPublishedProgramByMunicipality(long[] muIds)
        {
            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var groupByRoPeriod = config.GroupByRoPeriod;

            if (groupByRoPeriod == 0)
            {
                return PublishedProgramRecordDomain.GetAll()
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain
                        && muIds.Contains(x.PublishedProgram.ProgramVersion.Municipality.Id))
                    .Select(x => x.Stage2.Stage3Version.RealityObject);
            }
            return VersionRecordDomain.GetAll()
                    .Where(x => x.ProgramVersion.IsMain && muIds.Contains(x.ProgramVersion.Municipality.Id))
                    .Where(x => PublishedProgramRecordDomain.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id))
                    .Select(x => x.RealityObject);
        }

        public IQueryable<RecordObject> GetShortProgramRecordData()
        {
            return ShortProgramRecordDomain.GetAll()
                .Select(x => new RecordObject
                {
                    Id = x.RealityObject.Id,
                    TotalCost = x.Stage2.Sum
                });
        }
    }
}
