namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.Utils;

    using NHibernate.Linq;

    /// <summary>
    /// Сервис получения <see cref="NpaProxy"/>
    /// </summary>
    public class NpaSelectorService : BaseProxySelectorService<NpaProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, NpaProxy> GetCache()
        {
            var govDecisionRepository = this.Container.Resolve<IRepository<GovDecision>>();
            var viewAccOwnershipHistoryRepository = this.Container.Resolve<IViewAccOwnershipHistoryRepository>();

            using (this.Container.Using(govDecisionRepository, viewAccOwnershipHistoryRepository))
            {
                var persAccQuery = viewAccOwnershipHistoryRepository.GetAllDto(this.FilterService.PeriodId)
                    .WhereIfContainsBulked(this.FilterService.PersAccIds.Count > 0, x => x.Id, this.FilterService.PersAccIds)
                    .Filter(this.FilterService.PersAccFilter, this.Container);

                var query = this.FilterByEditDate(govDecisionRepository.GetAll())
                    .Where(x => persAccQuery.Any(y => y.RoId == x.RealityObject.Id));

                return this.GetProxies(query).ToDictionary(x => x.Id);
            }
        }

        /// <inheritdoc />
        protected override ICollection<NpaProxy> GetAdditionalCache()
        {
            var govDecisionRepository = this.Container.Resolve<IRepository<GovDecision>>();

            using (this.Container.Using(govDecisionRepository))
            {
                var query = govDecisionRepository.GetAll()
                    .WhereContainsBulked(x => x.ExportId, this.AdditionalIds);

                return this.GetProxies(query);
            }
        }

        protected virtual IList<NpaProxy> GetProxies(IQueryable<GovDecision> query)
        {
            return query.Fetch(x => x.NpaFile)
                .Select(x => new
                {
                    x.ExportId,
                    x.NpaName,
                    x.NpaDate,
                    x.NpaNumber,
                    AuthName = x.NpaContragent.Name,
                    InfoTypeId = x.TypeInformationNpa.Code,
                    ActTypeId = x.TypeNpa.Code,
                    ActKindId = x.TypeNormativeAct.Code,
                    ContragentId = (long?) x.NpaContragent.ExportId,
                    x.NpaFile,
                    x.NpaStatus,
                    x.NpaCancellationReason
                })
                .AsEnumerable()
                .Select(x => new NpaProxy
                {
                    Id = x.ExportId,
                    AuthLevel = 2, // Региональный
                    Name = x.NpaName,
                    DocumentDate = x.NpaDate,
                    Number = x.NpaNumber,
                    AuthName = x.AuthName,
                    InfoType = x.InfoTypeId,
                    ActType = x.ActTypeId,
                    ActKind = x.ActKindId,
                    ContragentId = x.ContragentId,
                    StartDate = x.NpaDate,
                    IsThroughoutTerritoryValid = 1,
                    File = x.NpaFile,
                    Status = x.NpaStatus.ToIntNullable(),
                    TerminationReason = x.NpaCancellationReason
                })
                .ToList();
        }
    }
}
