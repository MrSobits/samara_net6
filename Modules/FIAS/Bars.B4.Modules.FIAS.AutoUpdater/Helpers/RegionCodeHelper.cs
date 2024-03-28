namespace Bars.B4.Modules.FIAS.AutoUpdater.Helpers
{
    using System.Configuration;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;

    /// <summary>
    /// Вспомогательный класс для получения кода региона
    /// </summary>
    internal static class RegionCodeHelper
    {
        private static string _regionCode;
        
        /// <summary>
        /// Получить код региона
        /// </summary>
        internal static string GetRegionCode()
        {
            if (!string.IsNullOrEmpty(RegionCodeHelper._regionCode))
            {
                return RegionCodeHelper._regionCode;
            }
            
            var container = ApplicationContext.Current.Container;
            var configProvider = container.Resolve<IConfigProvider>();
            var config = configProvider.GetConfig().GetModuleConfig("Bars.B4.Modules.FIAS.AutoUpdater");
            var regionCode = config.GetAs("RegionCode", default(string), true);
            var code = 0;
            
            if (int.TryParse(regionCode, out code))
            {
                RegionCodeHelper._regionCode = regionCode;

                return RegionCodeHelper._regionCode;
            }

            var fiasRepository = container.Resolve<IRepository<Fias>>();
            using (container.Using(fiasRepository))
            {
                RegionCodeHelper._regionCode = fiasRepository.GetAll().Select(x => x.CodeRegion).FirstOrDefault(x => x != null && x != string.Empty);

                if (string.IsNullOrEmpty(RegionCodeHelper._regionCode))
                {
                    throw new ConfigurationException("Не удалось определить код региона для интеграции с ФИАС");
                }

                return RegionCodeHelper._regionCode;
            }
        }
    }
}