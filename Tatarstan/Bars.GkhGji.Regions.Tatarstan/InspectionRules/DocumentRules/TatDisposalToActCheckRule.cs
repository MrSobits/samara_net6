namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules
{
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;

    /// <summary>
    /// Правило создания Акта Проверки (Общий) из распоряжения в татарстане нет, поэтому переопредееляю метод и возвращаю внем false
    /// </summary>
    public class TatDisposalToActCheckRule : DisposalToActCheckRule
    {
        public override IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult(false, "В РТ нет правила создания общего Акта проверки из распоряжения");
        }
    }
}
