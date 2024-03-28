namespace Bars.Gkh.Overhaul.Tat.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
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
            var repository = this.Container.Resolve<IRepository<PropertyOwnerProtocols>>();
            var viewAccOwnershipHistoryRepository = this.Container.Resolve<IViewAccOwnershipHistoryRepository>();

            using (this.Container.Using(repository, viewAccOwnershipHistoryRepository))
            {
                var persAccQuery = viewAccOwnershipHistoryRepository.GetAllDto(this.FilterService.PeriodId)
                    .WhereIfContainsBulked(this.FilterService.PersAccIds.Count > 0, x => x.Id, this.FilterService.PersAccIds)
                    .Filter(this.FilterService.PersAccFilter, this.Container);

                var query = this.FilterByEditDate(repository.GetAll())
                    .Where(x => persAccQuery.Any(y => y.RoId == x.RealityObject.Id));

                return this.GetProxies(query).ToDictionary(x => x.Id);
            }
        }

        /// <inheritdoc />
        protected override ICollection<NpaProxy> GetAdditionalCache()
        {
            var repository = this.Container.Resolve<IRepository<PropertyOwnerProtocols>>();

            using (this.Container.Using(repository))
            {
                var query = repository.GetAll()
                    .WhereContainsBulked(x => x.Id, this.AdditionalIds);

                return this.GetProxies(query);
            }
        }

        protected virtual IList<NpaProxy> GetProxies(IQueryable<PropertyOwnerProtocols> query)
        {
            return query.Fetch(x => x.NpaFile)
                .Where(x => x.TypeProtocol == PropertyOwnerProtocolType.ResolutionOfTheBoard)
                .Select(x => new
                {
                    x.Id,
                    x.NpaName,
                    x.NpaDate,
                    x.NpaNumber,
                    AuthName = x.NpaContragent.Name,
                    InfoType = x.TypeInformationNpa.Code,
                    ActType = x.TypeNpa.Code,
                    ActKind = x.TypeNormativeAct.Code,
                    ContragentId = (long?) x.NpaContragent.ExportId,
                    x.NpaFile,
                    x.NpaStatus,
                    x.NpaCancellationReason
                })
                .AsEnumerable()
                .Select(x => new NpaProxy
                {
                    Id = x.Id,
                    AuthLevel = 3, // Муниципальный
                    Name = x.NpaName,
                    DocumentDate = x.NpaDate,
                    Number = x.NpaNumber,
                    AuthName = x.AuthName,
                    InfoType = x.InfoType,
                    ActType = x.ActType,
                    ActKind = x.ActKind,
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