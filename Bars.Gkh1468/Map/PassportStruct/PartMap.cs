/// <mapping-converter-backup>
/// namespace Bars.Gkh1468.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.Gkh1468.Entities;
/// 
///     public class PartMap : BaseGkhEntityByCodeMap<Part>
///     {
///         public PartMap()
///             : base("GKH_PSTRUCT_PART")
///         {
///             Map(x => x.Code, "CODE", false);
///             Map(x => x.Name, "NAME");
///             Map(x => x.OrderNum, "ORDER_NUM", false);
///             Map(x => x.Uo, "UO", false);
///             Map(x => x.Pku, "PKU", false);
///             Map(x => x.Pr, "PR", false);
///             Map(x => x.IntegrationCode, "INTEGRATION_CODE", false);
///             References(x => x.Parent, "PARENT_ID");
///             References(x => x.Struct, "PASSPORT_STRUCT_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh1468.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh1468.Entities;
    
    
    /// <summary>Маппинг для "Раздел структуры паспорта"</summary>
    public class PartMap : BaseEntityMap<Part>
    {
        
        public PartMap() : 
                base("Раздел структуры паспорта", "GKH_PSTRUCT_PART")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(36);
            Reference(x => x.Struct, "Структура паспорта, которому принадлежит раздел").Column("PASSPORT_STRUCT_ID");
            Property(x => x.Code, "Код раздела").Column("CODE").Length(250);
            Property(x => x.OrderNum, "Порядок, задаваемый вручную").Column("ORDER_NUM");
            Property(x => x.Name, "Наименование раздела").Column("NAME").Length(250);
            Reference(x => x.Parent, "Родительский раздел").Column("PARENT_ID");
            Property(x => x.Uo, "Заполняется УО").Column("UO");
            Property(x => x.Pku, "Заполняется ПКУ").Column("PKU");
            Property(x => x.Pr, "Заполняется ПР").Column("PR");
            Property(x => x.IntegrationCode, "Код интеграции").Column("INTEGRATION_CODE").Length(250);
        }
    }
}
