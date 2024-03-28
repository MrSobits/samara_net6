namespace Bars.GkhGji.StateChange
{
    /// <summary>
    /// Правило формирования номера представления Перми
    /// </summary>
    public class PresentationValidationNumberPermRule : BaseDocValidationNumberPermRule
    {
        public override string Id => "gji_perm_presen_validation_number";

        public override string Name => "Проверка возможности формирования номера представления Перми";

        public override string TypeId => "gji_document_presen";

        public override string Description => "Данное правило проверяет формирование номера представления в соответствии с правилами Перми";
    }
}