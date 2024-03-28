namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Resolution
{
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;

    /// <summary>
    /// Замена IDomainService<ProtocolMvd> на IDomainService<TatarstanProtocolMvd>
    /// </summary>
    public class ReplacementProtocolMvdDomainService : ReplacementDomainService<ProtocolMvd, TatarstanProtocolMvd>
    {
        
    }
}