namespace Bars.GkhGji.Regions.Smolensk.DomainService
{
    using Bars.GkhGji.Enums;

    public class ProtocolDefinitionService : Bars.GkhGji.DomainService.ProtocolDefinitionService
    {
        public override TypeDefinitionProtocol[] DefinitionTypes()
        {
            // Вообщем по умолчанию регистрируются только такие типы 
            // в слуаче если в регионе нобходимы другие, то тогда заменяем реализацию
            return new []
                {
                    TypeDefinitionProtocol.DenialPetition,
                    TypeDefinitionProtocol.ReturnProtocol, 
                    TypeDefinitionProtocol.TransferCase, 
                    TypeDefinitionProtocol.TermAdministrativeInfraction
                };
        }
    }
}