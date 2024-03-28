namespace Bars.GisIntegration.Base.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус обработки объекта
    /// </summary>
    public enum ObjectProcessingState
    {
        /// <summary>
        /// Успешно
        /// </summary>
        [Display("Успешно")]
        Success = 10,

        /// <summary>
        /// Ошибка
        /// </summary>
        [Display("Ошибка")]
        Error = 20,

        /// <summary>
        /// Предупреждение
        /// </summary>
        [Display("Предупреждение")]
        Warning = 30
    }
}
