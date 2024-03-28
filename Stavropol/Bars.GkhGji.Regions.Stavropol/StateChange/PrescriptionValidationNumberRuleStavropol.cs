namespace Bars.GkhGji.Regions.Stavropol.StateChange
{
    public class PrescriptionValidationNumberRuleStavropol : DocGjiValidationNumberRuleStavropol
    {
        public override string Id { get { return "gji_stavropol_prescription_validation_number"; } }

        public override string Name { get { return "Правило формирования номера предписания (Ставрополь)"; } }

        public override string TypeId { get { return "gji_document_prescr"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера предписания (Ставрополь)"; } }
    }
}