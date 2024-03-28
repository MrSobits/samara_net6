/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Конструктивный элемент"
///     /// </summary>
///     public class ConstructiveElementMap : BaseGkhEntityMap<ConstructiveElement>
///     {
///         public ConstructiveElementMap()
///             : base("GKH_DICT_CONST_ELEMENT")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.Code, "CODE").Not.Nullable().Length(300);
///             Map(x => x.Grp, "GRP").Length(100);
///             Map(x => x.Lifetime, "LIFETIME");
///             Map(x => x.RepairCost, "COST_REPAIR");
/// 
///             References(x => x.NormativeDoc, "NORM_DOC_ID").Fetch.Join();
///             References(x => x.Group, "GROUP_ID").Fetch.Join();
///             References(x => x.UnitMeasure, "UNIT_MEASURE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Конструктивный элемент"</summary>
    public class ConstructiveElementMap : BaseImportableEntityMap<ConstructiveElement>
    {
        
        public ConstructiveElementMap() : 
                base("Конструктивный элемент", "GKH_DICT_CONST_ELEMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300).NotNull();
            Property(x => x.Grp, "Группа старая (удалить)").Column("GRP").Length(100);
            Property(x => x.Lifetime, "Срок эксплуатации").Column("LIFETIME");
            Property(x => x.RepairCost, "Стоимость ремонта").Column("COST_REPAIR");
            Reference(x => x.NormativeDoc, "Нормативный документ").Column("NORM_DOC_ID").Fetch();
            Reference(x => x.Group, "Группа").Column("GROUP_ID").Fetch();
            Reference(x => x.UnitMeasure, "Единица измерения").Column("UNIT_MEASURE_ID").Fetch();
        }
    }
}
