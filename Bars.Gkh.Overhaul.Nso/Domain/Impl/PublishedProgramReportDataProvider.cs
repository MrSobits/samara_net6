namespace Bars.Gkh.Overhaul.Domain.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Domain;
    using Bars.Gkh.Overhaul.Nso.Entities;

    using Castle.Windsor;

    public class PublishedProgramReportDataProvider : IPublishedProgramReportDataProvider
    {
        public IWindsorContainer Container { get; set; }

        public IEnumerable<PublProgRecRep> GetData(long[] municipalityIds, int startYear, int endYear)
        {
            var versionRecordDomain = this.Container.Resolve<IDomainService<VersionRecord>>();
            var publishedProgramRecordDomain = this.Container.Resolve<IDomainService<PublishedProgramRecord>>();

            var result = new List<PublProgRecRep>();

            using (this.Container.Using(versionRecordDomain, publishedProgramRecordDomain))
            {
                var dataPublished =
                    publishedProgramRecordDomain.GetAll()
                        .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.PublishedProgram.ProgramVersion.Municipality.Id))
                        .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                        .Select(x => new { x.Stage2.Stage3Version.Id, x.PublishedYear })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.PublishedYear).FirstOrDefault());

                var query =
                    versionRecordDomain.GetAll()
                        .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ProgramVersion.Municipality.Id))
                        .Where(x => x.ProgramVersion.IsMain)
                        .Where(x => publishedProgramRecordDomain.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id && y.PublishedProgram.ProgramVersion.IsMain))
                        .Select(x => new
                        {
                            x.Id,
                            House = x.RealityObject.Id,
                            MuName = x.RealityObject.Municipality.Name,
                            Settlement = x.RealityObject.MoSettlement.Name,
                            x.RealityObject.Address,
                            Year = 0,
                            x.Sum,
                            Ooi = x.CommonEstateObjects,
                            x.RealityObject.AreaMkd,
                            x.RealityObject.AreaLivingNotLivingMkd,
                            x.RealityObject.AreaLiving
                        })
                        .AsEnumerable();

                result =
                    query
                        .Select(x => new PublProgRecRep
                        {
                            House = x.House,
                            MuName = x.MuName,
                            Settlement = x.Settlement ?? string.Empty,
                            Address = x.Address,
                            Year = dataPublished.ContainsKey(x.Id) ? dataPublished[x.Id] : 0,
                            Cost = x.Sum,
                            Ooi = x.Ooi,
                            TotalAreaMkd = x.AreaMkd ?? 0,
                            TotalAreaLivNotLiv = x.AreaLivingNotLivingMkd ?? 0,
                            TotalAreaLiving = x.AreaLiving ?? 0
                        })
                        .Where(x => x.Year >= startYear && x.Year <= endYear)
                        .OrderBy(x => x.MuName)
                        .ThenBy(x => x.Settlement)
                        .ThenBy(x => x.Address)
                        .ThenBy(x => x.Year)
                        .ToList();
            }

            return result;
        }
    }
}