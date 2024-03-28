namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Факт возбуждения дела
    /// </summary>
    public enum LawsuitFactInitiationType
    {
        /// <summary>
        /// Возбуждено
        /// </summary>
        [Display("Не возбуждено")]
        NotInitiated = 0,

        /// <summary>
        /// Возбуждено
        /// </summary>
        [Display("Возбуждено")]
        Initiated = 10,

        /// <summary>
        /// Отказано в возбуждении
        /// </summary>
        [Display("Отказано в возбуждении")]
        RefusedToInitiate = 20
    }
}
