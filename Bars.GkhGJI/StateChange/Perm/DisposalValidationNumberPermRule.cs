namespace Bars.GkhGji.StateChange
{
    using Bars.GkhGji.Contracts;

    /// <summary>
    /// Правило формирования номера распоряжения Перми
    /// </summary>
    public class DisposalValidationNumberPermRule : BaseDocValidationNumberPermRule
    {
        public IDisposalText DisposalText { get; set; }

        public override string Id => "gji_perm_disposal_validation_number";

        public override string TypeId => "gji_document_disp";

        public override string Name => $"Проверка возможности формирования номера {this.DisposalText.GenetiveCase.ToLower()} Пермь";

        public override string Description =>
            $"Данное правило проверяет формирование номера {this.DisposalText.GenetiveCase.ToLower()} в соответствии с правилами Перми";
    }
}