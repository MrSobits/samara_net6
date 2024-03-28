using Bars.B4.Utils;

namespace Bars.Gkh.RegOperator.Enums
{
    /// <summary>
    /// Тип получателя
    /// </summary>
    public enum RecipientType
    {
        /// <summary>
        /// Все
        /// </summary>
        [Display("Все")]
        All = 0,

        /// <summary>
        /// Юр.лицо
        /// </summary>
        [Display("Юр.лицо")]
        LegalEntity = 10,

        /// <summary>
        /// Физ.лицо
        /// </summary>
        [Display("Физ.лицо")]
        Person = 20
    }
}
