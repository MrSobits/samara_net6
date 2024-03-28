namespace Bars.Gkh.Overhaul.Hmao.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Типы решения собственников помещений МКД
    /// </summary>

    public enum PropertyOwnerDecisionType
    {
        [Display("Выбор способа формирования фонда кап.ремонта")]
        SelectMethodForming = 10,

        [Display("Установление минимального размера взноса кап.ремонта")]
        SetMinAmount = 20
    }
}