namespace Bars.Gkh.Overhaul.Hmao.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип критерия для актуализации регпрограммы
    /// </summary>

    public enum CriteriaType
    {
        [Display("Срок эксплуатации МКД")]
        Lifetime = 10,

        [Display("Доля фактически поступивших взносов")]
        ShareReceived = 20,

        [Display("Степень физического износа конструктивных элементов")]
        Werout = 30
    }
}