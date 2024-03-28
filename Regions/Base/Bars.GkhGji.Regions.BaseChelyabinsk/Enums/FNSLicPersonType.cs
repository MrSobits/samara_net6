namespace Bars.GkhGji.Regions.BaseChelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус запроса
    /// </summary>
    public enum FNSLicPersonType
    {

        /// <summary>
        /// ИП
        /// </summary>
        [Display("Индивидуальный предприниматель")]
        IP = 10,

        /// <summary>
        /// Удаление
        /// </summary>
        [Display("Юридическое лицо")]
        Juridical = 20,
    }
}