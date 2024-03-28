namespace Bars.GkhGji.Regions.Stavropol.StateChange
{
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;

    public class ActSurveyValidationNumberRuleStavropol : DocGjiValidationNumberRuleStavropol
    {
        public override string Id { get { return "gji_stavropol_actsurvey_validation_number"; } }

        public override string Name { get { return "Правило формирования номера акта обследования (Ставрополь)"; } }

        public override string TypeId { get { return "gji_document_actsur"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера акта обследования (Ставрополь)"; } }

        protected override ValidateResult Action(DocumentGji document)
        {//Акт обследования - Номер акту присваивается как «Номер распоряжения – Номер инспектора/порядковый номер акта обследования в проверке»
            return GetActDocumentNumber(document);
        }

    }
}