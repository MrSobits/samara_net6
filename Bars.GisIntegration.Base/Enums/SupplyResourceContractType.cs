namespace Bars.GisIntegration.Base.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид договора с поставщиком ресурсов
    /// </summary>
    public enum SupplyResourceContractType
    {
        /// <summary>
        /// Договор-оферты
        /// </summary>
        [Display("Договор-оферты")]
        OfferContract = 10,

        /// <summary>
        /// РСО и собственник/представитель собственников/арендатор нежилых помещений
        /// </summary>
        [Display("РСО и собственник/представитель собственников/арендатор нежилых помещений")]
        RsoAndOwnersContact = 20,

        /// <summary>
        /// РСО и исполнитель коммунальных услуг
        /// </summary>
        [Display("РСО и исполнитель коммунальных услуг")]
        RsoAndServicePerformerContract = 30
    }
}
