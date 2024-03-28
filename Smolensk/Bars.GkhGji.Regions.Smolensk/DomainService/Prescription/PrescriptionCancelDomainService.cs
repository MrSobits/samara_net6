namespace Bars.GkhGji.Regions.Smolensk.DomainService
{

    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Smolensk.Entities;
    
    /// <summary>
    /// Домен заменяющий старый домен на новый для т
    /// </summary>
    public class PrescriptionCancelDomainService: ReplacementDomainService<PrescriptionCancel, PrescriptionCancelSmol>
    {
        
    }

}