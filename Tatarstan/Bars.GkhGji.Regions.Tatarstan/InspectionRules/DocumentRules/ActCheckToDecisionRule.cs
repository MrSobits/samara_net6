namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules.DocumentRules
{
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanDecision;

    /// <summary>
    /// Правило создание документа 'Решение на проверку предписания' из документа 'Акт проверки'
    /// </summary>
    public class ActCheckToDecisionRule : ActCheckToDisposalBaseRule<TatarstanDecision>
    {
        /// <inheritdoc />
        public override string ResultName => "Решение на проверку предписания";

        /// <inheritdoc />
        public override TypeDocumentGji TypeDocumentResult => TypeDocumentGji.Decision;

        /// <inheritdoc />
        protected override TypeStage InspectionTypeStageResult => TypeStage.DecisionPrescription;
    }
}