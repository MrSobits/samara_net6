namespace Bars.GkhGji.Regions.Nso.InspectionRules
{
    using Bars.B4;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Правило создание документа 'Распоряжения (основного)' из документа 'Акт проверки'
    /// Данное правило в Новосибе ненужно
    /// </summary>
    public class ActCheckToDisposalBaseRule : Bars.GkhGji.InspectionRules.ActCheckToDisposalBaseRule
    {
        
        public override IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult(false, "Данное правило неработает в НСО");
        }
    }
}
