namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanDecision;

    /// <summary>
    /// Правило создания из основания плановой проверки ЮЛ документа Решение
    /// </summary>
    public class BaseJurPersonToDecisionRule : BaseJurPersonToDisposalBaseRule<TatarstanDecision>
    {
        /// <inheritdoc />
        public override string Id => "BaseJurPersonToDecisionRule";

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
            
            var baseJurPersonDomain = this.Container.ResolveDomain<BaseJurPerson>();
            var gjiValidityDocPeriodService = this.Container.Resolve<IGjiValidityDocPeriodService>();

            try
            {
                var baseJurPerson = baseJurPersonDomain.Get(inspection.Id);

                return gjiValidityDocPeriodService.DocPeriodValidation(baseJurPerson.DateStart, this.TypeDocumentResult);
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                this.Container.Release(baseJurPersonDomain);
                this.Container.Release(gjiValidityDocPeriodService);
            }
        }
    }
}