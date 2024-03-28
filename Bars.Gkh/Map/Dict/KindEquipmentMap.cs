/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "вид оснащения"
///     /// </summary>
///     public class KindEquipmentMap : BaseGkhEntityMap<KindEquipment>
///     {
///         public KindEquipmentMap()
///             : base("GKH_DICT_KINDEQUIPMENT")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
/// 
///             References(x => x.UnitMeasure, "UNIT_MEASURE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Вид оснащения"</summary>
    public class KindEquipmentMap : BaseImportableEntityMap<KindEquipment>
    {
        
        public KindEquipmentMap() : 
                base("Вид оснащения", "GKH_DICT_KINDEQUIPMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Reference(x => x.UnitMeasure, "Ед. измерения").Column("UNIT_MEASURE_ID").NotNull().Fetch();
        }
    }
}
