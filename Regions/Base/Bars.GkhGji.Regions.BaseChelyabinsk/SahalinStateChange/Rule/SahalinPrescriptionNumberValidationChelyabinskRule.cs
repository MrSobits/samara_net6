namespace Bars.GkhGji.Regions.BaseChelyabinsk.SahalinStateChange.Rule
{
    public class SahalinPrescriptionNumberValidationChelyabinskRule : SahalinBaseDocNumberValidationChelyabinskRule
    {
        public override string Id { get { return "gji_sahalin_prescription_validation_number"; } }

        public override string Name { get { return "САХАЛИН - Присвоение номера предписания"; } }

        public override string TypeId { get { return "gji_document_prescr"; } }

        public override string Description { get { return "САХАЛИН - Данное правило присваивает номера предписания"; } }
        
    }
}