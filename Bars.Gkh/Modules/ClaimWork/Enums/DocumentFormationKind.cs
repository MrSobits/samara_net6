using Bars.B4.Utils;

namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    /// <summary>
    /// Признак формирования документа
    /// </summary>
    public enum DocumentFormationKind
    {
        /// <summary>
        /// Не формировать
        /// </summary>
        [Display("Не формировать")]
        NoForm = 0,

        /// <summary>
        /// Формировать
        /// </summary>
        [Display("Формировать")]
        Form = 1
    }
}
