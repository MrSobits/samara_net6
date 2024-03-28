namespace Bars.GkhGji.Regions.BaseChelyabinsk.StateChange.Rule
{
    public class PresentationNumberValidationChelyabinskRule : BaseDocNumberValidationChelyabinskRule
    {
        public override string Id { get { return "gji_сhelyabinsk_presen_validation_number"; } }

        public override string Name { get { return "Челябинск - Присвоение номера Представление"; } }

        public override string TypeId { get { return "gji_document_presen"; } }

        public override string Description { get { return "Данное правило присваивает номера Представление"; } }
    }
}