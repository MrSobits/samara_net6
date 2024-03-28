using Bars.GkhGji.Regions.Nso.StateChange;

namespace Bars.GkhGji.StateChange
{
    public class PrescriptionNumberValidationNsoRule : BaseDocNumberValidationNsoRule
    {
        public override string Id { get { return "gji_nso_prescription_validation_number"; } }

        public override string Name { get { return "НСО - Присвоение номера предписания"; } }

        public override string TypeId { get { return "gji_document_prescr"; } }

        public override string Description { get { return "НСО - Данное правило присваивает номера предписания"; } }
        
    }
}