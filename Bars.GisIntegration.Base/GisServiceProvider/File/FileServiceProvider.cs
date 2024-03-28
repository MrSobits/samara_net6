namespace Bars.GisIntegration.Base.GisServiceProvider.File
{
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Service;
    using Bars.GisIntegration.Base.Service.Impl;

    /// <summary>
    /// Service provider для файлового сервиса
    /// </summary>
    public class FileServiceProvider: IGisServiceProvider
    {
        private string serviceAddress;

        /// <summary>
        /// Сервис для работы с настройками интеграции ГИС
        /// </summary>
        public IGisSettingsService GisSettingsService { get; set; }

        /// <summary>
        /// Значение перечисления, соответствующее сервису
        /// </summary>
        public IntegrationService IntegrationService => IntegrationService.File;

        /// <summary>
        /// Адрес сервиса
        /// </summary>
        public string ServiceAddress
        {
            get
            {
                if (string.IsNullOrEmpty(this.serviceAddress))
                {
                    var context = this.GisSettingsService.GetContext(AttachmentContext.Current.FileStorageName);
                    var address = this.GisSettingsService.GetServiceAddress(IntegrationService.File, false, "http://127.0.0.1:8080/ext-bus-file-store-service/rest");
                    this.serviceAddress = $"{address.TrimEnd('/')}/{context}";
                }

                return this.serviceAddress;
            }
        }
    }
}
