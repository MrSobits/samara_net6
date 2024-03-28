namespace Bars.GkhGji.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип даты проверки
    /// </summary>
    public enum CheckDateType
    {
        /// <summary>
        /// С учетом производственного календаря 
        /// </summary>
        [Display("С учетом производственного календаря")]
        WithIndustrialCalendar = 10,

        /// <summary>
        /// Без учета производственного календаря
        /// </summary>
        [Display("Без учета производственного календаря")]
        WithoutIndustrialCalendar = 20
    }
}