namespace Bars.GkhGji.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип исполнителя
    /// </summary>
    public enum PayerType
    {
        /// <summary>
        /// Физ. лицо
        /// </summary>
        [Display ("Физическое лицо")]
        Individual = 1,

        /// <summary>
        /// Юр. лицо
        /// </summary>
        [Display("Юридическое лицо")]
        Legal = 2
    }
}