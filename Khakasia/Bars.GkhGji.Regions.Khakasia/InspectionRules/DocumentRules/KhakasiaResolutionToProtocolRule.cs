namespace Bars.GkhGji.Regions.Khakasia.InspectionRules
{
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;

    /// <summary>
    /// В Хакасии нет такого правила поэтому перекрываем его и возвращаем false
    /// </summary>
    public class KhakasiaResolutionToProtocolRule : ResolutionToProtocolRule
    {
        public override IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult(false, "В Хакасии нет такого правила формирвоания");
        }
    }
}
