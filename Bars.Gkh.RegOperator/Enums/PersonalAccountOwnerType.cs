using Bars.B4.Utils;

namespace Bars.Gkh.RegOperator.Enums
{
    /// <summary>
    /// Тип абонента
    /// </summary>
    public enum PersonalAccountOwnerType
    {
        /// <summary>
        /// Счет физ.лица
        /// </summary>
        [Display("Счет физ.лица")]
        Individual = 0x0,

        /// <summary>
        /// Счет юр.лица
        /// </summary>
        [Display("Счет юр.лица")]
        Legal = 0x1
    }
}