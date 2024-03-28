namespace Bars.Gkh.Decisions.Nso.Map.Decisions
{
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Map;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Протокол решения органа государственной власти"</summary>
    public class GovDecisionMap : BaseImportableEntityMap<GovDecision>
    {

        public GovDecisionMap() :
                base("Протокол решения органа государственной власти", "DEC_GOV_DECISION")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ExportId, "ExportId").Column("EXPORT_ID").NotNull();
            this.Reference(x => x.RealityObject, "МКД").Column("RO_ID").NotNull().Fetch();
            this.Property(x => x.ProtocolNumber, "Номер протокола").Column("PROTOCOL_NUM").Length(250);
            this.Property(x => x.ProtocolDate, "Дата портокола").Column("PROTOCOL_DATE");
            this.Property(x => x.DateStart, "Дата вступления в силу").Column("DATE_START");
            this.Property(x => x.AuthorizedPerson, "Уполномоченное лицо").Column("AUTH_PERSON").Length(250);
            this.Property(x => x.RealtyManagement, "Управление домом").Column("REALTY_MANAG").Length(250);
            this.Property(x => x.AuthorizedPersonPhone, "Телефон уполномоченного лица").Column("AUTH_PERSON_PHONE").Length(250);
            this.Reference(x => x.ProtocolFile, "Файл протокола").Column("PROT_FILE_ID").Fetch();
            this.Property(x => x.FundFormationByRegop, "Способ формирования фонда на счету регионального оператора").Column("FUND_BY_REGOP");
            this.Property(x => x.Destroy, "Снос МКД").Column("DESTROY");
            this.Property(x => x.DestroyDate, "Дата сноса МКД").Column("DESTROY_DATE");
            this.Property(x => x.Reconstruction, "Реконструкция МКД").Column("RECONSTRUCT");
            this.Property(x => x.ReconstructionStart, "Дата начала реконструкции").Column("RECONSTR_START");
            this.Property(x => x.ReconstructionEnd, "Дата окончания реконструкции").Column("RECONSTR_END");
            this.Property(x => x.TakeLandForGov, "Изъятие для государственных или муниципальных нужд зумельного участка, на котором" +
                    " расположен МКД").Column("TAKE_LAND");
            this.Property(x => x.TakeLandForGovDate, "Дата изъятия земельного участка").Column("TAKE_LAND_DATE");
            this.Property(x => x.TakeApartsForGov, "Изъятие каждого жилого помещения в доме").Column("TAKE_APARTS");
            this.Property(x => x.TakeApartsForGovDate, "Дата изъятия жилых помещений").Column("TAKE_APARTS_DATE");
            this.Property(x => x.MaxFund, "Максимальный размер фонда").Column("MAX_FUND");
            this.Reference(x => x.State, "Состояние").Column("STATE_ID").NotNull().Fetch();
            this.Property(x => x.LetterNumber, "Номер входящего письма").Column("LETTER_NUMBER");
            this.Property(x => x.LetterDate, "Дата входящего письма").Column("LETTER_DATE");
            this.Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID").Length(36);
            this.Property(x => x.GisGkhTransportGuid, "ГИС ЖКХ Transport GUID").Column("GIS_GKH_TRANSPORT_GUID").Length(36);
            this.Property(x => x.GisGkhAttachmentGuid, "ГИС ЖКХ GUID вложения").Column("GIS_GKH_ATTACHMENT_GUID").Length(36);
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

    /// <summary>ReadOnly ExportId</summary>
    public class GovDecisionNhMapping : BaseHaveExportIdMapping<GovDecision>
    {
    }
}
