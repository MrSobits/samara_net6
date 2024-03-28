namespace Bars.GkhGji.StateChange
{
    public class PrescriptionValidationRule : BaseDocValidationRule
    {
        public override string Id
        {
            get { return "gji_document_prescr_validation_rule"; }
        }

        public override string Name
        {
            get { return "Проверка заполненности карточки предписания"; }
        }

        public override string TypeId
        {
            get { return "gji_document_prescr"; }
        }

        public override string Description
        {
            get { return "Данное правило проверяет заполненность необходимых полей предписания"; }
        }
    }
}