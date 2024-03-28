namespace Bars.GkhGji.Regions.BaseChelyabinsk.StateChange.Rule
{
    public class ResolutionNumberValidationChelyabinskRule : BaseDocNumberValidationChelyabinskRule
    {
        public override string Id { get { return "gji_nso_resolution_validation_number"; } }

        public override string Name { get { return "Челябинск - Присвоение номера постановления"; } }

        public override string TypeId { get { return "gji_document_resol"; } }

        public override string Description { get { return "Данное правило присваивает номера постановления"; } }
    }
}
