namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Платежный агент"</summary>
    public class ContragentClwMunicipalityMap : BaseEntityMap<ContragentClwMunicipality>
    {
        
        public ContragentClwMunicipalityMap() : 
                base("Платежный агент", "GKH_CONTRAGENT_CLW_MUNICIPALITY")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ContragentClw, "Контрагент ПИР").Column("CONTRAGENT_CLW_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "МО").Column("MUNICIPALITY_ID").NotNull().Fetch();
        }
    }
}
