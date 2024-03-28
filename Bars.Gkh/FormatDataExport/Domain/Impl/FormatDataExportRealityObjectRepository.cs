namespace Bars.Gkh.FormatDataExport.Domain.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.Utils;
    using B4.DataAccess;
    using Domain;
    using B4.IoC;
    using B4.Utils;

    public class FormatDataExportRealityObjectRepository : BaseFormatDataExportRepository<RealityObject>
    {
        /// <inheritdoc />
        public override IQueryable<RealityObject> GetQuery(IFormatDataExportFilterService filterService)
        {
            var roQuery = this.Repository.GetAll();

            var isSelectByIds = filterService.RealityObjectIds.Any();

            var query = isSelectByIds 
                ? roQuery.WhereContainsBulked(x => x.Id, filterService.RealityObjectIds)
                : filterService.FilterByMunicipality(roQuery, x => x.Municipality);

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

            if (filterService.HasContragentFilter)
            {
                if (filterService.Provider == FormatDataExportProviderType.Uo)
                {
                    query = this.FilterByContragent<ManOrgContractRealityObject>(filterService,
                        x => x.ManOrgContract.ManagingOrganization.Contragent,
                        x => x.RealityObject);
                }
            }

            return this.FilterByEditDate(query, filterService);
        }

        public override IDataResult List(BaseParams baseParams)
        {
            var contragentId = baseParams.Params.GetAs<long>("contragentId");

            var serviceManOrgContractRobject = this.Container.ResolveDomain<ManOrgContractRealityObject>();

            using (this.Container.Using(serviceManOrgContractRobject))
            {
                return this.Repository.GetAll()
                    .WhereIf(contragentId > 0,
                        y => serviceManOrgContractRobject.GetAll()
                            .Where(x => x.ManOrgContract.ManagingOrganization.Contragent.Id == contragentId)
                            .Any(x => x.RealityObject.Id == y.Id))
                            .Select(x => new
                            {
                                x.Id,
                                x.Address,
                                Municipality = x.Municipality.Name,
                                x.TypeHouse,
                                FiasHauseGuid = x.FiasAddress.HouseGuid.ToString()
                            })
                    .ToListDataResult(baseParams.GetLoadParam());
            }
        }
    }
}