/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Исполнитель документа ГЖИ"
///     /// </summary>
///     public class ExecutantDocGjiMap : BaseGkhEntityMap<ExecutantDocGji>
///     {
///         public ExecutantDocGjiMap(): base("GJI_DICT_EXECUTANT")
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
    
    
    /// <summary>Маппинг для "Исполнитель документа ГЖИ"</summary>
    public class ExecutantDocGjiMap : BaseEntityMap<ExecutantDocGji>
    {
        
        public ExecutantDocGjiMap() : 
                base("Исполнитель документа ГЖИ", "GJI_DICT_EXECUTANT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            this.Property(x => x.Code, "Код").Column("CODE").Length(300);
            this.Property(x => x.ErknmCode, "Код в ЕРКНМ").Column("ERKNM_CODE");
        }
    }
}
