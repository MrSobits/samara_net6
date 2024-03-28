namespace Bars.GkhGji.StateChange
{
    public class PresentationValidationRule : BaseDocValidationRule
    {
        public override string Id
        {
            get { return "gji_document_presen_validation_rule"; }
        }

        public override string Name
        {
            get { return "Проверка заполненности карточки представления"; }
        }

        public override string TypeId
        {
            get { return "gji_document_presen"; }
        }

        public override string Description
        {
            get { return "Данное правило проверяет заполненность необходимых полей представления"; }
        }
    }
}