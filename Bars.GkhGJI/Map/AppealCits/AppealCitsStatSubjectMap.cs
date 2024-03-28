/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map.Inspection
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     public class AppealCitsStatSubjectMap : BaseGkhEntityMap<AppealCitsStatSubject>
///     {
///         public AppealCitsStatSubjectMap() : base("GJI_APPCIT_STATSUBJ")
///         {
///             References(x => x.Subject, "STATEMENT_SUBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Subsubject, "SUBSUBJECT_ID").Fetch.Join();
///             References(x => x.Feature, "FEATURE_ID").Fetch.Join();
///             References(x => x.AppealCits, "APPCIT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Таблица связи тематики, подтематики и характеристики"</summary>
    public class AppealCitsStatSubjectMap : BaseEntityMap<AppealCitsStatSubject>
    {
        
        public AppealCitsStatSubjectMap() : 
                base("Таблица связи тематики, подтематики и характеристики", "GJI_APPCIT_STATSUBJ")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.ExportCode, "Код ССТУ").Column("EXPORT_CODE");
            Reference(x => x.Subject, "Тематика").Column("STATEMENT_SUBJECT_ID").NotNull().Fetch();
            Reference(x => x.Subsubject, "Подтематика").Column("SUBSUBJECT_ID").Fetch();
            Reference(x => x.Feature, "Характеристика").Column("FEATURE_ID").Fetch();
            Reference(x => x.AppealCits, "Обращение граждан").Column("APPCIT_ID").NotNull().Fetch();
        }
    }
}
