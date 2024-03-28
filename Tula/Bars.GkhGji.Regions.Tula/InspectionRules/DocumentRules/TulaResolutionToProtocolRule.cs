namespace Bars.GkhGji.Regions.Tula.InspectionRules
{
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;

    /// <summary>
    /// В Сахе нет такого правила поэтому перекрываем его и возвращаем false
    /// </summary>
    public class TulaResolutionToProtocolRule : ResolutionToProtocolRule
    {
        public override IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult(false, "В Сахе нет такого правила формирвоания");
        }
    }
}
