namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Категория информации НПА
    /// </summary>
    public enum CategoryInformationNpa
    {
        /// <summary>
        /// Жилищный фонд
        /// </summary>
        [Display("Жилищный фонд")]
        HousingStock = 0,

        /// <summary>
        /// Управление жилищным фондом
        /// </summary>
        [Display("Управление жилищным фондом")]
        HousingManagement = 1,

        /// <summary>
        /// Коммунальное хозяйство
        /// </summary>
        [Display("Коммунальное хозяйство")]
        CommunalEconomy = 2,

        /// <summary>
        /// Плата за жилое помещение и коммунальные услуги
        /// </summary>
        [Display("Плата за жилое помещение и коммунальные услуги")]
        PaymentResedentialPremises = 3,

        /// <summary>
        /// Информатизация в сфере ЖКХ
        /// </summary>
        [Display("Информатизация в сфере ЖКХ")]
        InformationCommunalService = 4,

        /// <summary>
        /// Контроль и надзор в жилищной сфере
        /// </summary>
        [Display("Контроль и надзор в жилищной сфере")]
        ControlAndSupervision = 5
    }
}