namespace Bars.GkhGji.Regions.Stavropol.StateChange
{
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;

    public class ActCheckValidationNumberRuleStavropol : DocGjiValidationNumberRuleStavropol
    {
        public override string Id { get { return "gji_stavropol_actcheck_validation_number"; } }

        public override string Name { get { return "Правило формирования номера акта проверки (Ставрополь)"; } }

        public override string TypeId { get { return "gji_document_actcheck"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера акта проверки (Ставрополь)"; } }

        protected override ValidateResult Action(DocumentGji document)
        {//Акт проверки - Номер акту присваивается как «Номер распоряжения – Номер инспектора/порядковый номер акта проверки в проверке»
            return GetActDocumentNumber(document);
        }
    }
}