namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="UoProxy"/>
    /// </summary>
    public class UoSelectorService : BaseProxySelectorService<UoProxy>
    {
        protected override ICollection<UoProxy> GetAdditionalCache()
        {
            var managingOrganizationRepository = this.Container.ResolveRepository<ManagingOrganization>();

            using (this.Container.Using(managingOrganizationRepository))
            {
                var query = managingOrganizationRepository.GetAll()
                    .WhereContainsBulked(x => x.Contragent.ExportId, this.AdditionalIds);

                return this.GetData(query);
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, UoProxy> GetCache()
        {
            var managingOrganizationRepository = this.Container.ResolveRepository<ManagingOrganization>();

            using (this.Container.Using(managingOrganizationRepository))
            {
                var byContragent = this.FilterService
                    .FilterByContragent(managingOrganizationRepository.GetAll(), x => x.Contragent);
                var query = this.FilterService
                    .FilterByMunicipality(byContragent, x => x.Contragent.Municipality);

                return this.GetData(query)
                    .ToDictionary(x => x.Id);
            }
        }

        protected virtual ICollection<UoProxy> GetData(IQueryable<ManagingOrganization> query)
        {
            var contragentContactRepository = this.Container.ResolveRepository<ContragentContact>();
            using (this.Container.Using(contragentContactRepository))
            {
                var leaderId = this.SelectParams.GetAsId("LeaderPositionId");

                var contactDict = contragentContactRepository.GetAll()
                    .Where(x => x.Position.Id == leaderId)
                    .Select(x => new
                    {
                        Id = x.Contragent.ExportId,
                        x.Phone
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => x.Phone)
                    .ToDictionary(x => x.Key, x => x.First());

                return query
                    .Select(x => new
                    {
                        Id = x.Contragent.ExportId,
                        x.NumberEmployees,
                        x.ShareSf,
                        x.ShareMo,
                        x.TypeManagement
                    })
                    .AsEnumerable()
                    .Select(x => new UoProxy
                    {
                        Id = x.Id,
                        LeaderPhone = contactDict.Get(x.Id),
                        AdministrativeStaffCount = x.NumberEmployees,
                        ShareSf = x.ShareSf,
                        ShareMo = x.ShareMo,
                        IsTsj = x.TypeManagement == TypeManagementManOrg.TSJ || x.TypeManagement == TypeManagementManOrg.JSK
                            ? 1
                            : 2
                    })
                    .ToList();
            }
        }
    }
}