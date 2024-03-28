/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Протокол деятельности ТСЖ ГЖИ"
///     /// </summary>
///     public class ActivityTsjProtocolMap : BaseGkhEntityMap<ActivityTsjProtocol>
///     {
///         public ActivityTsjProtocolMap() : base("GJI_ACT_TSJ_PROTOCOL")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(50);
///             Map(x => x.PercentageParticipant, "PERC_PARTICIPANT");
///             Map(x => x.VotesDate, "VOTES_DATE");
///             Map(x => x.CountVotes, "COUNT_VOTES");
///             Map(x => x.GeneralCountVotes, "GENERAL_COUNT_VOTES");
/// 
///             References(x => x.ActivityTsj, "ACTIVITY_TSJ_ID").Not.Nullable().Fetch.Join();
///             References(x => x.KindProtocolTsj, "KIND_PROTOCOL_TSJ_ID").Fetch.Join();
///             References(x => x.FileBulletin, "FILE_BULLETIN_ID").Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Протокол деятельности ТСЖ"</summary>
    public class ActivityTsjProtocolMap : BaseEntityMap<ActivityTsjProtocol>
    {
        
        public ActivityTsjProtocolMap() : 
                base("Протокол деятельности ТСЖ", "GJI_ACT_TSJ_PROTOCOL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата протокола").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNum, "Номер протокола").Column("DOCUMENT_NUM").Length(50);
            Property(x => x.PercentageParticipant, "Доля участников").Column("PERC_PARTICIPANT");
            Property(x => x.VotesDate, "Дата голосования").Column("VOTES_DATE");
            Property(x => x.CountVotes, "Количество голосов").Column("COUNT_VOTES");
            Property(x => x.GeneralCountVotes, "Общее количество голосов").Column("GENERAL_COUNT_VOTES");
            Reference(x => x.ActivityTsj, "Деятельность ТСЖ").Column("ACTIVITY_TSJ_ID").NotNull().Fetch();
            Reference(x => x.KindProtocolTsj, "Вид протокола ТСЖ").Column("KIND_PROTOCOL_TSJ_ID").Fetch();
            Reference(x => x.FileBulletin, "Файл бюллетень голосования").Column("FILE_BULLETIN_ID").Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
