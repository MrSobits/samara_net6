namespace Bars.GkhGji.StateChange
{
    public class ProtocolMhcValidationRule : BaseDocValidationRule
    {
        public override string Id
        {
            get { return "gji_document_protocolmhc_validation_rule"; }
        }

        public override string Name
        {
            get { return "Проверка заполненности карточки Протокол МЖК"; }
        }

        public override string TypeId
        {
            get { return "gji_document_protocolmhc"; }
        }

        public override string Description
        {
            get { return "Данное правило проверяет заполненность необходимых полей протокола МЖК"; }
        }
    }
}