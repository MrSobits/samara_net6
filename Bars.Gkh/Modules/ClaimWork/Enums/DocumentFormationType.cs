using Bars.B4.Utils;

namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    /// <summary>
    /// Тип формирования документа
    /// </summary>
    public enum DocumentFormationType
    {
        /// <summary>
        /// Не формировать
        /// </summary>
        [Display("Не формировать")]
        NoForm = 0,

        /// <summary>
        /// Формировать вручную
        /// </summary>
        [Display("Формировать вручную")]
        Form = 1,

        /// <summary>
        /// Формировать автоматически
        /// </summary>
        [Display("Формировать автоматически")]
        AutoForm = 2
    }
}