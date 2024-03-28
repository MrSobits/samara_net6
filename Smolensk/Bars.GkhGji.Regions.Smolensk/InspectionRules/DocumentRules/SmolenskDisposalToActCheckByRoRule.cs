namespace Bars.GkhGji.Regions.Smolensk.InspectionRules
{
    using Bars.B4;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Короче в смоленске такого правила нет, все акты проверки создаются по-умолчанию без домов
    /// </summary>
    public class SmolenskDisposalToActCheckByRoRule : Bars.GkhGji.InspectionRules.DisposalToActCheckByRoRule
    {
        public override IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult(false, "В смоленске такого нет правила, все акты по умолчанию добавляются без домов");
        }
    }
}
