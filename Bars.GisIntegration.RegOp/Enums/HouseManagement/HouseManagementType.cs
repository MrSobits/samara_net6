namespace Bars.GisIntegration.RegOp.Enums.HouseManagement
{
    using Bars.B4.Utils;

    /// <summary>
    /// Cпособ управления домом
    /// </summary>
    public enum HouseManagementType
    {
        /// <summary>
        /// Иной кооператив
        /// </summary>
        [Display("Иной кооператив")]
        AnotherCooperative = 10,

        /// <summary>
        /// Не выбран
        /// </summary>
        [Display("Не выбран")]
        NotSet = 20,

        /// <summary>
        /// Непосредственное управление
        /// </summary>
        [Display("Непосредственное управление")]
        LocalControl = 30
    }
}
