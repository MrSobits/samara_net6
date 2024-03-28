namespace Bars.GkhGji.StateChange
{
    public class ProtocolValidationNumberYanaoRule : BaseDocValidationNumberYanaoRule
    {
        public override string Id { get { return "gji_yanao_protocol_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера протокола ЯНАО"; } }

        public override string TypeId { get { return "gji_document_prot"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера протокола в соответствии с правилами ЯНАО"; } }
    }
}