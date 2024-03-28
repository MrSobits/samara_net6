namespace Bars.GkhGji.Regions.BaseChelyabinsk.StateChange.Rule
{
    public class PrescriptionNumberValidationChelyabinskRule : BaseDocNumberValidationChelyabinskRule
    {
        public override string Id { get { return "gji_nso_prescription_validation_number"; } }

        public override string Name { get { return "Челябинск - Присвоение номера предписания"; } }

        public override string TypeId { get { return "gji_document_prescr"; } }

        public override string Description { get { return "Данное правило присваивает номера предписания"; } }
        
    }
}