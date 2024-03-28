namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Gkh.Entities;
    using Overhaul.DomainService;
    using Overhaul.Entities;
    using Entities;
    using Castle.Windsor;
    using Bars.Gkh.Domain.CollectionExtensions;

    /// <inheritdoc />
    public class NsoRealityObjectDpkrDataService : IRealityObjectDpkrDataService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgrammStage3> CorrectionDomain { get; set; }

        /// <inheritdoc />
        public List<JobYears> GetWorkInfoByRealityObject(RealityObject ro)
        {
            if (ro == null || ro.Id == 0)
            {
                return new List<JobYears>();
            }

            var publQuery = this.Container.Resolve<IDomainService<PublishedProgramRecord>>().GetAll()
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                .Where(x => x.Stage2.Stage3Version.RealityObject.Id == ro.Id);

            var stage1Query = this.Container.Resolve<IDomainService<VersionRecordStage1>>().GetAll()
                .Where(x => x.StructuralElement.RealityObject.Id == ro.Id)
                .Where(x => publQuery.Any(y => y.Stage2.Id == x.Stage2Version.Id));

            var jobs = this.Container.Resolve<IDomainService<StructuralElementWork>>().GetAll()
                .Where(x => stage1Query.Any(y => y.StructuralElement.StructuralElement.Id == x.StructuralElement.Id))
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
                        SeId = x.StructuralElement.StructuralElement.Id,
                        Stage2Id = x.Stage2Version.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Stage2Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.SeId));

            var publData =
                publQuery
                    .Select(x => new
                    {
                        Stage2Id = x.Stage2.Id,
                        x.PublishedYear
                    });

            var result = new List<JobYears>();

            foreach (var record in publData)
            {
                var year = record.PublishedYear;

                if (!stage1Data.ContainsKey(record.Stage2Id))
                {
                    continue;
                }

                foreach (var seId in stage1Data[record.Stage2Id])
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
            return this.Container.ResolveDomain<PublishedProgramRecord>().GetAll()
                .Where(x => x.PublishedYear > year)
                .Where(x => x.Stage2.Stage3Version.RealityObject.Municipality == municipality)
                .Select(x => new RealityObjectDpkrInfo
                {
                    RealityObject = x.Stage2.Stage3Version.RealityObject,
                    Year = x.PublishedYear,
                    Sum = x.Sum
                });
        }

        /// <inheritdoc />
        public IQueryable<RealityObject> GetRobjectsInProgram(Municipality municipality = null)
        {
            var serviceStage3 = this.Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage3>>();

            var stage3Query = serviceStage3.GetAll()
                .WhereIf(municipality != null, x => x.RealityObject.Municipality.Id == municipality.Id);

            return this.Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .WhereIf(municipality != null, x => x.Municipality.Id == municipality.Id)
                .Where(y => stage3Query.Any(x => x.RealityObject.Id == y.Id));
        }

        /// <inheritdoc />
        public Dictionary<long, int> GetPublishYear(long roId, int minYear)
        {
            var publishedProgramRecDomain = this.Container.ResolveDomain<PublishedProgramRecord>();
            using (this.Container.Using(publishedProgramRecDomain))
            {
                return publishedProgramRecDomain.GetAll()
                .Where(x => x.Stage2.Stage3Version.RealityObject.Id == roId)
                .Where(x => x.PublishedProgram.ProgramVersion.IsMain && x.PublishedYear >= minYear)
                .Select(x => new
                {
                    CeoId = x.Stage2.CommonEstateObject.Id,
                    Year = x.PublishedYear,
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