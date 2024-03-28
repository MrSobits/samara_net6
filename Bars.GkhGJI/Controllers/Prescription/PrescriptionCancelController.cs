namespace Bars.GkhGji.Controllers
{
    using Bars.GkhGji.Entities;

    // Класс наслучай если ктото от него наследвоался в регионах
    public class PrescriptionCancelController : PrescriptionCancelController<PrescriptionCancel>
    {
        //Внимание все методы добавлят ьи изменять в Generic Классе
    }

    // Generic Класс поскольку в регионе данная сущност ьрасширяется sublass'ом чтобы избежать дублирвоания и необходимо писат ьфункционал в этот касс
    public class PrescriptionCancelController<T> : B4.Alt.DataController<T>
        where T : PrescriptionCancel
    {
        
    }
}