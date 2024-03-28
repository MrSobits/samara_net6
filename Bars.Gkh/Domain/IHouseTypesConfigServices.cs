namespace Bars.Gkh.Domain
{
    /// <summary>
    /// Получения конфига "Настройки версии ДПКР с МО"
    /// </summary>
    public interface IHouseTypesConfigService
    {
        /// <summary>
        /// Получения конфига "Дома в программе"
        /// </summary>
        ProxyHouseTypesConfig GetHouseTypesConfig();

    }

    /// <summary>
    /// Прокси "Дома в программе"
    /// </summary>
    public class ProxyHouseTypesConfig
    {
        /// <summary>
        /// Многоквартирный
        /// </summary>
        public bool UseManyApartments { get; set; }

        /// <summary>
        /// Блокированной застройки"
        /// </summary>
        public bool UseBlockedBuilding { get; set; }

        /// <summary>
        /// Индивидуальный
        /// </summary>
        public bool UseIndividual { get; set; }

        /// <summary>
        /// Общежитие
        /// </summary>
        public bool UseSocialBehavior { get; set; }
    }
}
