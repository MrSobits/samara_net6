namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules
{
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanDecision;

    /// <summary>
    /// Правило создания документа Решения из основания проверки по мероприятиям без взаимодействия с контролируемыми лицами
    /// </summary>
    public class InspectionActionIsolatedToDecisionRule : InspectionActionIsolatedToDisposalBaseRule<TatarstanDecision>
    {
        /// <inheritdoc />
        public override string Id => "InspectionActionIsolatedToDecisionRule";

        /// <inheritdoc />
        public override string ResultName => "Решение";

        /// <inheritdoc />
        public override TypeDocumentGji TypeDocumentResult => TypeDocumentGji.Decision;

        /// <inheritdoc />
        protected override TypeStage InspectionTypeStageResult => TypeStage.Decision;
    }
}