namespace Bars.GkhGji.Regions.Tatarstan.Map.Resolution
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolMvdMap"</summary>
    public class TatarstanProtocolMvdMap :  JoinedSubClassMap<TatarstanProtocolMvd>
    {
        public TatarstanProtocolMvdMap() : 
            base("Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolMvd", "GJI_TATARSTAN_PROTOCOL_MVD")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.SurName, "SurName").Column("SUR_NAME").Length(255);
            this.Property(x => x.Name, "Name").Column("FIRST_NAME").Length(255);
            this.Property(x => x.Patronymic, "Patronymic").Column("PATRONYMIC");
            this.Reference(x => x.Citizenship, "Citizenship").Column("GJI_DICT_CITIZENSHIP_ID");
            this.Property(x => x.CitizenshipType, "CitizenshipType").Column("CITIZENSHIP_TYPE");
            this.Property(x => x.Commentary, "Commentary").Column("ADDITIONAL_INFO").Length(255);
        }
    }
}