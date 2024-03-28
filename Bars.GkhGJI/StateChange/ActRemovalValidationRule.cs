namespace Bars.GkhGji.StateChange
{
    public class ActRemovalValidationRule : BaseDocValidationRule
    {
        public override string Id
        {
            get { return "gji_document_actrem_validation_rule"; }
        }

        public override string Name
        {
            get { return "Проверка заполненности карточки акта устранения нарушений"; }
        }

        public override string TypeId
        {
            get { return "gji_document_actrem"; }
        }

        public override string Description
        {
            get { return "Данное правило проверяет заполненность необходимых полей акта устранения нарушений"; }
        }
    }
}