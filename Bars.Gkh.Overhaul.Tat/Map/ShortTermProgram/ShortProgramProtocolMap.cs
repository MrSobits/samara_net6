/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Overhaul.Tat.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Протокол краткосрочки"
///     /// </summary>
///     public class ShortProgramProtocolMap : BaseEntityMap<ShortProgramProtocol>
///     {
///         public ShortProgramProtocolMap()
///             : base("OVRHL_SHORT_PROG_PROTOCOL")
///         {
///             Map(x => x.DocumentName, "DOCUMENT_NAME").Length(300);
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(300);
///             Map(x => x.CountAccept, "COUNT_ACCEPT");
///             Map(x => x.CountVote, "COUNT_VOTE");
///             Map(x => x.CountVoteGeneral, "COUNT_VOTE_GENERAL");
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
///             Map(x => x.DateFrom, "DATE_FROM");
///             Map(x => x.GradeOccupant, "GRADE_OCCUPANT");
///             Map(x => x.GradeClient, "GRADE_CLIENT");
///             Map(x => x.SumActVerificationOfCosts, "SUM_ACT_VER_OF_COSTS");
/// 
///             References(x => x.ShortObject, "SHORT_OBJ_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Contragent, "CONTRAGENT_ID").Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.ShortProgramProtocol"</summary>
    public class ShortProgramProtocolMap : BaseEntityMap<ShortProgramProtocol>
    {
        
        public ShortProgramProtocolMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.ShortProgramProtocol", "OVRHL_SHORT_PROG_PROTOCOL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentName, "DocumentName").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentNum, "DocumentNum").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.CountAccept, "CountAccept").Column("COUNT_ACCEPT");
            Property(x => x.CountVote, "CountVote").Column("COUNT_VOTE");
            Property(x => x.CountVoteGeneral, "CountVoteGeneral").Column("COUNT_VOTE_GENERAL");
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(2000);
            Property(x => x.DateFrom, "DateFrom").Column("DATE_FROM");
            Property(x => x.GradeOccupant, "GradeOccupant").Column("GRADE_OCCUPANT");
            Property(x => x.GradeClient, "GradeClient").Column("GRADE_CLIENT");
            Property(x => x.SumActVerificationOfCosts, "SumActVerificationOfCosts").Column("SUM_ACT_VER_OF_COSTS");
            Reference(x => x.ShortObject, "ShortObject").Column("SHORT_OBJ_ID").NotNull().Fetch();
            Reference(x => x.Contragent, "Contragent").Column("CONTRAGENT_ID").Fetch();
            Reference(x => x.File, "File").Column("FILE_ID").Fetch();
        }
    }
}
