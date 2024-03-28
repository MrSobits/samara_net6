namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    ///  Статус определения ЛС
    /// </summary>
    public enum PersonalAccountDeterminationState
    {
        /// <summary>
        /// Не определены
        /// </summary>
        [Display("Не определены")]
        NotDefined = 10,

        /// <summary>
        /// Частично определены
        /// </summary>
        [Display("Частично определены")]
        PartiallyDefined = 20,

        /// <summary>
        /// Определены
        /// </summary>
        [Display("Определены")]
        Defined = 30
    }
}