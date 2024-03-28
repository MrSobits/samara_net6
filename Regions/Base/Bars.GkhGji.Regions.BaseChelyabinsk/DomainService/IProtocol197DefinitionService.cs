namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.Enums;

    public interface IProtocol197DefinitionService
    {
        IDataResult ListTypeDefinition(BaseParams baseParams);

        TypeDefinitionProtocol[] DefinitionTypes();
    }
}