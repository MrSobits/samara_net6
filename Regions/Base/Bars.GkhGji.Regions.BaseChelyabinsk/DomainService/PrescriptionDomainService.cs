namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Prescription;

    /// <summary>
    /// Данный класс служит заменой домен сервиса IDomainService<Prescription> на IDomainService<ChelyabinskPrescription>
    /// </summary>
    public class ReplacementPrescriptionDomainService : ReplacementDomainService<Prescription, ChelyabinskPrescription>
    {
    }

}
