namespace Bars.GisIntegration.Base.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Маппинг настроек контекста
    /// </summary>
    public class ContextSettingsMap : BaseEntityMap<ContextSettings>
    {
        /// <summary>
        /// Конструктор маппинга
        /// </summary>
        public ContextSettingsMap()
            : base("Bars.GisIntegration.Base.Entities.ContextSettings", "GI_CONTEXT_SETTINGS")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.FileStorageName, "Наименование хранилища данных ГИС").Column("FILE_STORAGE_NAME").NotNull();
            this.Property(x => x.Context, "Контекст").Column("CONTEXT").Length(100).NotNull();
        }
    }
}
