namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules.DocumentRules
{
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    /// <summary>
    /// Правило создание документа 'Распоряжения на проверку предписания' из документа 'Акт проверки'
    /// </summary>
    public class ActCheckToTatDisposalRule : ActCheckToDisposalBaseRule<TatarstanDisposal>
    {
    }
}