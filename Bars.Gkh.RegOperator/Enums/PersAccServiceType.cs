namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    public enum PersAccServiceType
    {
        /// <summary>
        /// Не выбран
        /// </summary>
        [Display("Не выбран")]
        NotSelected = 10,

        /// <summary>
        /// Кап. ремонт
        /// </summary>
        [Display("Кап. ремонт")]
        Overhaul = 20,

        /// <summary>
        /// Найм
        /// </summary>
        [Display("Найм")]
        Recruitment = 30,

        /// <summary>
        /// Кап. ремонт и найм
        /// </summary>
        [Display("Кап. ремонт и найм")]
        OverhaulRecruitment = 40
    }
}