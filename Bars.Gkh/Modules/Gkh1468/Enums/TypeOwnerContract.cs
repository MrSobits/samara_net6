namespace Bars.Gkh.Modules.Gkh1468.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип лица/ организации
    /// </summary>
    public enum TypeOwnerContract
    {
        /// <summary>
        /// Юридическое лицо
        /// </summary>
        [Display("Юридическое лицо")]
        JurPersonOwnerContact = 10,

        /// <summary>
        /// Физическое лицо
        /// </summary>
        [Display("Физическое лицо")]
        IndividualOwnerContract = 20
    }
}
