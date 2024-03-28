namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    /// <summary>
    /// Данный класс служит заменой домен сервиса IDomainService<Disposal> на IDomainService<ChelyabinskDisposal> ReplacementDomainService ReplacementFileStorageDomainService
    /// </summary>
    public class ReplacementDisposalDomainService : ReplacementDomainService<Disposal, ChelyabinskDisposal>
    {
    }

}
