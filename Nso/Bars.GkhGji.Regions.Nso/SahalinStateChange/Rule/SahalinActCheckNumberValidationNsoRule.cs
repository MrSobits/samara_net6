using Bars.GkhGji.Regions.Nso.StateChange;

namespace Bars.GkhGji.StateChange
{
    public class SahalinActCheckNumberValidationNsoRule : SahalinBaseDocNumberValidationNsoRule
    {
        public override string Id { get { return "gji_sahalin_actcheck_validation_number"; } }

        public override string Name { get { return "САХАЛИН - Присвоение номера акта проверки"; } }

        public override string TypeId { get { return "gji_document_actcheck"; } }

        public override string Description { get { return "САХАЛИН - Данное правило присваивает номера акта проверки"; } }
        
    }
}