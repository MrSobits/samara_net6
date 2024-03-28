namespace Bars.GkhGji.Regions.Tula.DomainService
{
    using Bars.GkhGji.Enums;

    public class TulaProtocolDefinitionService : Bars.GkhGji.DomainService.ProtocolDefinitionService
    {
        public override TypeDefinitionProtocol[] DefinitionTypes()
        {
            // Вообщем по умолчанию регистрируются только такие типы 
            // в слуаче если в регионе нобходимы другие, то тогда заменяем реализацию
            return new[]
                   {
                       TypeDefinitionProtocol.TimeAndPlaceHearing, 
                       TypeDefinitionProtocol.ReturnProtocol,
                       TypeDefinitionProtocol.PostponeCase, 
                       TypeDefinitionProtocol.TransferCase,
                       TypeDefinitionProtocol.TermAdministrativeInfraction,
                       TypeDefinitionProtocol.RequestDeviation
                   };
        }
    }
}