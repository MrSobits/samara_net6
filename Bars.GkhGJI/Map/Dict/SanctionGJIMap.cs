/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Санкции"
///     /// </summary>
///     public class SanctionGjiMap : BaseGkhEntityMap<SanctionGji>
///     {
///         public SanctionGjiMap() : base("GJI_DICT_SANCTION")
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
    
    /// <summary>Маппинг для "Санкция ГЖИ"</summary>
    public class SanctionGjiMap : BaseEntityMap<SanctionGji>
    {
        public SanctionGjiMap() : 
                base("Санкция ГЖИ", "GJI_DICT_SANCTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.Code, "Код").Column("CODE").Length(300);
            Property(x => x.ErknmGuid, "Идентификатор в ЕРКНМ").Column("ERKNM_GUID").Length(36);
        }
    }
}
