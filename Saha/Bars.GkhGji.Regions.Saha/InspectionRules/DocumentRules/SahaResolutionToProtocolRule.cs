namespace Bars.GkhGji.Regions.Saha.InspectionRules
{
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;

    /// <summary>
    /// В Сахе нет такого правила поэтому перекрываем его и возвращаем false
    /// </summary>
    public class SahaResolutionToProtocolRule : ResolutionToProtocolRule
    {
        public override IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult(false, "В Сахе нет такого правила формирвоания");
        }
    }
}
