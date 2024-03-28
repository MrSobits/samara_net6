namespace Bars.Gkh.Overhaul.Nso.Enum
{
    using Bars.B4.Utils;

    public enum PriorityParamAdditionFactor
    {
        [Display("Не используется")]
        NotUsing = 10,

        [Display("Используется")]
        Using = 20
    }

    public enum PriorityParamFinalValue
    {
        [Display("Бальная оценка")]
        PointScore = 10,

        [Display("Значение параметра")]
        ParameterValue = 20
    }
}
