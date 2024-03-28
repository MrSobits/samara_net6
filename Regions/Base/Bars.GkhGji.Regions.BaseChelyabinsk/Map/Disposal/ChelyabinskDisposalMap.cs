/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map
/// {
///     using Bars.Gkh.Enums;
///     using Bars.GkhGji.Regions.Chelyabinsk.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Распоряжение"
///     /// </summary>
///     public class ChelyabinskDisposalMap : SubclassMap<ChelyabinskDisposal>
///     {
///         public ChelyabinskDisposalMap()
///         {
///             Table("GJI_NSO_DISPOSAL");
///             KeyColumn("ID");
/// 
///             Map(x => x.TimeVisitStart, "TIME_VISIT_SART");
///             Map(x => x.TimeVisitEnd, "TIME_VISIT_END");
/// 
///             Map(x => x.DateStatement, "DATE_STATEMENT");
///             Map(x => x.TimeStatement, "TIME_STATEMENT");
/// 
///             Map(x => x.NcNum, "NC_NUM");
///             Map(x => x.NcDate, "NC_DATE");
/// 
///             Map(x => x.NcNumLatter, "NC_NUM_LETTER");
///             Map(x => x.NcDateLatter, "NC_DATE_LETTER");
/// 
///             Map(x => x.MotivatedRequestNumber, "MOTIV_REQUEST_NUM");
///             Map(x => x.MotivatedRequestDate, "MOTIV_REQUEST_DATE");
/// 
///             Map(x => x.PeriodCorrect, "PERIOD_CORRECT");
/// 
///             Map(x => x.NcObtained, "NC_OBTAINED").CustomType<YesNo>();
///             Map(x => x.NcSent, "NC_SENT").CustomType<YesNo>();
/// 
///             Map(x => x.NoticeDateProtocol, "NOTICE_DATE_PROTOCOL");
///             Map(x => x.NoticeTimeProtocol, "NOTICE_TIME_PROTOCOL");
///             Map(x => x.NoticePlaceCreation, "NOTICE_PLACE_CREATION");
///             Map(x => x.NoticeDescription, "NOTICE_DESCRIPTION");
///             Map(x => x.ProsecutorDecNumber, "PROSECUTOR_DEC_NUM").Length(300);
///             Map(x => x.ProsecutorDecDate, "PROSECUTOR_DEC_DATE");
/// 
///             References(x => x.PoliticAuthority, "POLITIC_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Disposal
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.ChelyabinskDisposal"</summary>
    public class ChelyabinskDisposalMap : JoinedSubClassMap<ChelyabinskDisposal>
    {
        
        public ChelyabinskDisposalMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.ChelyabinskDisposal", "GJI_NSO_DISPOSAL")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.DateStatement, "DateStatement").Column("DATE_STATEMENT");
            this.Property(x => x.TimeStatement, "TimeStatement").Column("TIME_STATEMENT");
            this.Property(x => x.MotivatedRequestNumber, "MotivatedRequestNumber").Column("MOTIV_REQUEST_NUM");
            this.Property(x => x.MotivatedRequestDate, "MotivatedRequestDate").Column("MOTIV_REQUEST_DATE");
            this.Property(x => x.PeriodCorrect, "PeriodCorrect").Column("PERIOD_CORRECT");
            this.Property(x => x.NoticeDateProtocol, "NoticeDateProtocol").Column("NOTICE_DATE_PROTOCOL");
            this.Property(x => x.NoticeTimeProtocol, "NoticeTimeProtocol").Column("NOTICE_TIME_PROTOCOL");
            this.Property(x => x.NoticePlaceCreation, "NoticePlaceCreation").Column("NOTICE_PLACE_CREATION");
            this.Property(x => x.NoticeDescription, "NoticeDescription").Column("NOTICE_DESCRIPTION");
            this.Property(x => x.ProsecutorDecNumber, "ProsecutorDecNumber").Column("PROSECUTOR_DEC_NUM").Length(300);
            this.Property(x => x.ProsecutorDecDate, "ProsecutorDecDate").Column("PROSECUTOR_DEC_DATE");
            this.Reference(x => x.PoliticAuthority, "PoliticAuthority").Column("POLITIC_ID").Fetch();
        }
    }
}
