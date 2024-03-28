namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Disposal
{
    using System.Linq;

    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    public class DisposalDomainService : ReplacementDomainService<Bars.GkhGji.Entities.Disposal, TatarstanDisposal>
    {
        /// <inheritdoc />
        public override IQueryable<GkhGji.Entities.Disposal> GetAll()
        {
            // Ограничить все существующие запросы
            return base.GetAll().Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal);
        }
    }
}
