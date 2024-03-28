namespace Bars.GkhGji.Controllers
{
    using Bars.GkhGji.Entities;

    public class ActCheckWitnessController : ActCheckWitnessController<ActCheckWitness>
    {
    }

    // Класс переделан на для того чтобы в регионах можно было расширят ьсущность через subclass 
    // и при этом не писать дублирующий серверный код
    public class ActCheckWitnessController<T> : B4.Alt.DataController<T>
        where T : ActCheckWitness
    {

    }
}
