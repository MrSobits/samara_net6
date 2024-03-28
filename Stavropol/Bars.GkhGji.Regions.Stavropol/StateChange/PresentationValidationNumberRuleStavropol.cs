namespace Bars.GkhGji.Regions.Stavropol.StateChange
{
    public class PresentationValidationNumberRuleStavropol : DocGjiValidationNumberRuleStavropol
    {
        public override string Id { get { return "gji_stavropol_presentation_validation_number"; } }

        public override string Name { get { return "Правило формирования номера представления (Ставрополь)"; } }

        public override string TypeId { get { return "gji_document_presen"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера представления (Ставрополь)"; } }
    }
}