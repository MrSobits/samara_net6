namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Маппинг полей сущности <see cref="WarningDoc"/>
    /// </summary>
    /// <remarks>
    /// Этот файл сгенерирован автоматичеески
    /// 26.02.2018 14:06:18
    /// </remarks>
    public class WarningDocMap : JoinedSubClassMap<WarningDoc>
    {
        /// <inheritdoc />
        public WarningDocMap()
            : base("Предостережение ГЖИ", "GJI_WARNING_DOC")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.TakingDate, "Срок принятия мер о соблюдении требований").Column("TAKING_DATE");
            this.Property(x => x.ResultText, "Результат").Column("RESULT_TEXT");
            this.Property(x => x.BaseWarning, "Основание предостережения").Column("BASE_WARNING");
            this.Property(x => x.NcOutDate, "Дата документа (Уведомление о направлении предостережения)").Column("NC_OUT_DATE");
            this.Property(x => x.NcOutNum, "Номер документа (Уведомление о направлении предостережения)").Column("NC_OUT_NUM");
            this.Property(x => x.NcOutDateLatter, "Дата исходящего пиьма  (Уведомление о направлении предостережения)").Column("NC_OUT_DATE_LATTER");
            this.Property(x => x.NcOutNumLatter, "Номер исходящего письма  (Уведомление о направлении предостережения)").Column("NC_OUT_NUM_LATTER");
            this.Property(x => x.NcOutSent, "Уведомление отправлено (Уведомление о направлении предостережения)").Column("NC_OUT_SENT");
            this.Property(x => x.NcInDate, "Дата документа (Уведомление об устранении нарушений)").Column("NC_IN_DATE");
            this.Property(x => x.NcInNum, "Номер документа (Уведомление об устранении нарушений)").Column("NC_IN_NUM");
            this.Property(x => x.NcInDateLatter, "Дата исходящего пиьма  (Уведомление об устранении нарушений)").Column("NC_IN_DATE_LATTER");
            this.Property(x => x.NcInNumLatter, "Номер исходящего письма  (Уведомление об устранении нарушений)").Column("NC_IN_NUM_LATTER");
            this.Property(x => x.NcInRecived, "Уведомление отправлено (Уведомление об устранении нарушений)").Column("NC_IN_RECIVED");
            this.Property(x => x.ObjectionReceived, "Получено возражение").Column("OBJECTION_RECEIVED");
            this.Property(x => x.ActionStartDate, "Дата начала проведения мероприятия").Column("ACTION_START_DATE");
            this.Property(x => x.ActionEndDate, "Дата окончания проведения мероприятия").Column("ACTION_END_DATE");
            this.Property(x => x.ErknmRegistrationNumber, "Учетный номер предостережения в ЕРКНМ").Column("ERKNM_REGISTRATION_NUMBER");
            this.Property(x => x.ErknmRegistrationDate, "Дата присвоения учетного номера / идентификатора ЕРКНМ").Column("ERKNM_REGISTRATION_DATE");
            this.Reference(x => x.CompilationPlace, "Место составления").Column("COMPILATION_PLACE_ID");
            this.Reference(x => x.Autor, "Должностное лицо (ДЛ) вынесшее распоряжение").Column("AUTOR_ID");
            this.Reference(x => x.Executant, "Ответственный за исполнение").Column("EXECUTANT_ID");
            this.Reference(x => x.File, "Документ основания").Column("FILE_ID");
        }
    }
}
