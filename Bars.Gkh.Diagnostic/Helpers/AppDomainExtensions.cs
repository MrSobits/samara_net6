namespace Bars.Gkh.Diagnostic.Helpers
{
    using System;
    using System.IO;

    public static class AppDomainExtensions
    {
        /// <summary>
        /// Проверка что текущее приложение Web App
        /// </summary>
        /// <param name="appDomain">
        /// The app domain.
        /// </param>
        /// <returns>
        /// True если WebApp иначе false
        /// </returns>
        public static bool IsWebApp(this AppDomain appDomain)
        {
            var configFile = (string)appDomain.GetData("APP_CONFIG_FILE");
            if (string.IsNullOrEmpty(configFile))
            {
                return false;
            }

            var configFileName = Path.GetFileNameWithoutExtension(configFile);

            var isWeb = configFileName.Equals("WEB", StringComparison.OrdinalIgnoreCase);

            return isWeb;
        }
    }
}
