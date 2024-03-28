namespace Bars.GkhGji.StateChange
{
    /// <summary>
    /// Правило формирования номера акта проверки предписания Перми
    /// </summary>
    public class ActRemovalValidationNumberPermRule : BaseDocValidationNumberPermRule
    {
        public override string Id => "gji_perm_actremoval_validation_number";

        public override string Name => "Проверка возможности формирования номера акта проверки предписания Перми";

        public override string TypeId => "gji_document_actrem";

        public override string Description => "Данное правило проверяет формирование номера акта проверки предписания в соответствии с правилами Перми";
    }
}