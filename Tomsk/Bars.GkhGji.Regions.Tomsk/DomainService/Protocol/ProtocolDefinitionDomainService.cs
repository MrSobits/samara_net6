namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    /// <summary>
    /// Данный класс служит заменой домен сервиса IDomainService<ProtocolDefinition> на IDomainService<TomskProtocolDefinition>
    /// </summary>
    public class ReplacementProtocolDefinitionDomainService : ReplacementDomainService<ProtocolDefinition, TomskProtocolDefinition>
    {
    }
}
