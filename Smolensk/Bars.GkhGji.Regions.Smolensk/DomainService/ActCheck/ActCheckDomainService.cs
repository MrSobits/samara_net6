using Bars.Gkh.DomainService;
using Bars.GkhGji.Regions.Smolensk.Entities;

namespace Bars.GkhGji.Regions.Smolensk.DomainService
{
    /// <summary>
    /// Данный класс служит заменой домен сервиса IDomainService<ActCheck> на IDomainService<ActCheckSmol>
    /// </summary>
    public class ActCheckSmolDomainService : ReplacementDomainService<Bars.GkhGji.Entities.ActCheck, ActCheckSmol>
    {
    }
}
