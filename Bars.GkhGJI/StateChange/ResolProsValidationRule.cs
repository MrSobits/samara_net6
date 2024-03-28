namespace Bars.GkhGji.StateChange
{
    public class ResolProsValidationRule : BaseDocValidationRule
    {
        public override string Id
        {
            get { return "gji_document_resolpros_validation_rule"; }
        }

        public override string Name
        {
            get { return "Проверка заполненности карточки постановления прокуратуры"; }
        }

        public override string TypeId
        {
            get { return "gji_document_resolpros"; }
        }

        public override string Description
        {
            get { return "Данное правило проверяет заполненность необходимых полей постановления прокуратуры"; }
        }
    }
}