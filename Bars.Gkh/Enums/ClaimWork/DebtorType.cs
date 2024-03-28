namespace Bars.Gkh.Enums.ClaimWork
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип должника
    /// </summary>
    [Display("Тип должника")]
    public enum DebtorType
    {
        /// <summary>
        /// Физическое лицо
        /// </summary>
        [Display("Физическое лицо")]
        Individual = 1,

        /// <summary>
        /// Юридическое лицо
        /// </summary>
        [Display("Юридическое лицо")]
        Legal = 2,
        
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0
    }
}