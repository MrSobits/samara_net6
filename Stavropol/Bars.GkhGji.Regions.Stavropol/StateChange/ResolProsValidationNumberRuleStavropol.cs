namespace Bars.GkhGji.Regions.Stavropol.StateChange
{
    public class ResolProsValidationNumberRuleStavropol : DocGjiValidationNumberRuleStavropol
    {
        public override string Id { get { return "gji_stavropol_resolpros_validation_number"; } }

        public override string Name { get { return "Правило возможности формирования номера постановления прокуратуры (Ставрополь)"; } }

        public override string TypeId { get { return "gji_document_resolpros"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера постановления прокуратуры (Ставрополь)"; } }
    }
}