namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;

    /// <summary>
    /// Данный класс служит заменой домен сервиса IDomainService<ActCheck> на IDomainService<ChelyabinskActCheck>
    /// </summary>
    public class ReplacementActCheckDomainService : ReplacementDomainService<ActCheck, ChelyabinskActCheck>
    {
    }

}
