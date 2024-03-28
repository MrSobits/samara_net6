namespace Bars.GkhGji.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    public class GjiSelectorService : BaseProxySelectorService<GjiProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, GjiProxy> GetCache()
        {
            var gjiContragentId = this.SelectParams.GetAsId("GjiContragentId");
            var contragentRepository = this.Container.ResolveRepository<Contragent>();

            using (this.Container.Using(contragentRepository))
            {
                return contragentRepository.GetAll()
                    .Where(x => x.Id == gjiContragentId)
                    .Select(x => new GjiProxy
                    {
                        Id = x.ExportId,
                        Type = 1,
                        FrguId = x.FrguRegNumber,
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}