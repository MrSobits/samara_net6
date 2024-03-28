namespace Bars.GkhGji.StateChange
{
    public class ResolProsValidationNumberTatRule : BaseDocValidationNumberTatRule
    {
        public override string Id { get { return "gji_tatar_resolpros_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера постановления прокуратуры РТ"; } }

        public override string TypeId { get { return "gji_document_resolpros"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера постановления прокуратуры в соответствии с правилами РТ"; } }
    }
}