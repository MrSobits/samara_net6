namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.Enums;

    public interface IResolutionDefinitionService
    {
        IDataResult ListTypeDefinition(BaseParams baseParams);

        TypeDefinitionResolution[] DefinitionTypes();
    }
}