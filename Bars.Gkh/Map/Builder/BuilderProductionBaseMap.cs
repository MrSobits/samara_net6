/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Производственная база"
///     /// </summary>
///     public class BuilderProductionBaseMap : BaseGkhEntityMap<BuilderProductionBase>
///     {
///         public BuilderProductionBaseMap()
///             : base("GKH_BUILDER_PRODUCTBASE")
///         {
///             Map(x => x.Notation, "NOTATION").Length(300);
///             Map(x => x.Volume, "VOLUME");
/// 
///             References(x => x.KindEquipment, "KIND_EQUIPMENT_ID").Fetch.Join();
///             References(x => x.DocumentRight, "FILE_ID").Fetch.Join();
///             References(x => x.Builder, "BUILDER_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Производственная база"</summary>
    public class BuilderProductionBaseMap : BaseImportableEntityMap<BuilderProductionBase>
    {
        
        public BuilderProductionBaseMap() : 
                base("Производственная база", "GKH_BUILDER_PRODUCTBASE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Notation, "Примечание").Column("NOTATION").Length(300);
            Property(x => x.Volume, "Объем").Column("VOLUME");
            Reference(x => x.KindEquipment, "Вид оснащения").Column("KIND_EQUIPMENT_ID").Fetch();
            Reference(x => x.DocumentRight, "правоустанавливающий документ").Column("FILE_ID").Fetch();
            Reference(x => x.Builder, "Подрядчик").Column("BUILDER_ID").NotNull().Fetch();
        }
    }
}
