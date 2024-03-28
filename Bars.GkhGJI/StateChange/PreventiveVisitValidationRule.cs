namespace Bars.GkhGji.StateChange
{
    public class PreventiveVisitValidationRule : BaseDocValidationRule
    {
        public override string Id
        {
            get { return "gji_document_preventicevisit_validation_rule"; }
        }

        public override string Name
        {
            get { return "Проверка заполненности карточки акта профилактического визита"; }
        }

        public override string TypeId
        {
            get { return "gji_document_prevvisit"; }
        }

        public override string Description
        {
            get { return "Данное правило проверяет заполненность необходимых полей акта профвизита"; }
        }
    }
}