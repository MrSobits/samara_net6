namespace Bars.Gkh.RegOperator.FormatDataExport.Domain.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.Gkh.FormatDataExport.Domain.Impl;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.Utils;

    public class FormatDataExportRealityObjectRegopRepository : FormatDataExportRealityObjectRepository
    {
        /// <inheritdoc />
        public override IQueryable<RealityObject> GetQuery(IFormatDataExportFilterService filterService)
        {
            var viewAccOwnershipHistoryRepository = this.Container.Resolve<IViewAccOwnershipHistoryRepository>();
            using (this.Container.Using(viewAccOwnershipHistoryRepository))
            {
                var roQuery = base.GetQuery(filterService);

                var isSelectByIds = filterService.RealityObjectIds.Any();

                var query = filterService.FilterByMunicipality(roQuery, x => x.Municipality)
                    .WhereIfContainsBulked(isSelectByIds, x => x.Id, filterService.RealityObjectIds);

                if (!isSelectByIds)
                {
                    query = query
                        .Select(x => new
                        {
                            x.Id,
                            x.Address,
                            x.Municipality,
                            x.TypeHouse,
                            FiasHauseGuid = x.FiasAddress.HouseGuid,
                            RealityObject = x
                        })
                        .Filter(filterService.RealityObjectFilter, this.Container)
                        .Select(x => x.RealityObject);
                }

                if (filterService.HasPersAccFilter)
                {
                    var roIdQuery = viewAccOwnershipHistoryRepository.GetAllDto(filterService.PeriodId)
                        .WhereIfContainsBulked(filterService.PersAccIds.Count > 0, x => x.Id, filterService.PersAccIds)
                        .Filter(filterService.PersAccFilter, this.Container)
                        .Select(x => x.RoId);

                    query = query.Where(x => roIdQuery.Any(r => r == x.Id));
                }

                return query;
            }
        }
    }
}