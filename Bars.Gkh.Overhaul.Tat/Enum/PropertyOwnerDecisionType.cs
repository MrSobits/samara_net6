namespace Bars.Gkh.Overhaul.Tat.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Типы решения собственников помещений МКД
    /// </summary>
    public enum PropertyOwnerDecisionType
    {
        /// <summary>
        /// Выбор способа формирования фонда кап.ремонта
        /// </summary>
        [Display("Выбор способа формирования фонда кап.ремонта")]
        SelectMethodForming = 10,

        /// <summary>
        /// Установление минимального размера взноса на кап.ремонт
        /// </summary>
        [Display("Установление минимального размера взноса на кап.ремонт")]
        SetMinAmount = 20,

        /// <summary>
        /// Установление фактических дат проведения КР
        /// </summary>
        [Display("Установление фактических дат проведения КР")]
        ListOverhaulServices = 30,

        /// <summary>
        /// Установление минимального размера фонда КР
        /// </summary>
        [Display("Установление минимального размера фонда КР")]
        MinCrFundSize = 60
    }
}