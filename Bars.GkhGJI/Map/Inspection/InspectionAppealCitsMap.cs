/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     public class InspectionAppealCitsMap : BaseGkhEntityMap<InspectionAppealCits>
///     {
///         public InspectionAppealCitsMap()
///             : base("GJI_BASESTAT_APPCIT")
///         {
///             References(x => x.AppealCits, "GJI_APPCIT_ID").Not.Nullable().LazyLoad();
///             References(x => x.Inspection, "INSPECTION_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Обращение граждан проверки по обращениям граджан Это просто таблица связи обращения и Проверки по обращению Непутать с InspectionGji"</summary>
    public class InspectionAppealCitsMap : BaseEntityMap<InspectionAppealCits>
    {
        
        public InspectionAppealCitsMap() : 
                base("Обращение граждан проверки по обращениям граджан Это просто таблица связи обращен" +
                        "ия и Проверки по обращению Непутать с InspectionGji", "GJI_BASESTAT_APPCIT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.AppealCits, "обращение граждан").Column("GJI_APPCIT_ID").NotNull();
            Reference(x => x.Inspection, "Основание обращение граждан ГЖИ").Column("INSPECTION_ID").NotNull().Fetch();
        }
    }
}
