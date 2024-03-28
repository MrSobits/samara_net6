namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип приложения к предписанию
    /// </summary>
    public enum TypePrescriptionAnnex
    {
        /// <summary>
        /// Иное
        /// </summary>
        [Display("Иное")]
        Other = 0,

        /// <summary>
        /// Предписание
        /// </summary>
        [Display("Предписание")]
        Prescription = 10,

        /// <summary>
        /// Ходатайство о продлении
        /// </summary>
        [Display("Ходатайство о продлении")]
        RenewalApplication = 20,

    }
}