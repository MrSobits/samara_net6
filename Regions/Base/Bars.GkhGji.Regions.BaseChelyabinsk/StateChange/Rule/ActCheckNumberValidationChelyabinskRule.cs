namespace Bars.GkhGji.Regions.BaseChelyabinsk.StateChange.Rule
{
    public class ActCheckNumberValidationChelyabinskRule : BaseDocNumberValidationChelyabinskRule
    {
        public override string Id { get { return "gji_nso_actcheck_validation_number"; } }

        public override string Name { get { return "Челябинск - Присвоение номера акта проверки"; } }

        public override string TypeId { get { return "gji_document_actcheck"; } }

        public override string Description { get { return "Данное правило присваивает номера акта проверки"; } }
        
    }
}