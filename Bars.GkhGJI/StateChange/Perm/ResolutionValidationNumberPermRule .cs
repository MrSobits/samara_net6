namespace Bars.GkhGji.StateChange
{
    /// <summary>
    /// Правило проверки возможности формирования номера постановления Перми
    /// </summary>
    public class ResolutionValidationNumberPermRule : BaseDocValidationNumberPermRule
    {
        public override string Id => "gji_perm_resol_validation_number";

        public override string Name => "Проверка возможности формирования номера постановления Перми";

        public override string TypeId => "gji_document_resol";

        public override string Description => "Данное правило проверяет формирование номера постановления в соответствии с правилами Перми";
    }
}