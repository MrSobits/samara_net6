namespace Bars.GkhGji.StateChange
{
    public class PrescriptionValidationNumberYanaoRule : BaseDocValidationNumberYanaoRule
    {
        public override string Id { get { return "gji_yanao_prescription_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера предписания ЯНАО"; } }

        public override string TypeId { get { return "gji_document_prescr"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера предписания в соответствии с правилами ЯНАО"; } }
    }
}