namespace Bars.GkhGji.FormatDataExport.Domain.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class FormatDataExportInspectionRepository : BaseFormatDataExportRepository<ViewFormatDataExportInspection>
    {
        public override IQueryable<ViewFormatDataExportInspection> GetQuery(IFormatDataExportFilterService filterService)
        {
            var repository = this.Container.Resolve<IViewInspectionRepository>();
            using (this.Container.Using(repository))
            {
                var filter = filterService.InspectionFilter.Filter;
                var startDate = filter.GetAs<DateTime?>("StartDate");
                var endDate =filter.GetAs<DateTime?>("EndDate");
                var auditType = filter.GetAs<AuditType>("AuditType");

                var query = repository.GetAll()
                    .WhereIf(startDate.IsValid(), x => x.DocumentDate >= startDate)
                    .WhereIf(endDate.IsValid(), x => x.DocumentDate <= endDate)
                    .WhereIf(auditType == AuditType.Planned, x => x.IsPlanned)
                    .WhereIf(auditType == AuditType.NotPlanned, x => !x.IsPlanned);

                return filterService.InspectionIds.Any()
                    ? query.WhereContainsBulked(x => x.Id, filterService.InspectionIds)
                    : query.Filter(filterService.InspectionFilter, this.Container);
            }
        }
    }
}