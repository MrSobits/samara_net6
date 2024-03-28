namespace Bars.GkhGji.Regions.Stavropol.StateChange
{
    public class DisposalValidationNumberRuleStavropol : DocGjiValidationNumberRuleStavropol
    {
        public override string Id { get { return "gji_stavropol_disposal_validation_number"; } }

        public override string Name { get { return "Правило формирования номера распоряжения (Ставрополь)"; } }

        public override string TypeId { get { return "gji_document_disp"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера распоряжения (Ставрополь)"; } }
    }
}