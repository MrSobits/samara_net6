namespace Bars.GisIntegration.Base.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип триггера
    /// </summary>
    public enum TriggerType
    {
        /// <summary>
        /// Подготовка данных
        /// </summary>
        [Display("Подготовка данных")]
        PreparingData = 10,

        /// <summary>
        /// Отправка данных
        /// </summary>
        [Display("Отправка данных")]
        SendingData = 20
    }
}
