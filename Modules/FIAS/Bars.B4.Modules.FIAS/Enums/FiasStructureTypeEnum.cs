namespace Bars.B4.Modules.FIAS
{
    using Bars.B4.Utils;

    /// <summary>
    /// Перечень видов строений ФИАС
    /// </summary>
    public enum FiasStructureTypeEnum : byte
    {
        /// <summary>
        /// Не определено
        /// </summary>
        [Display("")]
        [Description("Не определено")]
        NotDefined = 0,

        /// <summary>
        /// Строение
        /// </summary>
        [Display("стр")]
        [Description("Строение")]
        Structure = 1,

        /// <summary>
        /// Сооружение
        /// </summary>
        [Display("соор")]
        [Description("Сооружение")]
        Construction = 2,

        /// <summary>
        /// Литер
        /// </summary>
        [Display("литер")]
        [Description("Литер")]
        Letter = 3
    }
}