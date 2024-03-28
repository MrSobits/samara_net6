namespace Bars.GkhGji.Controllers
{

    using Bars.GkhGji.Entities;

    public class ProtocolRSODefinitionController : ProtocolRSODefinitionController<ProtocolRSODefinition>
    {
        // Внимание все методы писать в Generic
    }

    public class ProtocolRSODefinitionController<T> : B4.Alt.DataController<T>
        where T : ProtocolRSODefinition
    {
        
    }
}