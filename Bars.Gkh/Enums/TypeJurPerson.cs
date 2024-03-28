namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    public enum TypeJurPerson
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        [Display("Управляющая организация")]
        ManagingOrganization = 10,

        /// <summary>
        /// Поставщик коммунальных услуг
        /// </summary>
        [Display("Поставщик коммунальных услуг")]
        SupplyResourceOrg = 20,

        /// <summary>
        /// Орган местного самоуправления
        /// </summary>
        [Display("Орган местного самоуправления")]
        LocalGovernment = 30,

        /// <summary>
        /// Орган государственной власти
        /// </summary>
        [Display("Орган государственной власти")]
        PoliticAuthority = 40,

        /// <summary>
        /// Подрядчик
        /// </summary>
        [Display("Подрядчик")]
        Builder = 50,

        /// <summary>
        /// Поставщик жилищных услуг
        /// </summary>
        [Display("Поставщик жилищных услуг")]
        ServOrg = 60,

        /// <summary>
        /// Организация-арендатор
        /// </summary>
        [Display("Организация-арендатор")]
        RenterOrg = 70,

        /// <summary>
        /// Региональный оператор
        /// </summary>
        [Display("Региональный оператор")]
        RegOp = 80,

        /// <summary>
        /// Обслуживающая компания
        /// </summary>
        [Display("Обслуживающая компания")]
        ServiceCompany = 90,

        /// <summary>
        /// ТСЖ, ЖСК, специализированный кооператив
        /// </summary>
        [Display("ТСЖ, ЖСК, специализированный кооператив")]
        Tsj = 100,

        /// <summary>
        /// Организация - собственник
        /// </summary>
        [Display("Организация - собственник")]
        Owner = 110,

        /// <summary>
        /// Ресурсоснабжающая организация
        /// </summary>
        [Display("Ресурсоснабжающая организация")]
        ResourceCompany = 120,

        /// <summary>
        /// Поставщик ресурсов
        /// </summary>
        [Display("Поставщик ресурсов")]
        PublicServiceOrg = 130
    }
}