namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanDecision;

    /// <summary>
    /// Правило создания из основания проверки по поручению руководства документа Решение
    /// </summary>
    public class BaseDispHeadToDecisionRule : BaseDispHeadToDisposalBaseRule<TatarstanDecision>
    {
        /// <inheritdoc />
        public override string Id => "BaseDispHeadToDecisionRule";

        /// <inheritdoc />
        public override string ResultName => "Решение";

        /// <inheritdoc />
        public override TypeDocumentGji TypeDocumentResult => TypeDocumentGji.Decision;

        /// <inheritdoc />
        protected override TypeStage InspectionTypeStageResult => TypeStage.Decision;

        /// <inheritdoc />
        public override IDataResult ValidationRule(InspectionGji inspection)
        {
            var result = base.ValidationRule(inspection);

            if (!result.Success)
            {
                return result;
            }
            
            var gjiValidityDocPeriodService = this.Container.Resolve<IGjiValidityDocPeriodService>();

            using (this.Container.Using(gjiValidityDocPeriodService))
            {
                return gjiValidityDocPeriodService.DocPeriodValidation(inspection.CheckDate, this.TypeDocumentResult);
            }
        }
    }
}