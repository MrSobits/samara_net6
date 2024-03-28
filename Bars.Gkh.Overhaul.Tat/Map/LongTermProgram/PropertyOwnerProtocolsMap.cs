namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;

    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.PropertyOwnerProtocols"</summary>
    public class PropertyOwnerProtocolsMap : BaseEntityMap<PropertyOwnerProtocols>
    {
        public PropertyOwnerProtocolsMap()
            :
                base("Bars.Gkh.Overhaul.Tat.Entities.PropertyOwnerProtocols", "OVRHL_PROP_OWN_PROTOCOLS")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.DocumentDate, "DocumentDate").Column("DOCUMENT_DATE");
            this.Property(x => x.DocumentNumber, "DocumentNumber").Column("DOCUMENT_NUMBER");
            this.Property(x => x.Description, "Description").Column("DESCRIPTION");
            this.Property(x => x.NumberOfVotes, "NumberOfVotes").Column("NUMBER_OF_VOTES");
            this.Property(x => x.TotalNumberOfVotes, "TotalNumberOfVotes").Column("TOTAL_NUMBER_OF_VOTES");
            this.Property(x => x.PercentOfParticipating, "PercentOfParticipating").Column("PERCENT_OF_PARTICIPATE");
            this.Property(x => x.TypeProtocol, "TypeProtocol").Column("TYPE_PROTOCOL").NotNull();
            this.Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            this.Reference(x => x.DocumentFile, "DocumentFile").Column("DOCUMENT_FILE_ID").Fetch();
            this.Property(x => x.LoanAmount, "Сумма займа").Column("LOAN_AMOUNT");
            this.Reference(x => x.Borrower, "Заемщик").Column("BORROWER_CONTRAGENT_ID");
            this.Reference(x => x.Lender, "Кредитор").Column("LENDER_CONTRAGENT_ID");

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

            this.Property(x => x.NpaName, "Наименование").Column("NPA_NAME");
            this.Property(x => x.NpaDate, "Дата принятия документа").Column("NPA_DATE");
            this.Property(x => x.NpaNumber, "Номер").Column("NPA_NUMBER");
            this.Property(x => x.NpaStatus, "Статус").Column("NPA_STATUS");
            this.Property(x => x.NpaCancellationReason, "Причина аннулирования").Column("NPA_CANCELLATION_REASON");
            this.Reference(x => x.TypeInformationNpa, "Тип информации в НПА").Column("TYPE_INFORMATION_NPA_ID");
            this.Reference(x => x.TypeNpa, "Тип НПА").Column("TYPE_NPA_ID");
            this.Reference(x => x.TypeNormativeAct, "Вид нормативного акта").Column("TYPE_NORMATIVE_ACT_ID");
            this.Reference(x => x.NpaContragent, "Орган, принявший НПА").Column("NPA_CONTRAGENT_ID");
            this.Reference(x => x.NpaFile, "Файл").Column("NPA_FILE_ID").Fetch();
        }
    }
}