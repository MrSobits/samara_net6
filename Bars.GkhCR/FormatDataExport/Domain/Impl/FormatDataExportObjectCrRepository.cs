namespace Bars.GkhCr.FormatDataExport.Domain.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;

    public class FormatDataExportObjectCrRepository : BaseFormatDataExportRepository<ObjectCr>
    {
        /// <inheritdoc />
        public override IQueryable<ObjectCr> GetQuery(IFormatDataExportFilterService filterService)
        {
            var filter = filterService.ObjectCrFilter.Filter;
            var programCrIdList = filter.GetAs<long[]>("programCrIdList");
            var municipalityIdList = filter.GetAs<long[]>("municipalityIdList");
            var isSelectByIds = filterService.ObjectCrIds.Any();

            var objectCrQuery = this.Repository.GetAll()
                .WhereIfContainsBulked(programCrIdList.Any(), x => x.ProgramCr.Id, programCrIdList)
                .WhereIfContainsBulked(municipalityIdList.Any(), x => x.RealityObject.Municipality.Id, municipalityIdList)
                .WhereIfContainsBulked(isSelectByIds, x => x.Id, filterService.ObjectCrIds);

            if (!isSelectByIds)
            {
                objectCrQuery = objectCrQuery
                    .Select(x => new
                    {
                        x.Id,
                        x.ProgramNum,
                        RealityObjName = x.RealityObject.Address,
                        Municipality = x.RealityObject.Municipality.Name,
                        x.State,
                        ProgramCrName = x.ProgramCr.Name,
                        ObjectCr = x
                    })
                    .Filter(filterService.ObjectCrFilter, this.Container)
                    .Select(x => x.ObjectCr);
            }

            return objectCrQuery;
        }
    }
}