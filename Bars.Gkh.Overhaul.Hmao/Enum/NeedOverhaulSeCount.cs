namespace Bars.Gkh.Overhaul.Hmao.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Количество конструктивных элементов, требующих ремонта
    /// </summary>

    public enum NeedOverhaulSeCount
    {
        [Display("Полный перечень работ и (или) услуг по КР")]
        All = 10,

        [Display("50% от перечня услуг и (или) работ по КР")]
        OverFiftyPercent = 20,

        [Display("Отдельные виды услуг и (или) работ по КР")]
        LessFiftyPercent = 30
    }
}