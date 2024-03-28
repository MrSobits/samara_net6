namespace Bars.GkhGji.StateChange
{
    /// <summary>
    /// Правило формирования номера акта проверки Перми
    /// </summary>
    public class ActCheckValidationNumberPermRule : BaseDocValidationNumberPermRule
    {
        public override string Id => "gji_perm_actcheck_validation_number";

        public override string Name => "Проверка возможности формирования номера акта проверки Перми";

        public override string TypeId => "gji_document_actcheck";

        public override string Description => "Данное правило проверяет формирование номера акта проверки в соответствии с правилами Перми";
    }
}