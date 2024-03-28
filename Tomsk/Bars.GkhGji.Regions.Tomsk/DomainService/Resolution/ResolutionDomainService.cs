namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    /// <summary>
    /// Данный класс служит заменой домен сервиса IDomainService<Resolution> на IDomainService<TomskResolution>
    /// </summary>
    public class ReplacementResolutionDomainService : ReplacementDomainService<Resolution, TomskResolution>
    {
    }
}
