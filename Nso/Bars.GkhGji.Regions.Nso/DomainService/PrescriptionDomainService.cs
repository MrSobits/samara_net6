namespace Bars.GkhGji.Regions.Nso.DomainService
{
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Nso.Entities;

    /// <summary>
    /// Данный класс служит заменой домен сервиса IDomainService<Prescription> на IDomainService<NsoPrescription>
    /// </summary>
    public class ReplacementPrescriptionDomainService : ReplacementDomainService<Prescription, NsoPrescription>
    {
    }

}
