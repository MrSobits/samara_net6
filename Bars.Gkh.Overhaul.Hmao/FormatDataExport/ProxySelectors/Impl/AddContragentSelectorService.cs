namespace Bars.Gkh.Overhaul.Hmao.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Overhaul.Hmao.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Дополнительный поставщик информации (addcontragent.csv)
    /// </summary>
    public class AddContragentSelectorService : BaseProxySelectorService<AddContragentProxy>
    {
        /// <inheritdoc />
        protected override ICollection<AddContragentProxy> GetAdditionalCache()
        {
            var contragentRepository = this.Container.ResolveRepository<Contragent>();

            using (this.Container.Using(contragentRepository))
            {
                var query = contragentRepository.GetAll()
                    .WhereContainsBulked(x => x.ExportId, this.AdditionalIds);

                return this.GetProxies(query).ToList();
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, AddContragentProxy> GetCache()
        {
            var contragentRepository = this.Container.ResolveRepository<Contragent>();

            using (this.Container.Using(contragentRepository))
            {
                var query = this.FilterService.FilterByContragent(contragentRepository.GetAll());

                return this.GetProxies(query)
                    .ToDictionary(x => x.Id);
            }
        }

        protected IEnumerable<AddContragentProxy> GetProxies(IQueryable<Contragent> contragentQuery)
        {
            var contragentList = contragentQuery
                .Select(x => new
                {
                    x.ExportId,
                    MainRoleCode = x.MainRole.Code
                })
                .AsEnumerable()
                .Select(x => new AddContragentProxy
                {
                    Id = x.ExportId,
                    InformationProviderType = x.MainRoleCode
                })
                .ToList();

            return contragentList;
        }
    }
}