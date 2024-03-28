namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "ТехПаспорт"</summary>
    public class TehPassportMap : BaseImportableEntityMap<TehPassport>
    {
        
        public TehPassportMap() : 
                base("ТехПаспорт", "TP_TEH_PASSPORT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.RealityObject, "Объект недвижимости").Column("REALITY_OBJ_ID").NotNull().Fetch();
        }
    }
}
