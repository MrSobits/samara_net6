/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "статьи закона предписания"
///     /// </summary>
///     public class PrescriptionArticleLawMap : BaseGkhEntityMap<PrescriptionArticleLaw>
///     {
///         public PrescriptionArticleLawMap()
///             : base("GJI_PRESCRIPTION_ARTLAW")
///         {
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
/// 
///             References(x => x.Prescription, "PRESCRIPTION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ArticleLaw, "ARTICLELAW_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Статьи закона в предписании ГЖИ"</summary>
    public class PrescriptionArticleLawMap : BaseEntityMap<PrescriptionArticleLaw>
    {
        
        public PrescriptionArticleLawMap() : 
                base("Статьи закона в предписании ГЖИ", "GJI_PRESCRIPTION_ARTLAW")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(2000);
            Reference(x => x.Prescription, "Предписание").Column("PRESCRIPTION_ID").NotNull().Fetch();
            Reference(x => x.ArticleLaw, "Статья закона").Column("ARTICLELAW_ID").NotNull().Fetch();
        }
    }
}
