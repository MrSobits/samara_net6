/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Деятельность ТСЖ ГЖИ"
///     /// </summary>
///     public class ActivityTsjMap : BaseGkhEntityMap<ActivityTsj>
///     {
///         public ActivityTsjMap() : base("GJI_ACTIVITY_TSJ")
///         {
///             References(x => x.ManagingOrganization, "MANAGING_ORG_ID").Not.Nullable().LazyLoad();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Деятельности ТСЖ (Не путать с проверкой по дейтельности ТСЖ)"</summary>
    public class ActivityTsjMap : BaseEntityMap<ActivityTsj>
    {
        
        public ActivityTsjMap() : 
                base("Деятельности ТСЖ (Не путать с проверкой по дейтельности ТСЖ)", "GJI_ACTIVITY_TSJ")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MANAGING_ORG_ID").NotNull();
        }
    }
}
