namespace Bars.GkhGji.Regions.Nnovgorod.InspectionRules
{
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;

    /// <summary>
    /// В нижнем нет такого правила поэтому перекрываем его и возвращаем false
    /// </summary>
    public class NnResolutionToProtocolRule : ResolutionToProtocolRule
    {
        public override IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult(false, "В нижнем новгороде нет такого правила формирвоания");
        }
    }
}
