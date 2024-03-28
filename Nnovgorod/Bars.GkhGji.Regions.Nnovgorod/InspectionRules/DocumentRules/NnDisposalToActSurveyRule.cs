namespace Bars.GkhGji.Regions.Nnovgorod.InspectionRules
{
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;

    /// <summary>
    /// В нижнем новгороде правила создания Акта обследовния нет поэтому перекрываем его и возаращаем false
    /// </summary>
    public class NnDisposalToActSurveyRule : DisposalToActSurveyRule
    {
       public override IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult(false, "В Нижнем Новгороде нет такого правила");
        }
    }
}
