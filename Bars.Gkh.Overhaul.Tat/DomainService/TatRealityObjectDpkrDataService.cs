namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain.CollectionExtensions;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Castle.Windsor;

    /// <inheritdoc />
    public class TatRealityObjectDpkrDataService : IRealityObjectDpkrDataService
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public List<JobYears> GetWorkInfoByRealityObject(RealityObject ro)
        {
            var stage1Query = this.Container.Resolve<IDomainService<VersionRecordStage1>>().GetAll()
                .Where(x => x.RealityObject.Id == ro.Id)
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.State.FinalState)
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain);

            var jobs = this.Container.Resolve<IDomainService<StructuralElementWork>>().GetAll()
                .Where(x => stage1Query.Any(y => y.StrElement.Id == x.StructuralElement.Id))
                .Select(x => new
                {
                    SeId = x.StructuralElement.Id,
                    x.Job
                })
                .AsEnumerable()
                .GroupBy(x => x.SeId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Job));

            var stage1Data =
                stage1Query
                    .Select(x => new
                    {
                        SeId = x.StrElement.Id,
                        x.Stage2Version.Stage3Version.CorrectYear
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.CorrectYear)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.SeId));

            var result = new List<JobYears>();

            foreach (var record in stage1Data)
            {
                var year = record.Key;

                foreach (var seId in record.Value)
                {
                    if (!jobs.ContainsKey(seId))
                    {
                        continue;
                    }

                    foreach (var job in jobs[seId])
                    {
                        result.Add(new JobYears
                        {
                            Job = job,
                            Year = year
                        });
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        public IQueryable<RealityObjectDpkrInfo> GetDpkrDataAboveYear(Municipality municipality, int year)
        {
#warning временная заглушка, реализовать
            return new List<RealityObjectDpkrInfo>().AsQueryable();
        }

        public IQueryable<RealityObject> GetRobjectsInProgram(Municipality municipality = null)
        {
            var serviceStage3 = this.Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage3>>();

            return this.Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .WhereIf(municipality != null, x => x.Municipality.Id == municipality.Id)
                .Where(y =>
                    serviceStage3.GetAll()
                        .WhereIf(municipality != null, x => x.RealityObject.Municipality.Id == municipality.Id)
                        .Any(x => x.RealityObject.Id == y.Id));
        }

        public Dictionary<long, int> GetPublishYear(long roId, int minYear)
        {
            var stage2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            using (this.Container.Using(stage2Domain))
            {
                return stage2Domain.GetAll()
                .Where(x => x.Stage3Version.RealityObject.Id == roId)
                .Where(x => x.Stage3Version.ProgramVersion.IsMain && x.Stage3Version.ProgramVersion.State.FinalState)
                .Where(x => x.Stage3Version.CorrectYear >= minYear)
                .Select(x => new
                {
                    CeoId = x.CommonEstateObject.Id,
                    Year = x.Stage3Version.CorrectYear,
                })
                .AsEnumerable()
                .GroupBy(x => x.CeoId)
                .ToDictionary(x => x.Key, y => y.SafeMin(x => x.Year));
            }
        }

        /// <inheritdoc />
        public Dictionary<long, int> GetAdjustedYear(long roId)
        {
            return new Dictionary<long, int>();
        }
    }
}