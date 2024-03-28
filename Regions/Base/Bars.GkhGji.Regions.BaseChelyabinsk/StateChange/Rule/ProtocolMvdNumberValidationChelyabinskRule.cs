namespace Bars.GkhGji.Regions.BaseChelyabinsk.StateChange.Rule
{
    public class ProtocolMvdNumberValidationChelyabinskRule : BaseDocNumberValidationChelyabinskRule
    {
        public override string Id { get { return "gji_сhelyabinsk_protocolmvd_validation_number"; } }

        public override string Name { get { return "Челябинск - Присвоение номера Протокол МВД"; } }

        public override string TypeId { get { return "gji_document_protocolmvd"; } }

        public override string Description { get { return "Данное правило присваивает номера Протокол МВД"; } }

    }
}