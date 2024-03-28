namespace Bars.Gkh.Overhaul.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="BankProxy"/>
    /// </summary>
    public class BankSelectorService : BaseProxySelectorService<BankProxy>
    {
        /// <inheritdoc />
        protected override ICollection<BankProxy> GetAdditionalCache()
        {
            var repository = this.Container.ResolveRepository<CreditOrg>();
            using (this.Container.Using(repository))
            {
                var query = repository.GetAll()
                    .WhereContainsBulked(x => x.ExportId, this.AdditionalIds);

                return this.GetProxies(query);
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, BankProxy> GetCache()
        {
            var repository = this.Container.ResolveRepository<CreditOrg>();
            using (this.Container.Using(repository))
            {
                var query = repository.GetAll()
                    .Where(x => (long?) x.Parent.Id == null);

                return this.GetProxies(query)
                    .ToDictionary(x => x.Id);
            }
        }

        protected ICollection<BankProxy> GetProxies(IQueryable<CreditOrg> query)
        {
            return query.Select(x => new BankProxy
                {
                    Id = x.ExportId,
                    Bik = x.Bik,
                    CorrAccount = x.CorrAccount
                })
                .ToList();
        }
    }
}