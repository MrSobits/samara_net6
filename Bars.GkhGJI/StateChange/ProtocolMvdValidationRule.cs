namespace Bars.GkhGji.StateChange
{
    public class ProtocolMvdValidationRule : BaseDocValidationRule
    {
        public override string Id
        {
            get { return "gji_document_protocolmvd_validation_rule"; }
        }

        public override string Name
        {
            get { return "Проверка заполненности карточки протокола МВД"; }
        }

        public override string TypeId
        {
            get { return "gji_document_protocolmvd"; }
        }

        public override string Description
        {
            get { return "Данное правило проверяет заполненность необходимых полей протокола МВД"; }
        }
    }
}