namespace Bars.GkhGji.Regions.Stavropol.DomainService
{
    using Bars.GkhGji.Enums;
    using Castle.Windsor;

    public class ResolutionDefinitionService : Bars.GkhGji.DomainService.ResolutionDefinitionService
    {
        public IWindsorContainer Container { get; set; }
        
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
                    TypeDefinitionResolution.SuspenseReviewAppeal,
                    TypeDefinitionResolution.Installment
                };
        }

        protected class TypeResolutionDefinitionProxy
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Display { get; set; }
        }
    }
}