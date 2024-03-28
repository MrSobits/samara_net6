namespace Bars.GkhGji.StateChange
{
    public class ActCheckValidationRule : BaseDocValidationRule
    {
        public override string Id
        {
            get { return "gji_document_actcheck_validation_rule"; }
        }

        public override string Name
        {
            get { return "Проверка заполненности карточки акта проверки"; }
        }

        public override string TypeId
        {
            get { return "gji_document_actcheck"; }
        }

        public override string Description
        {
            get { return "Данное правило проверяет заполненность необходимых полей акта проверки"; }
        }
    }
}