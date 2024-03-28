namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol;

    /// <summary>
    /// Данный класс служит заменой домен сервиса IDomainService<Protocol> на IDomainService<ChelyabinskProtocol>
    /// </summary>
    public class ReplacementProtocolDomainService : ReplacementDomainService<Protocol, ChelyabinskProtocol>
    {
    }
}
