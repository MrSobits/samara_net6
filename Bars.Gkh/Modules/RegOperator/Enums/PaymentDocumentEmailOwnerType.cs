namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    public enum PaymentDocumentEmailOwnerType
    {
        /// <summary>
        /// Всем
        /// </summary>
        [Display("Всем")]
        All = 0,

        /// <summary>
        /// Физ. лицо
        /// </summary>
        [Display("Физ. лицо")]
        Individual = 10,

        /// <summary>
        /// Юр. лицо
        /// </summary>
        [Display("Юр. лицо")]
        Legal = 20
    }
}