namespace Bars.Gkh.Decisions.Nso.Map
{
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Map;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Протокол решений собственников"</summary>
    public class RealityObjectDecisionProtocolMap : BaseImportableEntityMap<RealityObjectDecisionProtocol>
    {
        public RealityObjectDecisionProtocolMap() :
                base("Протокол решений собственников", "GKH_OBJ_D_PROTOCOL")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ExportId, "ExportId").Column("EXPORT_ID").NotNull();
            this.Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").NotNull();
            this.Reference(x => x.File, "Файл протокола").Column("FILE_ID").Fetch();
            this.Property(x => x.DocumentName, "Наименование документа").Column("DOCUMENT_NAME").Length(300);
            this.Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(50);
            this.Property(x => x.DateStart, "Дата вступления в силу").Column("DATE_START").NotNull();
            this.Property(x => x.ProtocolDate, "Дата протокола").Column("PROTOCOL_DATE").NotNull();
            this.Property(x => x.Description, "Description").Column("DESCR").Length(500);
            this.Property(x => x.VotesTotalCount, "VotesTotalCount").Column("V_TOTAL_COUNT").NotNull();
            this.Property(x => x.VotesParticipatedCount, "VotesParticipatedCount").Column("V_PART_COUNT").NotNull();
            this.Property(x => x.ParticipatedShare, "ParticipatedShare").Column("PART_SHARE").NotNull();
            this.Property(x => x.HasQuorum, "HasQuorum").Column("HAS_QUORUM").NotNull();
            this.Property(x => x.PositiveVotesCount, "PositiveVotesCount").Column("POS_V_COUNT").NotNull();
            this.Property(x => x.DecidedShare, "DecidedShare").Column("DECIDED_SHARE").NotNull();
            this.Property(x => x.AuthorizedPerson, "Уполномоченное лицо").Column("AUTHORIZED_PERSON").Length(200);
            this.Property(x => x.PhoneAuthorizedPerson, "Телефон уполномоченного лица").Column("PHONE_AUTHORIZED_PERSON").Length(200);
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Property(x => x.LetterNumber, "Номер входящего письма").Column("LETTER_NUMBER");
            this.Property(x => x.LetterDate, "Дата входящего письма").Column("LETTER_DATE");
            this.Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID").Length(36);
            this.Property(x => x.GisGkhTransportGuid, "ГИС ЖКХ Transport GUID").Column("GIS_GKH_TRANSPORT_GUID").Length(36);
            this.Property(x => x.GisGkhAttachmentGuid, "ГИС ЖКХ GUID вложения").Column("GIS_GKH_ATTACHMENT_GUID").Length(36);
            this.Property(x => x.FormVoting, "Форма проведения голосования").Column("FORM_VOTING");
            this.Property(x => x.EndDateDecision, "Дата окончания приема решений").Column("END_DATE_DECISION");
            this.Property(x => x.PlaceDecision, "Место приема решений").Column("PLACE_DECISION");
            this.Property(x => x.PlaceMeeting, "Место проведения").Column("PLACE_MEETING");
            this.Property(x => x.DateMeeting, "Дата проведения собрания").Column("DATE_MEETING");
            this.Property(x => x.TimeMeeting, "Время проведения собрания").Column("TIME_MEETING");
            this.Property(x => x.VotingStartDate, "Дата начала проведения голосования").Column("VOTING_START_DATE");
            this.Property(x => x.VotingStartTime, "Время начала проведения голосования").Column("VOTING_START_TIME");
            this.Property(x => x.VotingEndDate, "Дата окончания проведения голосования").Column("VOTING_END_DATE");
            this.Property(x => x.VotingEndTime, "Время окончания проведения голосования").Column("VOTING_END_TIME");
            this.Property(x => x.OrderTakingDecisionOwners, "Порядок приема решений собственников").Column("ORDER_TAKING_DEC_OWNERS");
            this.Property(x => x.OrderAcquaintanceInfo, "Порядок ознакомления с информацией").Column("ORDER_ACQUAINTANCE_INFO");
            this.Property(x => x.AnnuaLMeeting, "Ежегодное собрание").Column("ANNUAL_MEETING");
            this.Property(x => x.LegalityMeeting, "Правомерность собрания").Column("LEGALITY_MEETING");
            this.Property(x => x.VotingStatus, "Статус").Column("VOTING_STATUS");
        }
    }

    /// <summary>ReadOnly ExportId</summary>
    public class RealityObjectDecisionProtocolNhMapping : BaseHaveExportIdMapping<RealityObjectDecisionProtocol>
    {
    }
}
