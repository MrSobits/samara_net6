namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using Bars.GkhGji.Enums;

    public class ResolutionDefinitionService : Bars.GkhGji.DomainService.ResolutionDefinitionService
    {
        public override TypeDefinitionResolution[] DefinitionTypes()
        {
            // Вообщем по умолчанию регистрируются только такие типы 
            // в слуаче если в регионе нобходимы другие, то тогда заменяем реализацию
            return new []
                {
                    TypeDefinitionResolution.Deferment,
                    TypeDefinitionResolution.CorrectionError,
                    TypeDefinitionResolution.ProlongationReview,
                    TypeDefinitionResolution.TransferRegulation,
                    TypeDefinitionResolution.AppointmentPlaceTime,
                    TypeDefinitionResolution.SuspenseReviewAppeal
                };
        }
    }
}