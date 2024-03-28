namespace Bars.GisIntegration.UI.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.UI.Entities;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.RisSettings"
    /// </summary>
    public class RisSettingsMap : BaseEntityMap<RisSettings>
    {
        public RisSettingsMap() : 
                base("Bars.Gkh.Ris.Entities.RisSettings", "RIS_SETTINGS")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Code").Column("CODE").Length(50);
            this.Property(x => x.Name, "Name").Column("NAME").Length(100);
            this.Property(x => x.Value, "Value").Column("VALUE").Length(100);
        }
    }
}