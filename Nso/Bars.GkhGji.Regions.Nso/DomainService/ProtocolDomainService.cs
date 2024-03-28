namespace Bars.GkhGji.Regions.Nso.DomainService
{
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Nso.Entities;

    /// <summary>
    /// Данный класс служит заменой домен сервиса IDomainService<Protocol> на IDomainService<NsoProtocol>
    /// </summary>
    public class ReplacementProtocolDomainService : ReplacementDomainService<Protocol, NsoProtocol>
    {
    }
}
