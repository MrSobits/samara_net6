namespace Bars.GkhGji.Regions.BaseChelyabinsk.StateChange.Rule
{
    public class ActRemovalNumberValidationChelyabinskRule : BaseDocNumberValidationChelyabinskRule
    {
        public override string Id { get { return "gji_nso_actremoval_validation_number"; } }

        public override string Name { get { return "Челябинск - Присвоение номера акта устранения нарушений"; } }

        public override string TypeId { get { return "gji_document_actrem"; } }

        public override string Description { get { return "Данное правило присваивает номера акта устранения нарушений"; } }
        
    }
}