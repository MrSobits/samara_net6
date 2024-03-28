namespace Bars.Gkh.Modules.Gkh1468.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид договора (сторона договора)
    /// </summary>
    public enum TypeContractPart
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
        RsoAndServicePerformerContract = 30,

        /// <summary>
        /// РСО и бюджетные организации
        /// </summary>
        [Display("РСО и бюджетные организации")]
        BudgetOrgContract = 40,

        /// <summary>
        /// РСО и поставщик топливно-энергетических ресурсов
        /// </summary>
        [Display("РСО и поставщик топливно-энергетических ресурсов")]
        FuelEnergyResourceContract = 50
    }
}
