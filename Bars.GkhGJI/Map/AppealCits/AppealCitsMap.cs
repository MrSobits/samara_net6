using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    
    
    /// <summary>Маппинг для "Обращение граждан"</summary>
    public class AppealCitsMap : BaseEntityMap<AppealCits>
    {
        
        public AppealCitsMap() : 
                base("Обращение граждан", "GJI_APPEAL_CITIZENS")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Status, "Статус из Монжф (для хранения)").Column("STATUS").NotNull();
            this.Property(x => x.SuretyDate, "Срок исполнения (Поручитель)").Column("SURETY_DATE");
            this.Property(x => x.ExecuteDate, "Дата исполнения").Column("EXECUTE_DATE");
            this.Property(x => x.Number, "Номер").Column("NUM").Length(500);
            this.Property(x => x.Year, "Год").Column("YEAR");
            this.Property(x => x.Correspondent, "Корреспондент").Column("CORRESPONDENT").Length(255);
            this.Property(x => x.CorrespondentAddress, "Адрес корреспондента").Column("CORRESPONDENT_ADDRESS").Length(2000);
            this.Property(x => x.Email, "Эл.почта").Column("EMAIL").Length(255);
            this.Property(x => x.Phone, "Контактный Телефон").Column("PHONE").Length(255);
            this.Property(x => x.QuestionsCount, "Количество Вопросов").Column("QUESTIONS_COUNT");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(10000);
            this.Property(x => x.FlatNum, "Номер квартиры").Column("FLAT_NUM").Length(255);
            this.Property(x => x.NumberGji, "Номер ГЖИ").Column("GJI_NUMBER").Length(255);
            this.Property(x => x.SortNumberGji, "Номер ГЖИ для сортировок").Column("GJI_NUM_SORT");
            this.Property(x => x.DateFrom, "От").Column("DATE_FROM");
            this.Property(x => x.CheckTime, "Контрольный срок").Column("CHECK_TIME");
            this.Property(x => x.DescriptionLocationProblem, "Описание (Место возникновения проблемы)").Column("DESC_LOCATION_PROBLEM").Length(255);
            this.Property(x => x.DocumentNumber, "Номер обращения").Column("DOCUMENT_NUMBER").Length(50);
            this.Property(x => x.PreviousAppealCitExternalId, "ПредыдущееОбращение (ExternalId)").Column("GJI_APPEAL_ID");
            this.Property(x => x.TypeCorrespondent, "Тип корреспондента").Column("TYPE_CORRESPONDENT").NotNull();
            this.Property(x => x.Accepting, "Отдел, принявший обращение").Column("ACCEPTING");
            this.Property(x => x.IntNumber, "Целая часть номера").Column("INT_NUMBER");
            this.Property(x => x.IntSubnumber, "Целая часть подномера").Column("INT_SUBNUMBER");
            this.Property(x => x.ArchiveNumber, "Архивный номер").Column("ARCHIVE_NUMBER");
            this.Property(x => x.SpecialControl, "Особый контроль обращений (Поле нужно для Томска)").Column("SPECIAL_CONTROL");
            this.Property(x => x.CaseNumber, "Номер дела (Место хранения документа)").Column("CASE_NUMBER").Length(150);
            this.Property(x => x.CaseDate, "Дата дела (Место хранения документа)").Column("CASE_DATE");
            this.Property(x => x.IsImported, "Импортировано из АСЭД ДЕЛО").Column("IS_IMPORTED");
            this.Property(x => x.CitizenId, "Уникальный номер гражданина").Column("CITIZEN_ID");
            this.Property(x => x.ExtensTime, "Продленный контрольный срок").Column("EXTENS_TIME");
            this.Property(x => x.Comment, "Комментарий").Column("COMMENT").Length(1000);
            this.Property(x => x.AmountPages, "Количество листов в обращении").Column("AMOUNT_PAGES");
            this.Property(x => x.DeclarantMailingAddress, "Почтовый адрес заявителя").Column("DECLARANT_MAILING_ADDRESS");
            this.Property(x => x.DeclarantWorkPlace, "Место работы заявителя").Column("DECLARANT_WORK_PLACE");
            this.Property(x => x.DeclarantSex, "Пол заявителя").Column("DECLARANT_SEX");
            this.Property(x => x.AppealStatus, "Статус заявления").Column("APPEAL_STATUS");
            this.Property(x => x.PlannedExecDate, "Планируемая дата исполнения").Column("PLANNED_EXEC_DATE");
            this.Property(x => x.AppealUid, "UID обращения").Column("APPEAL_UID");
            this.Property(x => x.ExecutantTakeDate, "Дата приёма в работу исполнителем").Column("EXEC_TAKE_DATE");
            this.Property(x => x.IsRequestWorkSend, "Отправлена информация о приеме в работу").Column("IS_REQUEST_WORK_SEND").NotNull();
            this.Property(x => x.IsAcceptedWorkSend, "Отправлена информация о принятии в работу").Column("IS_ACCEPTED_WORK_SEND").NotNull();
            this.Property(x => x.StatementSubjects, "Тематики обращения").Column("STATEMENT_SUBJECTS");
            this.Property(x => x.AnswerDate, "Дата ответа").Column("ANSWER_DATE");
            this.Property(x => x.SSTUExportState, "Экспортировано в ССТУ").Column("SSTU").NotNull();
            this.Property(x => x.QuestionStatus, "Статус вопроса ССТУ").Column("QUESTION_STATE").NotNull();
            this.Reference(x => x.OrderContragent, "Организация ответственная за обращение").Column("ORDER_CONTRAGENT_ID").Fetch();
            this.Reference(x => x.SSTUTransferOrg, "Организация трансфера обращений").Column("TRANSFER_ORG_ID").Fetch();
            this.Property(x => x.IsIdentityVerified, "Данные личности подтверждены").Column("IS_IDENTITY_VERIFIED").NotNull();
            
            this.Reference(x => x.Surety, "Поручитель").Column("SURETY_ID").Fetch();
            this.Reference(x => x.SuretyResolve, "Резолюция").Column("SURETY_RESOLVE_ID").Fetch();
            this.Reference(x => x.Executant, "Исполнитель").Column("EXECUTANT_ID").Fetch();
            this.Reference(x => x.Tester, "Проверяющий").Column("TESTER_ID").Fetch();
            this.Reference(x => x.ZonalInspection, "Зональная жилищная инспекция").Column("ZONAINSP_ID");
            this.Reference(x => x.ManagingOrganization, "Управляющая Организация").Column("MANAGING_ORG_ID");
            this.Reference(x => x.RedtapeFlag, "Признак Волокиты").Column("GJI_REDTAPE_FLAG_ID").Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            this.Property(x => x.IsPrelimentaryCheck, "Проведена предварительная проверка").Column("IS_PRELIMENTARY_CHECK").NotNull();
            this.Reference(x => x.PreviousAppealCits, "Предыдущее Обращение").Column("PREVIOUS_APPEAL_CITIZENS_ID").Fetch();
            this.Reference(x => x.KindStatement, "Вид обращения").Column("GJI_DICT_KIND_ID").Fetch();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Reference(x => x.SocialStatus, "Социальный статус").Column("SOCIAL_ST_ID").Fetch();
            this.Reference(x => x.ApprovalContragent, "Контрагент (а рсммотрении)").Column("APPROVALCONTRAGENT_ID");
            this.Reference(x => x.Citizenship, "Гражданство").Column("GJI_DICT_CITIZENSHIP_ID");
            this.Reference(x => x.AppealRegistrator, "Регистратор обращения").Column("REGISTRATOR_ID");
            //поля для оперативного реестра
            this.Property(x => x.RealityAddresses, "Адреса домов").Column("REALITY_ADDRESSES");
            this.Property(x => x.IncomingSources, "Источники поступления").Column("INCOMING_SOURCES");
            this.Property(x => x.IncomingSourcesName, "Источники поступления").Column("INCOMING_SOURCES_NAMES");
            this.Property(x => x.Executors, "Исполнители").Column("EXECUTORS");
            this.Property(x => x.Testers, "Проверяющие").Column("TESTERS");
            this.Property(x => x.Municipality, "Муниципальное образование из первого дома").Column("MUNICIPALITY");
            this.Property(x => x.MunicipalityId, "Муниципальное образование из первого дома").Column("MUNICIPALITY_ID");

            Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID").Length(36);
            Property(x => x.GisGkhTransportGuid, "ГИС ЖКХ Transport GUID").Column("GIS_GKH_TRANSPORT_GUID").Length(36);
            Property(x => x.GisGkhParentGuid, "ГИС ЖКХ GUID родительского обращения").Column("GIS_GKH_PARENT_GUID").Length(36);
            Property(x => x.GisGkhContragentGuid, "ГИС ЖКХ GUID контрагента, от которого обращение").Column("GIS_GKH_CONTRAGENT_GUID").Length(36);
            Property(x => x.GisWork, "Признак работы с обращением в ГИС ЖКХ").Column("GIS_WORK");
            Property(x => x.IdentityConfirmed, "Личность заявителя подтверждена").Column("IDENTITY_CONFIRMED");
            Property(x => x.CorrespondentAge, "Возраст").Column("CORR_AGE");
            this.Reference(x => x.ContragentCorrespondent, "Контрагент как корреспондент в обращении").Column("CORRESPONDENT_CONTRAGENT_ID").Fetch();
            this.Property(x => x.FastTrack, "Системный номер").Column("FAST_TRACK");
            this.Property(x => x.ControlDateGisGkh, "Срок ГИС ЖКХ").Column("CONTROL_DATE_GIS_GKH");
        }
    }
}
