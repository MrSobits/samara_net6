namespace Bars.GkhGji.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Рапоряжение ГЖИ"</summary>
    public class DecisionMap : GkhJoinedSubClassMap<Decision>
    {

        public DecisionMap() : base("GJI_DICISION")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.TypeDisposal, "Тип распоряжения").Column("TYPE_DICISION").NotNull();
            this.Property(x => x.KindKNDGJI, "Вид контроля/надзора").Column("KIND_KND").NotNull();
            this.Property(x => x.DateStart, "Дата начала обследования").Column("DATE_START");
            this.Property(x => x.DateEnd, "Дата окончания обследования").Column("DATE_END");
            this.Property(x => x.TypeAgreementProsecutor, "Согласование с прокуратурой").Column("TYPE_AGRPROSECUTOR").NotNull();
            this.Property(x => x.DocumentNumberWithResultAgreement, "Номер документа с результатом согласования").Column("DOCUMENT_NUMBER_WITH_RESULT_AGREEMENT");
            this.Property(x => x.TypeAgreementResult, "Результат согласования").Column("TYPE_AGRRESULT").NotNull();
            this.Property(x => x.DocumentDateWithResultAgreement, "Дата документа с результатом согласования").Column("DOCUMENT_DATE_WITH_RESULT_AGREEMENT");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            this.Property(x => x.ObjectVisitStart, "Выезд на объект с").Column("OBJECT_VISIT_START");
            this.Property(x => x.RiskCategory, "ФИО").Column("TYPE_RISK");
            this.Property(x => x.ObjectVisitEnd, "Выезд на объект по").Column("OBJECT_VISIT_END");
            this.Reference(x => x.KindCheck, "Вид проверки").Column("KIND_CHECK_ID").Fetch();
            this.Reference(x => x.ProcAprooveFile, "Вид проверки").Column("APROOVE_FILE_ID").Fetch();
            this.Reference(x => x.IssuedDisposal, "Должностное лицо (ДЛ) вынесшее распоряжение").Column("ISSUED_DECISION_ID").Fetch();
            this.Property(x => x.TimeVisitStart, "Время начала обследования").Column("TIME_VISIT_START");
            this.Property(x => x.TimeVisitEnd, "Время окончания обследования").Column("TIME_VISIT_END");
            this.Property(x => x.ProcAprooveDate, "Дата согласования").Column("APROOVE_DATE");
            this.Property(x => x.ProcAprooveNum, "Номер согласования").Column("APROOVE_NUMBER");
            this.Property(x => x.ERPID, "Ид в версии").Column("ERPID");
            this.Property(x => x.ERKNMID, "Ид в версии").Column("ERKNMID");
            this.Property(x => x.FioProcAproove, "ФИО специалиста прокуратуры").Column("FIO_DOC_APPROVE");
            this.Property(x => x.PositionProcAproove, "должность специалиста прокуратуры").Column("POSITION_DOC_APPROVE");
            this.Property(x => x.NcNum, "Номер документа").Column("NC_NUM");
            this.Property(x => x.NcDate, "Дата документа").Column("NC_DATE");
            this.Property(x => x.RequirNum, "Номер документа").Column("REQ_NUM");
            this.Property(x => x.RequirDate, "Дата документа").Column("REQ_DATE");
            this.Property(x => x.NcSent, "Уведомление отправлено").Column("NC_SENT");
            this.Reference(x => x.ProsecutorOffice, "Отдел прокуратуры").Column("PROSECUTOR_ID").Fetch();
            this.Property(x => x.ProsecutorDecNumber, "Номер решения прокурора").Column("PROSECUTOR_DEC_NUM").Length(300);
            this.Property(x => x.ProsecutorDecDate, "ProsecutorDecDate").Column("PROSECUTOR_DEC_DATE");
            this.Property(x => x.DateStatement, "DateStatement").Column("DATE_STATEMENT");
            this.Property(x => x.TimeStatement, "TimeStatement").Column("TIME_STATEMENT");
            this.Property(x => x.PeriodCorrect, "PeriodCorrect").Column("PERIOD_CORRECT");
            this.Property(x => x.HourOfProceedings, "Время вынесения решения час").Column("HOUR_OF_DICISION");
            this.Property(x => x.MinuteOfProceedings, "Время вынесения решения мин").Column("MINUTE_OF_DICISION");
        }
    }
}
