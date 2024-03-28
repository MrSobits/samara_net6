namespace Bars.Gkh.Gis.Enum
{
    using B4.Utils;

    /// <summary>
    /// Вид населенногоо пункта
    /// </summary>
    [Display("Вид населенногоо пункта")]
    public enum SettelmentType
    {
        /// <summary>
        /// Город
        /// </summary>
        [Display("Город")]
        City = 1,

        /// <summary>
        /// Не город
        /// </summary>
        [Display("Не город")]
        NotCity = 2,

        /// <summary>
        /// Общий
        /// </summary>
        [Display("Общий")]
        Common = 3
    } 
}