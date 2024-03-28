namespace Bars.Gkh.Modules.Gkh1468.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>Маппинг для "Поставщик ресурсов"</summary>
    public class PublicServiceOrgTemperatureInfoMap : BaseImportableEntityMap<PublicServiceOrgTemperatureInfo>
    {
        
        public PublicServiceOrgTemperatureInfoMap() : 
                base("Информация о температурном графике", "GKH_TEMP_GRAPH_INFO")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.OutdoorAirTemp, "Температура наружного воздуха").Column("ORG_STATE_ROLE").NotNull();
            this.Property(x => x.CoolantTempSupplyPipeline, "Температура теплоносителя в подающем трубопроводе").Column("COOLANT_TEMP_SUPPLY").NotNull();
            this.Property(x => x.CoolantTempReturnPipeline, "Температура теплоносителя в обратном трубопроводе").Column("COOLANT_TEMP_RETURN").NotNull();

            this.Reference(x => x.Contract, "Договор поставщика ресурсов с домом").Column("CONTRACT_ID").NotNull().Fetch();
        }
    }
}
