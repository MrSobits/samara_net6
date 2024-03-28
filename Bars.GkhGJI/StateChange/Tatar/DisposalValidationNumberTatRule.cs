namespace Bars.GkhGji.StateChange
{
    using Bars.GkhGji.Contracts;

    public class DisposalValidationNumberTatRule : BaseDocValidationNumberTatRule
    {
        public IDisposalText DisposalText { get; set; }

        public override string Id { get { return "gji_tatar_disposal_validation_number"; } }

        public override string TypeId { get { return "gji_document_disp"; } }

        public override string Name 
        { 
            get { return string.Format("Проверка возможности формирования номера {0} РТ", DisposalText.GenetiveCase.ToLower()); } 
        }

        public override string Description 
        {
            get { return string.Format("Данное правило проверяет формирование номера {0} в соответствии с правилами РТ", DisposalText.GenetiveCase.ToLower()); }
        }
    }
}