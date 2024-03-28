namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using Entities;


    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.BaseJurPersonContragent"</summary>
    public class BaseOMSUContragentMap : BaseEntityMap<BaseOMSUContragent>
    {
        
        public BaseOMSUContragentMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.BaseJurPersonContragent", "GJI_CH_BASEOMSU_CONTRAGENT")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.BaseOMSU, "BaseOMSU").Column("BASEOMSU_ID").NotNull();
            this.Reference(x => x.Contragent, "Contragent").Column("CONTRAGENT_ID");
        }
    }
}
