namespace Bars.GkhGji.Regions.BaseChelyabinsk.StateChange.Rule
{
    public class ProtocolMhcNumberValidationChelyabinskRule : BaseDocNumberValidationChelyabinskRule
    {
        public override string Id { get { return "gji_сhelyabinsk_protocolmhc_validation_number"; } }

        public override string Name { get { return "Челябинск - Присвоение номера Протокол МЖК"; } }

        public override string TypeId { get { return "gji_document_protocolmhc"; } }

        public override string Description { get { return "Данное правило присваивает номера Протокол МЖК"; } }
    }
}