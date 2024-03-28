namespace Bars.GisIntegration.Gkh
{
    using Bars.B4.Config;
    using Bars.B4.Utils;

    /// <summary>
    /// Настройки модуля
    /// </summary>
    public static class Settings
    {
        private const string ModuleName = "Bars.Gkh.Ris";
        private const string ConfigRisUrl = "RisUrl";

        /// <summary>
        /// Путь к системе RIS
        /// </summary>
        public static string RisUrl { get; private set; }

        /// <summary>
        /// Инициализация настроек
        /// </summary>
        /// <param name="configProvider">Провайдер настроек</param>
        public static void Init(IConfigProvider configProvider)
        {
            var config = configProvider.GetConfig();
            DynamicDictionary moduleConfig;
            if (config.ModulesConfig.TryGetValue(Settings.ModuleName, out moduleConfig))
            {
                Settings.RisUrl = moduleConfig.GetAs<string>(Settings.ConfigRisUrl);
            }
        }
    }
}