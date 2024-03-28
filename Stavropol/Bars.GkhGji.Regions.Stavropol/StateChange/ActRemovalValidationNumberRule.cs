namespace Bars.GkhGji.Regions.Stavropol.StateChange
{
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActRemovalValidationNumberRuleStavropol : DocGjiValidationNumberRuleStavropol
    {
        public override string Id { get { return "gji_stavropol_actremoval_validation_number"; } }

        public override string Name { get { return "Правило формирования номера акта устранения нарушений (Ставрополь)"; } }

        public override string TypeId { get { return "gji_document_actrem"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера акта устранения нарушений (Ставрополь)"; } }

        protected override ValidateResult Action(DocumentGji document)
        {//Акт устранения - Номер акту присваивается как «Номер распоряжения – Номер инспектора/порядковый номер акта устранения в проверке»
            return GetActDocumentNumber(document, TypeDocumentGji.ActCheck);
        }
    }
}