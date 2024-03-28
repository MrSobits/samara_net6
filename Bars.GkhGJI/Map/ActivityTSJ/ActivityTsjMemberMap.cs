/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     public class ActivityTsjMemberMap : BaseGkhEntityMap<ActivityTsjMember>
///     {
///         public ActivityTsjMemberMap() : base("GJI_ACT_TSJ_MEMBER")
///         {
///             Map(x => x.Year, "YEAR");
///             
///             References(x => x.State, "STATE_ID").Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
/// 
///             References(x => x.ActivityTsj, "ACTIVITY_TSJ_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ActivityTsjMember"</summary>
    public class ActivityTsjMemberMap : BaseEntityMap<ActivityTsjMember>
    {
        
        public ActivityTsjMemberMap() : 
                base("Bars.GkhGji.Entities.ActivityTsjMember", "GJI_ACT_TSJ_MEMBER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Year, "Год").Column("YEAR");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            Reference(x => x.File, "Файл реестра").Column("FILE_ID").Fetch();
            Reference(x => x.ActivityTsj, "Деятельность ТСЖ").Column("ACTIVITY_TSJ_ID").NotNull().Fetch();
        }
    }
}
