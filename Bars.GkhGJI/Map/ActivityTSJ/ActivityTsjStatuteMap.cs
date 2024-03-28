/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Устав деятельности ТСЖ ГЖИ"
///     /// </summary>
///     public class ActivityTsjStatuteMap : BaseGkhEntityMap<ActivityTsjStatute>
///     {
///         public ActivityTsjStatuteMap() : base("GJI_ACT_TSJ_STATUTE")
///         {
///             Map(x => x.StatuteProvisionDate, "STAT_PROVISION_DATE");
///             Map(x => x.StatuteApprovalDate, "STAT_APPROVAL_DATE");
///             Map(x => x.ConclusionDate, "CONCLUSION_DATE");
///             Map(x => x.ConclusionNum, "CONCLUSION_NUM").Length(50);
///             Map(x => x.ConclusionDescription, "DESCRIPTION").Length(500);
///             Map(x => x.TypeConclusion, "TYPE_CONCLUSION").Not.Nullable().CustomType<TypeConclusion>();
///             References(x => x.ActivityTsj, "ACTIVITY_TSJ_ID").Not.Nullable().Fetch.Join();
///             References(x => x.StatuteFile, "STATUTE_FILE").Fetch.Join();
///             References(x => x.ConclusionFile, "CONCLUSION_FILE").Fetch.Join();
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    /// <summary>
    /// Маппинг для "Устав деятельности ТСЖ"
    /// </summary>
    public class ActivityTsjStatuteMap : BaseEntityMap<ActivityTsjStatute>
    {
        public ActivityTsjStatuteMap() : 
                base("Устав деятельности ТСЖ", "GJI_ACT_TSJ_STATUTE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocNumber, "DocNumber").Column("DOC_NUMBER").Length(50);
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.StatuteProvisionDate, "Дата предоставления устава").Column("STAT_PROVISION_DATE");
            Property(x => x.StatuteApprovalDate, "Дата утверждения устава").Column("STAT_APPROVAL_DATE");
            Property(x => x.ConclusionDate, "Дата заключения").Column("CONCLUSION_DATE");
            Property(x => x.ConclusionNum, "Номер заключения").Column("CONCLUSION_NUM").Length(50);
            Property(x => x.ConclusionDescription, "Описание заключения").Column("DESCRIPTION").Length(500);
            Property(x => x.TypeConclusion, "Тип заключения").Column("TYPE_CONCLUSION").NotNull();
            Reference(x => x.ActivityTsj, "Деятельность ТСЖ").Column("ACTIVITY_TSJ_ID").NotNull().Fetch();
            Reference(x => x.StatuteFile, "Файл устава").Column("STATUTE_FILE").Fetch();
            Reference(x => x.ConclusionFile, "Файл заключения").Column("CONCLUSION_FILE").Fetch();
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}