namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules.DocumentRules
{
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;

    /// <summary>
    /// Правило формирования документа "Акт проверки" из "Решение"
    /// </summary>
    public class DecisionToActCheckPrescriptionRule : DisposalToActCheckPrescriptionRule
    {
        /// <inheritdoc />
        public override string Id => "DecisionToActCheckPrescriptionRule";

        /// <inheritdoc />
        public override string Description => "Правило создание документа 'Акт проверки предписания' из документа 'Решение'";

        /// <inheritdoc />
        public override TypeDocumentGji TypeDocumentInitiator => TypeDocumentGji.Decision;
    }
}