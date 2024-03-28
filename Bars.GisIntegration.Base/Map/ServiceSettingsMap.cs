namespace Bars.GisIntegration.Base.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Маппинг для <see cref="ServiceSettings"/>
    /// </summary>
    public class ServiceSettingsMap : BaseEntityMap<ServiceSettings>
    {
        public ServiceSettingsMap()
            : base("Настройки сервиса - Bars.GisIntegration.Base.Entities.ServiceSettings", "GI_SERVICE_SETTINGS")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.IntegrationService, "Сервис интеграции").Column("INTEGRATION_SERVICE").NotNull();
            this.Property(x => x.ServiceAddress, "Адрес сервиса").Column("ADDRESS").Length(255);
            this.Property(x => x.AsyncServiceAddress, "Адрес асинхронного сервиса").Column("ASYNC_ADDRESS").Length(255);
        }
    }
}