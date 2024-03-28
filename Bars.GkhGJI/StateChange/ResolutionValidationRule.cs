namespace Bars.GkhGji.StateChange
{
    public class ResolutionValidationRule : BaseDocValidationRule
    {
        public override string Id
        {
            get { return "gji_document_resol_validation_rule"; }
        }

        public override string Name
        {
            get { return "Проверка заполненности карточки постановления"; }
        }

        public override string TypeId
        {
            get { return "gji_document_resol"; }
        }

        public override string Description
        {
            get { return "Данное правило проверяет заполненность необходимых полей постановления"; }
        }
    }
}