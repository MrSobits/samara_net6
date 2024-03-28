namespace Bars.GkhGji.StateChange
{
    public class DisposalValidationNumberYanaoRule : BaseDocValidationNumberYanaoRule
    {
        public override string Id { get { return "gji_yanao_disposal_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера распоряжения ЯНАО"; } }

        public override string TypeId { get { return "gji_document_disp"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера распоряжения в соответствии с правилами ЯНАО"; } }
    }
}