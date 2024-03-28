/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Источник поступления обращения граждан"
///     /// </summary>
///     public class AppealSourcesMap : BaseGkhEntityMap<AppealCitsSource>
///     {
///         public AppealSourcesMap()
///             : base("GJI_APPEAL_SOURCES")
///         {
///             Map(x => x.RevenueDate, "REVENUE_DATE");
///             Map(x => x.RevenueSourceNumber, "REVENUE_SOURCE_NUMBER").Length(50);
/// 
///             References(x => x.RevenueSource, "REVENUE_SOURCE_ID").Fetch.Join();
///             References(x => x.RevenueForm, "REVENUE_FORM_ID").Fetch.Join();
///             References(x => x.AppealCits, "APPCIT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Источник поступления обращения"</summary>
    public class AppealCitsSourceMap : BaseEntityMap<AppealCitsSource>
    {
        
        public AppealCitsSourceMap() : 
                base("Источник поступления обращения", "GJI_APPEAL_SOURCES")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.RevenueDate, "Дата поступления").Column("REVENUE_DATE");
            Property(x => x.SSTUDate, "Дата ССТУ").Column("SSTU_DATE");
            Property(x => x.RevenueSourceNumber, "Исх. № источника поступления").Column("REVENUE_SOURCE_NUMBER").Length(50);
            Reference(x => x.RevenueSource, "Источник поступления").Column("REVENUE_SOURCE_ID").Fetch();
            Reference(x => x.RevenueForm, "Форма поступления").Column("REVENUE_FORM_ID").Fetch();
            Reference(x => x.AppealCits, "Обращение граждан").Column("APPCIT_ID").NotNull().Fetch();
        }
    }
}
