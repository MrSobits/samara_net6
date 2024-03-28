namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.Entities;

    // такую пустышку делаю потмоучто  врегионе необходимо заменять
    public class PrescriptionCancelDomainService : PrescriptionCancelDomainService<PrescriptionCancel>
    {
        // Внимание !! Код override нужно писать не в этом классе а в Generic
    }

    // Такую фигню делаю чтобы в модулях регионов расширять сущность PrescriptionCancel 
    public class PrescriptionCancelDomainService<T> : BaseDomainService<T>
        where T : PrescriptionCancel
    {
        
    }
}