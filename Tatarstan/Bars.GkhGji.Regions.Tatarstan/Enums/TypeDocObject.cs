namespace Bars.GkhGji.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип объекта
    /// </summary>
    public enum TypeDocObject
    {
        /// <summary>
        /// Физ. лицо
        /// </summary>
        [Display("Физическое лицо")]
        Individual = 1,

        /// <summary>
        /// Юр. лицо
        /// </summary>
        [Display("Юридическое лицо")]
        Legal = 2,

        /// <summary>
        /// Должностное лицо
        /// </summary>
        [Display("Должностное лицо")]
        Official = 3,

        /// <summary>
        /// Индивидуальный предприниматель
        /// </summary>
        [Display("Индивидуальный предприниматель")]
        Entrepreneur = 4
    }
}