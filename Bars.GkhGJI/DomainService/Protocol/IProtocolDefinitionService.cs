namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.Enums;

    public interface IProtocolDefinitionService
    {
        IDataResult ListTypeDefinition(BaseParams baseParams);

        TypeDefinitionProtocol[] DefinitionTypes();
    }
}