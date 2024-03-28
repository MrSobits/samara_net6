namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    /// <summary>
    /// Правило создания из основания плановой проверки ЮЛ документа Распоряжения
    /// </summary>
    public class BaseJurPersonToTatDisposalRule : BaseJurPersonToDisposalBaseRule<TatarstanDisposal>
    {
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