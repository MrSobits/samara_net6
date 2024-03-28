namespace Bars.GkhGji.StateChange
{
    /// <summary>
    /// Правило формирования номера протокола Перми
    /// </summary>
    public class ProtocolValidationNumberPermRule : BaseDocValidationNumberPermRule
    {
        public override string Id => "gji_perm_protocol_validation_number";

        public override string Name => "Проверка возможности формирования номера протокола Перми";

        public override string TypeId => "gji_document_prot";

        public override string Description => "Данное правило проверяет формирование номера протокола в соответствии с правилами Перми";
    }
}