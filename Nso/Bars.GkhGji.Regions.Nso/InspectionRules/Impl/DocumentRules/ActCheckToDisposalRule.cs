namespace Bars.GkhGji.Regions.Nso.InspectionRules
{
    using Bars.B4;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Правило создание документа 'РАспоряжения на проверку предписания' из документа 'Акт проверки'
    /// В НСО данноеправило не работает
    /// </summary>
    public class ActCheckToDisposalRule : Bars.GkhGji.InspectionRules.ActCheckToDisposalRule
    {
       
        public override IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult(false, "В НСО данное правило не работает");
        }
    }
}
