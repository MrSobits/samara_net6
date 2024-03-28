namespace Bars.GkhGji.StateChange
{
    /// <summary>
    /// Правило формирования номера предписания Перми
    /// </summary>
    public class PrescriptionValidationNumberPermRule : BaseDocValidationNumberPermRule
    {
        public override string Id => "gji_perm_prescr_validation_number";

        public override string Name => "Проверка возможности формирования номера предписания Перми";

        public override string TypeId => "gji_document_prescr";

        public override string Description => "Данное правило проверяет формирование номера предписания в соответствии с правилами Перми";
    }
}