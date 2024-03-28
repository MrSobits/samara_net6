namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules
{
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;

    /// <summary>
    /// Правило создание документа 'Акт обследования' из документа 'Распоряжение' (по выбранным домам)
    /// </summary>
    public class TatDisposalToActSurveyRule : DisposalToActSurveyRule
    {
        public override IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult(false, "В РТ нет правила создания Акта обследования из Распоряжения");
        }
    }
}
