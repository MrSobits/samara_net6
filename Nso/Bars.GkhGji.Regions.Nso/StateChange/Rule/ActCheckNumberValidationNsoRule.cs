using Bars.GkhGji.Regions.Nso.StateChange;

namespace Bars.GkhGji.StateChange
{
    public class ActCheckNumberValidationNsoRule : BaseDocNumberValidationNsoRule
    {
        public override string Id { get { return "gji_nso_actcheck_validation_number"; } }

        public override string Name { get { return "НСО - Присвоение номера акта проверки"; } }

        public override string TypeId { get { return "gji_document_actcheck"; } }

        public override string Description { get { return "НСО - Данное правило присваивает номера акта проверки"; } }
        
    }
}