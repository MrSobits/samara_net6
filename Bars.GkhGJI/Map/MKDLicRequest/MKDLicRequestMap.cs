using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    
    
    /// <summary>Маппинг для "Обращение граждан"</summary>
    public class MKDLicRequestMap : BaseEntityMap<MKDLicRequest>
    {
        
        public MKDLicRequestMap() : 
                base("Заявка на внесение изменений в реестр лицензий", "GJI_MKD_LIC_STATEMENT")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Reference(x => x.Contragent, "Контрагент заявитель").Column("CONTRAGENT_ID").Fetch();
            this.Reference(x => x.ExecutantDocGji, "Тип заявителя").Column("EXECUTANT_TYPE_ID").NotNull().Fetch();
            this.Reference(x => x.Inspector, "Исполнитель").Column("INSPECTOR_ID").Fetch();
            this.Reference(x => x.MKDLicTypeRequest, "Тип запроса").Column("TYPE_REQUEST_ID").NotNull().Fetch();
            this.Reference(x => x.StatmentContragent, "Контрагент в заявлении").Column("STMT_CONTRAGENT_ID").Fetch();

            this.Property(x => x.ConclusionDate, "Дата заключения").Column("CONCLUSION_DATE");
            this.Property(x => x.ConclusionNumber, "Номер заключения").Column("CONCLUSION_NUMBER");
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION");
            this.Property(x => x.LicStatementResult, "Результат рассмотрения").Column("STATEMENT_RESULT");
            this.Property(x => x.LicStatementResultComment, "Комментарий к результату").Column("RESULT_COMMENT").Length(1000);
            this.Property(x => x.Objection, "Поступило возражение").Column("OBJECTION");
            this.Property(x => x.ObjectionResult, "Результат обжалования").Column("OBJECTION_RESULT");
            this.Property(x => x.PhysicalPerson, "ФИО заявителя").Column("FIO").Length(300);
            this.Property(x => x.StatementDate, "Дата заявления").Column("STATEMENT_DATE").NotNull();
            this.Property(x => x.StatementNumber, "Номер заявления").Column("STATEMENT_NUMBER").Length(255);

            this.Reference(x => x.ConclusionFile, "Файл решения").Column("CONCLUSION_FILE_ID");
            this.Reference(x => x.RequestFile, "Файл запроса").Column("REQUEST_FILE_ID");
            this.Reference(x => x.ZonalInspection, "Зональная инспекция").Column("ZONAINSP_ID");
            this.Reference(x => x.PreviousRequest, "Предыдущая заявка").Column("PREVIOUS_REQUEST_ID").Fetch();
            this.Reference(x => x.KindStatement, "Вид обращения").Column("GJI_DICT_KIND_ID").Fetch();
            this.Reference(x => x.RedtapeFlag, "Признак волокиты").Column("GJI_REDTAPE_FLAG_ID").Fetch();
            this.Reference(x => x.Executant, "Исполнитель").Column("EXECUTANT_ID").Fetch();
            this.Reference(x => x.Surety, "Поручитель").Column("SURETY_ID").Fetch();
            this.Reference(x => x.SuretyResolve, "Резолюция").Column("SURETY_RESOLVE_ID").Fetch();
            this.Reference(x => x.ApprovalContragent, "Контрагент (в рсммотрении)").Column("APPROVALCONTRAGENT_ID");

            this.Property(x => x.QuestionStatus, "Статус вопроса").Column("QUESTION_STATE");
            this.Property(x => x.SSTUExportState, "Экспортированно в ССТУ").Column("SSTU");
            this.Property(x => x.CheckTime, "Контрольный срок").Column("CHECK_TIME");
            this.Property(x => x.ExtensTime, "Продленный контрольный срок").Column("EXTENS_TIME");
            this.Property(x => x.ControlDateGisGkh, "Срок ГИС ЖКХ").Column("CONTROL_DATE_GIS_GKH");
            this.Property(x => x.NumberGji, "Номер ГЖИ").Column("GJI_NUMBER");
            this.Property(x => x.RequestStatus, "Статус заявления").Column("REQUEST_STATUS");
            this.Property(x => x.AmountPages, "Количество листов в обращении").Column("AMOUNT_PAGES");
            this.Property(x => x.QuestionsCount, "Количество Вопросов").Column("QUESTION_COUNT");
            this.Property(x => x.PlannedExecDate, "Планируемая дата исполнения").Column("PLANNED_EXEC_DATE");
            this.Property(x => x.ExecutantTakeDate, "Дата приёма в работу исполнителем").Column("EXEC_TAKE_DATE");
            this.Property(x => x.Comment, "Комментарий").Column("COMMENT").Length(1000);
            this.Property(x => x.Year, "Год").Column("YEAR");
            this.Property(x => x.ChangeDate, "Дата внесения изменений в реестр лицензий").Column("CHANGE_DATE");
        }
    }
}
