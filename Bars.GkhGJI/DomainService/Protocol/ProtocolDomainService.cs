namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.Entities;

    // такую пустышку делаю чтобы в регионах заменять , но для этого надо чтобы именно она была зарегистрирована в основном модуле
    public class ProtocolDomainService : ProtocolDomainService<Protocol>
    {
        // Внимание !! Код override нужно писать не в этом классе а в ProtocolDomainService<T>
    }

    //Такую фигню делаю чтобы в модулях регионов расширять сущность Protocol
    public class ProtocolDomainService<T> : BaseDomainService<T>
        where T: Protocol
    {
        
    }
}