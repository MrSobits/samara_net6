namespace Bars.GkhGji.Regions.BaseChelyabinsk.StateChange.Rule
{
    public class ResolProsNumberValidationChelyabinskRule : BaseDocNumberValidationChelyabinskRule
    {
        public override string Id { get { return "gji_сhelyabinsk_resolpros_validation_number"; } }

        public override string Name { get { return "Челябинск - Присвоение номера Постановление прокуратуры"; } }

        public override string TypeId { get { return "gji_document_resolpros"; } }

        public override string Description { get { return "Данное правило присваивает номера Постановление прокуратуры"; } }

    }
}