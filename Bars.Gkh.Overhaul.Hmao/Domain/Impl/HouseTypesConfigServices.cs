namespace Bars.Gkh.Overhaul.Hmao.Domain.Impl
{
    using Bars.Gkh.Domain;
    using Gkh.Utils;
    using Castle.Windsor;
    using ConfigSections;

    /// <summary>
    /// Получения конфига "Настройки версии ДПКР с МО"
    /// </summary>
    public class HouseTypesConfigServices : IHouseTypesConfigService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получения конфига "Дома в программе"
        /// </summary>
        public ProxyHouseTypesConfig GetHouseTypesConfig()
        {
            var houseTypesConfig = this.Container.GetGkhConfig<OverhaulHmaoConfig>().HouseTypesConfig;

            return new ProxyHouseTypesConfig
            {
                UseBlockedBuilding = houseTypesConfig.UseBlockedBuilding,
                UseIndividual = houseTypesConfig.UseIndividual,
                UseManyApartments = houseTypesConfig.UseManyApartments,
                UseSocialBehavior = houseTypesConfig.UseSocialBehavior
            };
        }
    }
}