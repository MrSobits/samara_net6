namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Представление "КЭ""</summary>
    public class ViewStructElementRealObjMap : PersistentObjectMap<ViewStructElementRealObj>
    {
        
        public ViewStructElementRealObjMap() : 
                base("Представление \"КЭ\"", "VIEW_STRUCT_EL_RO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.RealityObjectId, "Идентификатор жилого дома").Column("RO_ID");
            Property(x => x.Ooi, "Наименование группы конструктивного элемента (ООИ)").Column("OOI");
            Property(x => x.NameStructEl, "Наименование КЭ").Column("NAME_SE");
            Property(x => x.LastYear, "Последний год кап ремонта или год установки").Column("LAST_YEAR");
            Property(x => x.Wearout, "Износ").Column("WEAROUT");
            Property(x => x.Volume, "Объем").Column("VOLUME");
            Property(x => x.UnitMeasure, "Единица измерения").Column("UNIT_MEASURE");
        }
    }
}
