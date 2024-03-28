namespace Bars.Gkh.Overhaul.Tat.FormatDataExport.ProxySelectors.Impl
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
    using Bars.Gkh.Overhaul.Tat.Entities;
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
            var recordRepository = this.Container.ResolveRepository<VersionRecordStage2>();
            var programVersionRepository = this.Container.ResolveRepository<ProgramVersion>();
            var typeWorkCrRepository = this.Container.ResolveRepository<TypeWorkCr>();

            using (this.Container.Using(recordRepository, programVersionRepository, typeWorkCrRepository))
            {
                var municipalityIds = this.FilterService.ProgramVersionFilter.Filter.GetAs<long[]>("municipalityId");

                var objectCrQuery = this.FilterService.GetFiltredQuery<ObjectCr>();
                var programVersionQuery = this.FilterService.ProgramVersionIds.Any()
                    ? programVersionRepository.GetAll()
                        .WhereContainsBulked(x => x.Id, this.FilterService.ProgramVersionIds)
                    : programVersionRepository.GetAll()
                        .WhereIfContainsBulked(municipalityIds.Any(), x => x.Municipality.Id, municipalityIds)
                        .Filter(this.FilterService.ProgramVersionFilter, this.Container);

                var longTermTypeWorks = recordRepository.GetAll()
                    .Where(x => programVersionQuery.Any(y => y == x.Stage3Version.ProgramVersion))
                    .Where(x => x.Stage3Version.ProgramVersion.IsMain)
                    .Select(x => new
                    {
                        x.Id,
                        ProgramId = x.Stage3Version.ProgramVersion.GetId(),
                        HouseId = x.Stage3Version.RealityObject.GetId(),
                        WorkKprTypeId = x.CommonEstateObject.GetNullableId(),
                        x.Stage3Version.Year
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var startDate = x.Year > 0 ? new DateTime(x.Year, 1, 1) : default(DateTime?);
                        var endDate = startDate?.AddMonths(12).AddDays(-1);
                        return new TypeWorkCrDto
                        {
                            Id = x.Id,
                            PkrDomId = UniqueIdTool.GetId(x.ProgramId, x.HouseId),
                            WorkKprTypeId = x.WorkKprTypeId,
                            StartDate = startDate,
                            EndDate = endDate,
                            IsLongTerm = true
                        };
                    });

                return this.GetProxies(typeWorkCrRepository.GetAll()
                        .Where(x => x.Work.TypeWork == TypeWork.Work)
                        .Where(x => objectCrQuery.Any(y => y == x.ObjectCr))
                        .Select(x => new TypeWorkCrDto
                        {
                            Id = x.Id,
                            PkrDomId = x.ObjectCr.GetId(),
                            WorkKprTypeId = x.Work.GetNullableId(),
                            StartDate = x.DateStartWork,
                            EndDate = x.DateEndWork
                        })
                        .AsEnumerable()
                        .Union(longTermTypeWorks)
                        .ToList())
                    .ToDictionary(x => x.Id);
            }
        }
    }
}