namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Resolution
{
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;

    /// <summary>
    /// Замена IDomainService<Resolution> на IDomainService<TatarstanResolution>
    /// </summary>
    public class ReplacementResolutionDomainService : ReplacementDomainService<Resolution, TatarstanResolution>
    {
        
    }
}