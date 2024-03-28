namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Resolution
{
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;

    /// <summary>
    /// Замена IDomainService<ResolutionPayFine> на IDomainService<TatarstanResolutionPayFine>
    /// </summary>
    public class ReplacementResolutionPayFineService : ReplacementDomainService<ResolutionPayFine, TatarstanResolutionPayFine>
    {
        
    }
}