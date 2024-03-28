/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Форма постапления"
///     /// </summary>
///     public class RevenueFormGjiMap : BaseGkhEntityMap<RevenueFormGji>
///     {
///         public RevenueFormGjiMap()
///             : base("GJI_DICT_REVENUEFORM")
///         {
///             Map(x => x.Name, "NAME").Length(300).Not.Nullable();
///             Map(x => x.Code, "CODE").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Форма поступления"</summary>
    public class RevenueFormGjiMap : BaseEntityMap<RevenueFormGji>
    {
        
        public RevenueFormGjiMap() : 
                base("Форма поступления", "GJI_DICT_REVENUEFORM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
        }
    }
}
