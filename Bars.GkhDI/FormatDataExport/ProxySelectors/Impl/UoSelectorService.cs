namespace Bars.GkhDi.FormatDataExport.ProxySelectors.Impl
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
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Сервис получения <see cref="UoProxy"/>
    /// </summary>
    public class UoSelectorService : BaseProxySelectorService<UoProxy>
    {
        /// <inheritdoc />
        protected override ICollection<UoProxy> GetAdditionalCache()
        {
            var manOrgRepository = this.Container.ResolveRepository<ManagingOrganization>();
            using (this.Container.Using(manOrgRepository))
            {
                return this.GetProxy(manOrgRepository.GetAll()
                        .WhereContainsBulked(x => x.Contragent.ExportId, this.AdditionalIds))
                    .ToList();
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, UoProxy> GetCache()
        {
            var manOrgRepository = this.Container.ResolveRepository<ManagingOrganization>();
            using (this.Container.Using(manOrgRepository))
            {
                return this.GetProxy(this.FilterService
                        .FilterByContragent(manOrgRepository.GetAll(), x => x.Contragent))
                    .ToDictionary(x => x.Id);
            }
        }

        private IEnumerable<UoProxy> GetProxy(IQueryable<ManagingOrganization> manorgQuery, ICollection<long> ids = null)
        {
            var contragentContactRepository = this.Container.ResolveRepository<ContragentContact>();
            var disclosureInfoRepository = this.Container.ResolveRepository<DisclosureInfo>();

            using (this.Container.Using(contragentContactRepository, disclosureInfoRepository))
            {
                var leaderId = this.SelectParams.GetAsId("LeaderPositionId");

                var contactQuery = ids == null
                    ? this.FilterService
                        .FilterByContragent(contragentContactRepository.GetAll(), x => x.Contragent)
                    : contragentContactRepository.GetAll().WhereContainsBulked(x => x.Contragent.ExportId, ids);

                var contactDict = contactQuery
                    .Where(x => x.Position != null && x.Position.Id == leaderId)
                    .Select(x => new
                    {
                        Id = x.Contragent.ExportId,
                        x.Phone
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => x.Phone)
                    .ToDictionary(x => x.Key, x => x.First());

                var disclosureInfoQuery = ids == null
                    ? this.FilterService
                        .FilterByContragent(disclosureInfoRepository.GetAll(), x => x.ManagingOrganization.Contragent)
                    : disclosureInfoRepository.GetAll().WhereContainsBulked(x => x.ManagingOrganization.Contragent.ExportId, ids);

                var disclosureInfoDict = disclosureInfoQuery
                    .Select(x => new
                    {
                        Id = x.ManagingOrganization.Contragent.ExportId,
                        x.AdminPersonnel,
                        x.Engineer,
                        x.Work,
                        x.PeriodDi.DateStart
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.OrderByDescending(d => d.DateStart).First());

                return manorgQuery
                    .Select(x => new
                    {
                        Id = x.Contragent.ExportId,
                        x.NumberEmployees,
                        x.ShareSf,
                        x.ShareMo,
                        x.TypeManagement
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var diInfo = disclosureInfoDict.Get(x.Id);
                        return new UoProxy
                        {
                            Id = x.Id,
                            LeaderPhone = contactDict.Get(x.Id),
                            AdministrativeStaffCount = diInfo?.AdminPersonnel,
                            EngineersCount = diInfo?.Engineer,
                            EmployeesCount = diInfo?.Work,
                            ShareSf = x.ShareSf,
                            ShareMo = x.ShareMo,
                            IsTsj = x.TypeManagement == TypeManagementManOrg.TSJ || x.TypeManagement == TypeManagementManOrg.JSK
                                ? 1
                                : 2
                        };
                    });
            }
        }
    }
}