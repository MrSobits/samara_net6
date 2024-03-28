namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Disposal
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    public class TatarstanDisposalDomainService : BaseDomainService<TatarstanDisposal>
    {
        /// <inheritdoc />
        public override IQueryable<TatarstanDisposal> GetAll()
        {
            // Ограничить все существующие запросы
            return base.GetAll().Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal);
        }
    }
}
