namespace Bars.GkhGji.StateChange
{
    public class ActSurveyValidationRule : BaseDocValidationRule
    {
        public override string Id
        {
            get { return "gji_document_actsur_validation_rule"; }
        }

        public override string Name
        {
            get { return "Проверка заполненности карточки акта обследования"; }
        }

        public override string TypeId
        {
            get { return "gji_document_actsur"; }
        }

        public override string Description
        {
            get { return "Данное правило проверяет заполненность необходимых полей акта обследования"; }
        }
    }
}