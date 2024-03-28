﻿namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    /// <summary>
    /// Правило создания документа Распоряжение для проверки по требованию прокуратуры
    /// </summary>
    public class BaseProsClaimToTatDisposalRule : BaseProsClaimToDisposalBaseRule<TatarstanDisposal>
    {
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