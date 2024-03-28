namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип служебной записки по предписаниям
    /// </summary>
    public enum OfficialReportType
    {
        /// <summary>
        /// Устранение нарушений
        /// </summary>
        [Display("Служебная записка")]
        Removal = 10,

        /// <summary>
        /// Продление срока исполнения
        /// </summary>
        [Display("Решение о продлении")]
        Extension = 20
    }
}