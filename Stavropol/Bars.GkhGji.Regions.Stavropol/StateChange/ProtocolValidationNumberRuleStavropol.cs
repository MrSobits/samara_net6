namespace Bars.GkhGji.Regions.Stavropol.StateChange
{
    public class ProtocolValidationNumberRuleStavropol : DocGjiValidationNumberRuleStavropol
    {
        public override string Id { get { return "gji_stavropol_protocol_validation_number"; } }

        public override string Name { get { return "Правило формирования номера протокола (Ставрополь)"; } }

        public override string TypeId { get { return "gji_document_prot"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера протокола (Ставрополь)"; } }
    }
}