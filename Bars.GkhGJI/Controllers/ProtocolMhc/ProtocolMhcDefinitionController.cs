namespace Bars.GkhGji.Controllers
{

    using Bars.GkhGji.Entities;

    public class ProtocolMhcDefinitionController : ProtocolMhcDefinitionController<ProtocolMhcDefinition>
    {
        // Внимание все методы писать в Generic
    }

    public class ProtocolMhcDefinitionController<T> : B4.Alt.DataController<T>
        where T : ProtocolMhcDefinition
    {
        
    }
}