namespace Bars.GisIntegration.Base.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Хранилище данных ГИС
    /// </summary>
    public enum FileStorageName
    {
        /// <summary>
        /// Управление домами, Лицевые счета
        /// </summary>
        [Display("Управление домами, Лицевые счета")]
        HomeManagement = 10,

        /// <summary>
        /// Реестр коммунальной инфраструктуры
        /// </summary>
        [Display("Реестр коммунальной инфраструктуры")]
        Rki = 20,

        /// <summary>
        /// Голосование
        /// </summary>
        [Display("Голосование")]
        Voting = 30,

        /// <summary>
        /// Инспектирование жилищного фонда
        /// </summary>
        [Display("Инспектирование жилищного фонда")]
        Inspection = 40,

        /// <summary>
        /// Оповещения
        /// </summary>
        [Display("Оповещения")]
        Informing = 50,

        /// <summary>
        /// Электронные счета
        /// </summary>
        [Display("Электронные счета")]
        Bills = 60,

        /// <summary>
        /// Лицензии
        /// </summary>
        [Display("Лицензии")]
        Licenses = 70,

        /// <summary>
        /// Договора (ДУ, уставы, ДПОИ)
        /// </summary>
        [Display("Договора (ДУ, уставы, ДПОИ)")]
        Agreements = 80,

        /// <summary>
        /// Нормативно-справочная информация
        /// </summary>
        [Display("Нормативно-справочная информация")]
        Nsi = 90,

        /// <summary>
        /// Капитальный ремонт
        /// </summary>
        [Display("Капитальный ремонт")]
        CapitalRepairPrograms = 110,

        /// <summary>
        /// Раскрытие деятельности УО
        /// </summary>
        [Display("Раскрытие деятельности УО")]
        Disclosure = 120
    }
}