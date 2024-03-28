/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map.Dict
/// {
///     using B4.DataAccess;
///     using Entities.Dict;
/// 
///     public class AuditPurposeGjiMap : BaseEntityMap<AuditPurposeGji>
///     {
///         public AuditPurposeGjiMap()
///             : base("GJI_DICT_AUDIT_PURPOSE")
///         {
///             Map(x => x.Name, "NAME").Length(250);
///             Map(x => x.Code, "CODE").Length(250);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;
    
    
    /// <summary>Маппинг для "Цель проведения проверки"</summary>
    public class AuditPurposeGjiMap : BaseEntityMap<AuditPurposeGji>
    {
        
        public AuditPurposeGjiMap() : 
                base("Цель проведения проверки", "GJI_DICT_AUDIT_PURPOSE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование цели").Column("NAME").Length(250);
            Property(x => x.Code, "Код").Column("CODE").Length(250);
        }
    }
}
