namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид жилищной услуги
    /// </summary>
    public enum TypeHousingResource
    {
        /// <summary>
        /// Плата за содержание и ремонт жилого помещения
        /// </summary>
        [Display("Плата за содержание и ремонт жилого помещения")]
        RealityObjectPay = 10,

        /// <summary>
        /// Взнос на капитальный ремонт
        /// </summary>
        [Display("Взнос на капитальный ремонт")]
        OverhaulPay = 20,

        /// <summary>
        /// Техническое обслуживание
        /// </summary>
        [Display("Техническое обслуживание")]
        Maintenance = 30,

        /// <summary>
        /// Социальный наём
        /// </summary>
        [Display("Социальный наём")]
        SocialRentals = 40,

        /// <summary>
        /// Коммерческий наём
        /// </summary>
        [Display("Коммерческий наём")]
        CommercialRentals = 50,

        /// <summary>
        /// Аренда
        /// </summary>
        [Display("Аренда")]
        Rent = 60,

        /// <summary>
        /// КР по обязательным работам
        /// </summary>
        [Display("КР по обязательным работам")]
        MandatoryWork = 70,

        /// <summary>
        /// КР по дополнительным работам
        /// </summary>
        [Display("КР по дополнительным работам")]
        AdditionalWork = 80
    }
}
