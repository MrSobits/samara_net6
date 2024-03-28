namespace Bars.GisIntegration.Base.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Настройка сервиса
    /// </summary>
    public class ServiceSettings : BaseEntity
    {
        /// <summary>
        /// Сервис интеграции
        /// </summary>
        public virtual IntegrationService IntegrationService { get; set; }

        /// <summary>
        /// Адрес сервиса
        /// </summary>
        public virtual string ServiceAddress { get; set; }

        /// <summary>
        /// Адрес асинхронного сервиса
        /// </summary>
        public virtual string AsyncServiceAddress { get; set; }
    }
}