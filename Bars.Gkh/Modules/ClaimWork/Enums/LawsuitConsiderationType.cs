namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using B4.Utils;

    /// <summary>
    /// Кем рассмотрено исковое заявление
    /// </summary>
    public enum LawsuitConsiderationType
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Арбитражный суд
        /// </summary>
        [Display("Арбитражный суд")]
        ArbitrationCourt = 10,

        /// <summary>
        /// Мировой суд
        /// </summary>
        [Display("Мировой суд")]
        WorldCourt = 20,

        /// <summary>
        /// Районный суд
        /// </summary>
        [Display("Районный суд")]
        RaionCourt = 30
    }
}