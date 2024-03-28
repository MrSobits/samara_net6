namespace Bars.Gkh.DomainService
{
    using Bars.B4;
    using Bars.Gkh.Entities;

    // такую пустышку делаю чтобы в регионах заменять , но для этог онад очтобы именно она была зарегистрирована в основном модуле
    public class OperatorDomainService : OperatorDomainService<Operator>
    {
        // Внимание !! Код override нужно писать не в этом классе а в Generic
    }

    //Такую фигню делаю чтобы в модулях регионов расширять сущность Operator 
    public class OperatorDomainService<T> : BaseDomainService<T>
        where T : Operator
    {

    }
}