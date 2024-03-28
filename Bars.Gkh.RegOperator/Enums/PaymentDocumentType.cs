namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип документа на оплату
    /// </summary>
    public enum PaymentDocumentType
    {
        /// <summary>
        /// Документ на оплату физ.лица
        /// </summary>
        [Display("Документ на оплату физ.лица")]
        Individual = 0x0,

        /// <summary>
        /// Документ на оплату юр.лица
        /// </summary>
        [Display("Документ на оплату юр.лица")]
        Legal = 0x1,

        /// <summary>
        /// Документ на оплату юр.лица с реестром
        /// </summary>
        [Display("Документ на оплату юр.лица с реестром")]
        RegistryLegal = 0x2
    }  
}