namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.Entities;

    // такую пустышку делаю чтобы в регионах заменять , но для этог онад очтобы именно она была зарегистрирована в основном модуле
    public class PrescriptionDomainService : PrescriptionDomainService<Prescription>
    {
        // Внимание !! Код override нужно писать не в этом классе а в ActCheckDomainService<T>
    }

    // Такую фигню делаю чтобы в модулях регионов расширять сущность ActCheck 
    public class PrescriptionDomainService<T> : BaseDomainService<T>
        where T : Prescription
    {
        
    }
}