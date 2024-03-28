namespace Bars.GkhGji.Regions.Chelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    ///Группы тяжести
    /// </summary>
    public enum SeverityGroup
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Группа А"
        /// </summary>
        [Display("Группа А")]
        AGroup = 10,

        /// <summary>
        /// Группа А"
        /// </summary>
        [Display("Группа Б")]
        BGroup = 20,
    }
}