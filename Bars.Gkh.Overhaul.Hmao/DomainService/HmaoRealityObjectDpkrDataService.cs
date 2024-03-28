namespace Bars.Gkh.Overhaul.Hmao.DomainService
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
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Castle.Windsor;

    /// <inheritdoc />
    public class HmaoRealityObjectDpkrDataService : IRealityObjectDpkrDataService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен Записей опубликованной программы
        /// </summary>
        public IDomainService<PublishedProgramRecord> PublishedProgramRecordDomain { get; set; }
        
        /// <summary>
        /// Домен Версиоинирование первого этапа
        /// </summary>
        public IDomainService<VersionRecordStage1> VersionRecordStage1Domain { get; set; }

        /// <summary>
        /// Домен Записей в версии программы
        /// </summary>
        public IDomainService<VersionRecord> VersionRecordStage3Domain { get; set; }

        /// <inheritdoc />
        public List<JobYears> GetWorkInfoByRealityObject(RealityObject ro)
        {
            var publQuery = this.Container.Resolve<IDomainService<PublishedProgramRecord>>().GetAll()
                .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                .Where(x => x.RealityObject.Id == ro.Id);

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
                .Where(x => x.Stage2 != null)
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
            var correctionDomainDomain = this.Container.ResolveDomain<DpkrCorrectionStage2>();
            
            using (this.Container.Using(correctionDomainDomain))
            {
                return correctionDomainDomain.GetAll()
                    .Where(x => x.RealityObject.Municipality == municipality && x.PlanYear > year)
                    .Select(x => new RealityObjectDpkrInfo
                    {
                        RealityObject = x.RealityObject,
                        Year = x.PlanYear,
                        Sum = x.Stage2.Sum
                    });
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public Dictionary<long, int> GetPublishYear(long roId, int minYear)
        {
            using (this.Container.Using(this.VersionRecordStage3Domain))
            {
                return this.VersionRecordStage3Domain.GetAll()
                    .Where(x => x.RealityObject.Id == roId)
                    .Where(x => x.ProgramVersion.IsMain && x.Year >= minYear)
                    .Join(this.VersionRecordStage1Domain.GetAll(),
                        x => x.Id,
                        y => y.Stage2Version.Stage3Version.Id,
                        (x, y) => new
                        {
                            y.StructuralElement.Id,
                            x.Year
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Max(x => x.Year));
            }
        }

        /// <inheritdoc />
        public Dictionary<long, int> GetAdjustedYear(long roId)
        {
            using (this.Container.Using(this.PublishedProgramRecordDomain, this.VersionRecordStage1Domain))
            {
                return this.PublishedProgramRecordDomain.GetAll()
                    .Where(x => x.RealityObject.Id == roId)
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                    .Where(x => x.Stage2 != null)
                    .Join(this.VersionRecordStage1Domain.GetAll(),
                        x => x.Stage2,
                        y => y.Stage2Version,
                        (x, y) => new
                        {
                            y.StructuralElement.Id,
                            Year = x.PublishedYear
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Max(x => x.Year));
            }    
        }
    }
}