namespace Bars.Gkh.Overhaul.Hmao.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Overhaul.FormatDataExport.ProxySelectors.Impl;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.EntityExtensions;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Селектор Программ капитального ремонта (pkrdomwork.csv)
    /// </summary>
    public class PkrDomWorkSelectorService : BasePkrDomWorkSelectorService
    {
        /// <inheritdoc />
        protected override IDictionary<long, PkrDomWorkProxy> GetCache()
        {
            var programVersionRepository = this.Container.ResolveRepository<ProgramVersion>();
            var typeWorkCrRepository = this.Container.ResolveRepository<TypeWorkCr>();
            var publishedProgramRecord = this.Container.ResolveRepository<PublishedProgramRecord>();
            var overhaulHmaoConfig = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var versionRecordStage1Repository = this.Container.ResolveRepository<VersionRecordStage1>();

            using (this.Container.Using(programVersionRepository,
                typeWorkCrRepository,
                publishedProgramRecord,
                overhaulHmaoConfig,
                versionRecordStage1Repository))
            {
                var municipalityIds = this.FilterService.ProgramVersionFilter.Filter.GetAs<long[]>("municipalityId");
                var objectCrQuery = this.FilterService.GetFiltredQuery<ObjectCr>();
                var yearPeriods = GetPeriods(overhaulHmaoConfig);

                var programVersionQuery = this.FilterService.ProgramVersionIds.Any()
                    ? programVersionRepository.GetAll()
                        .WhereContainsBulked(x => x.Id, this.FilterService.ProgramVersionIds)
                    : programVersionRepository.GetAll()
                        .WhereIfContainsBulked(municipalityIds.Any(), x => x.Municipality.Id, municipalityIds)
                        .Filter(this.FilterService.ProgramVersionFilter, this.Container);

                var longTermTypeWorks = publishedProgramRecord.GetAll()
                    .Where(x => programVersionQuery.Any(y => y == x.Stage2.Stage3Version.ProgramVersion))
                    .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                    .Join(versionRecordStage1Repository.GetAll(),
                        first => first.Stage2,
                        second => second.Stage2Version,
                        (first, second) => new
                        {
                            RealityObjectId = first.RealityObject.GetId(),
                            first.Stage2,
                            ProgramId = 1,
                            first.PublishedYear,
                            StructuralElementExportId = second.StructuralElement.StructuralElement.GetNullableId()
                        })
                    .AsEnumerable()
                    .GroupBy(x => new { x.RealityObjectId, x.StructuralElementExportId, x.PublishedYear, x.ProgramId })
                    .Select(x => new
                    {
                        x.Key.RealityObjectId,
                        x.Key.StructuralElementExportId,
                        x.Key.PublishedYear,
                        x.Key.ProgramId
                    })
                    .GroupBy(x => new { x.RealityObjectId, x.StructuralElementExportId })
                    .SelectMany(s =>
                    {
                        var index = 0;
                        var list = new List<TypeWorkCrDto>();

                        foreach (var f in s.OrderBy(o => o.PublishedYear))
                        {
                            list.Add(new TypeWorkCrDto
                            {
                                Id = long.Parse(string.Format($"{f.RealityObjectId}{f.StructuralElementExportId:D3}{++index:D2}")),
                                PkrDomId = UniqueIdTool.GetId(f.ProgramId, f.RealityObjectId),
                                WorkKprTypeId = f.StructuralElementExportId,
                                StartDate = yearPeriods[f.PublishedYear].Item1,
                                EndDate = yearPeriods[f.PublishedYear].Item2,
                                IsLongTerm = true
                            });
                        }

                        return list;
                    });

                var typeWorkCrDtoList = typeWorkCrRepository.GetAll()
                    .Where(x => x.Work.TypeWork == TypeWork.Work)
                    .Where(x => objectCrQuery.Any(y => y == x.ObjectCr))
                    .Select(x => new TypeWorkCrDto
                    {
                        Id = x.Id,
                        PkrDomId = x.ObjectCr.GetNullableId(),
                        WorkKprTypeId = x.Work.GetNullableId(),
                        StartDate = x.DateStartWork,
                        EndDate = x.DateEndWork
                    })
                    .AsEnumerable()
                    .Union(longTermTypeWorks)
                    .ToList();

                return this.GetProxies(typeWorkCrDtoList)
                    .ToDictionary(x => x.Id);
            }
        }

        private Dictionary<int,Tuple<DateTime, DateTime>> GetPeriods(OverhaulHmaoConfig overhaulHmaoConfig)
        {
            var programPeriodStart = overhaulHmaoConfig.ProgrammPeriodStart;
            var programPeriodEnd = overhaulHmaoConfig.ProgrammPeriodEnd;
            var period = 3;
            var dictDates = new Dictionary<int, Tuple<DateTime, DateTime>>();

           for (var currentYear = programPeriodStart; currentYear <= programPeriodEnd; currentYear++)
           {
               programPeriodStart = currentYear < programPeriodStart + period ? programPeriodStart : programPeriodStart + period;
               dictDates.Add(currentYear,new Tuple<DateTime, DateTime>(new DateTime(programPeriodStart, 1, 1), new DateTime(programPeriodStart + period, 1, 1).AddDays(-1)));
           }

           return dictDates;
        }
    }
}