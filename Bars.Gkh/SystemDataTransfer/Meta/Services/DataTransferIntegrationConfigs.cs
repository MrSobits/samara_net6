namespace Bars.Gkh.SystemDataTransfer.Meta.Services
{
    using Bars.B4.Config;
    using Bars.B4.Utils.Annotations;

    public class DataTransferIntegrationConfigs
    {
        /// <summary>
        /// Адрес внешней системы
        /// </summary>
        public string RemoteAddress { get; }

        /// <summary>
        /// Токен авторизации
        /// </summary>
        public string AccessToken { get; }

        public DataTransferIntegrationConfigs(IConfigProvider configProvider)
        {
            var appSettings = configProvider.GetConfig().AppSettings;

            ArgumentChecker.InCollection(appSettings, "DataTransfer.RemoteAddress", "DataTransfer.RemoteAddress");
            ArgumentChecker.InCollection(appSettings, "DataTransfer.AccessToken", "DataTransfer.AccessToken");

            this.RemoteAddress = appSettings.GetAs<string>("DataTransfer.RemoteAddress").TrimEnd('/') + "/ws/DataTransfer.svc";
            this.AccessToken = appSettings.GetAs<string>("DataTransfer.AccessToken");
        }
    }
}