namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Способ поступления
    /// </summary>
    public enum AdmissionType
    {
        /// <summary>
        /// Не выбрано
        /// </summary>
        [Display("Не выбрано")]
        Unselected = 10,

        /// <summary>
        /// Вручную
        /// </summary>
        [Display("Вручную")]
        Manual = 20,

        /// <summary>
        /// Получено из ГИС ГМП
        /// </summary>
        [Display("Получено из ГИС ГМП")]
        RecievedFromGisGmp = 30
    }
}