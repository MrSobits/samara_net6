namespace Bars.GkhGji.Regions.Tomsk.DomainService
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
                    TypeDefinitionProtocol.TimeAndPlaceHearing,
                    TypeDefinitionProtocol.ReturnProtocol, 
                    TypeDefinitionProtocol.PostponeCase, 
                    TypeDefinitionProtocol.About,
                    TypeDefinitionProtocol.TransferCase,
                    TypeDefinitionProtocol.DenialPetition,
                    TypeDefinitionProtocol.CorrectionMisprint,
                    TypeDefinitionProtocol.ReclamationInformation,
                    TypeDefinitionProtocol.TermAdministrativeInfraction
                };
        }
    }
}