using Bars.Gkh.DomainService;
using Bars.GkhGji.Regions.Smolensk.Entities;

namespace Bars.GkhGji.Regions.Smolensk.DomainService
{
    /// <summary>
    /// Данный класс служит заменой домен сервиса IDomainService<Disposal> на IDomainService<DisposalSmol>
    /// </summary>
    public class DisposalSmolDomainService : ReplacementDomainService<Bars.GkhGji.Entities.Disposal, DisposalSmol>
    {
    }
}
