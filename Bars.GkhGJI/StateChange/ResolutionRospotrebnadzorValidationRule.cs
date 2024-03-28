namespace Bars.GkhGji.StateChange
{
    public class ResolutionRospotrebnadzorValidationRule : BaseDocValidationRule
    {
        public override string Id => "gji_document_resol_rosp_validation_rule";

        public override string Name => "Проверка заполненности карточки постановления Роспотребнадзора";

        public override string TypeId => "gji_document_resol_rosp";

        public override string Description => "Данное правило проверяет заполненность необходимых полей постановления Роспотребнадзора";
    }
}