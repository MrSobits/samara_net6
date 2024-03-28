namespace Bars.GkhGji.StateChange
{
    public class ProtocolValidationRule : BaseDocValidationRule
    {
        public override string Id
        {
            get { return "gji_document_prot_validation_rule"; }
        }

        public override string Name
        {
            get { return "Проверка заполненности карточки протокола"; }
        }

        public override string TypeId
        {
            get { return "gji_document_prot"; }
        }

        public override string Description
        {
            get { return "Данное правило проверяет заполненность необходимых полей протокола"; }
        }
    }
}