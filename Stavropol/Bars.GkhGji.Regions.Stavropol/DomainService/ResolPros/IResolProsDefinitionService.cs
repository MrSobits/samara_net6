using Bars.B4;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.Stavropol.Enums;

namespace Bars.GkhGji.Regions.Stavropol.DomainService.ResolPros
{
	public interface IResolProsDefinitionService
    {
        IDataResult ListTypeDefinition(BaseParams baseParams);
        TypeDefinitionResolPros[] DefinitionTypes();
    }
}