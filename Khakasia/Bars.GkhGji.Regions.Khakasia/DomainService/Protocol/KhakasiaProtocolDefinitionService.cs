namespace Bars.GkhGji.Regions.Khakasia.DomainService
{
    using Bars.GkhGji.Enums;

	/// <summary>
	/// Сервис для Определение протокола
	/// </summary>
    public class KhakasiaProtocolDefinitionService : Bars.GkhGji.DomainService.ProtocolDefinitionService
    {
		/// <summary>
		/// Получить типы определений
		/// </summary>
		/// <returns>Типы определений</returns>
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