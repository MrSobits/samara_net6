namespace Bars.GkhGji.Regions.Stavropol.StateChange
{
    public class ResolutionValidationNumberRuleStavropol : DocGjiValidationNumberRuleStavropol
    {
        public override string Id { get { return "gji_stavropol_resolution_validation_number"; } }

        public override string Name { get { return "Правило формирования номера постановления (Ставрополь)"; } }

        public override string TypeId { get { return "gji_document_resol"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера постановления (Ставрополь)"; } }
    }
}