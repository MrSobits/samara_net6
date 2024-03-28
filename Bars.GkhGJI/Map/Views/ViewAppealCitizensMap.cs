namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    public class ViewAppealCitizensMap : ViewAppealCitizensMap<ViewAppealCitizens>
    {
    }

    /// <summary>Маппинг для "Данная вьюха предназначена для реестра Обращения граждан, чтобы вернуть Муниципальное образование и количество домов у обращения"</summary>
    public abstract class ViewAppealCitizensMap<T> : PersistentObjectMap<T>
        where T : ViewAppealCitizens
    {
        protected ViewAppealCitizensMap() :
                base("Данная вьюха предназначена для реестра Обращения граждан, чтобы вернуть Муниципальное " +
                    "образование и количество домов у обращения",
                    "VIEW_GJI_APPEAL_CITS")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.RealityObjectIds, "Строка идентификаторов жилых домов вида /1/2/4/").Column("RO_IDS");
            this.Property(x => x.SuretyDate, "Срок исполнения (Поручитель)").Column("SURETY_DATE");
            this.Property(x => x.ExecuteDate, "Срок исполнения (Исполнитель)").Column("EXECUTE_DATE");
            this.Property(x => x.CountRealtyObj, "количество домов во вкладке \"Место возникновения\"").Column("COUNT_RO");
            this.Property(x => x.Municipality, "Муниципальное образование из первого дома").Column("MUNICIPALITY");
            this.Property(x => x.MunicipalityId, "Муниципальное образование из первого дома").Column("MUNICIPALITY_ID");
            this.Property(x => x.Number, "Номер").Column("DOCUMENT_NUMBER");
            this.Property(x => x.NumberGji, "Номер ГЖИ").Column("GJI_NUMBER");
            this.Property(x => x.SortableNumberGji, "Номер ГЖИ для сортировки").Column("GJI_NUM_SORT");
            this.Property(x => x.DateFrom, "Дата обращения").Column("DATE_FROM");
            this.Property(x => x.CheckTime, "Контрольный срок").Column("CHECK_TIME");
            this.Property(x => x.QuestionsCount, "Количество вопросов").Column("QUESTIONS_COUNT");
            this.Property(x => x.ContragentName, "Управляющая организация").Column("CONTRAGENT_NAME");
            this.Property(x => x.Correspondent, "Корреспондент").Column("CORRESPONDENT");
            this.Property(x => x.CorrespondentAddress, "Адрес корреспондента").Column("CORRESPONDENT_ADDRESS");
            this.Property(x => x.RealObjAddresses, "Адреса домов").Column("RO_ADR");
            this.Property(x => x.MoSettlement, "Муниципальное образование из первого дома").Column("MO_SETTLEMENT");
            this.Property(x => x.PlaceName, "Населенный пункт из первого дома").Column("PLACE_NAME");
            this.Property(x => x.RevenueSourceNames, "Источники обращения").Column("SOURCE_NAMES");
            this.Property(x => x.RevenueSourceDates, "Дата обращения").Column("SOURCE_DATES");
            this.Property(x => x.RevenueSourceNumbers, "Номер обращения").Column("SOURCE_NUMBERS");
            this.Property(x => x.ExtensTime, "Продленный контрольный срок").Column("EXTENS_TIME");
            this.Property(x => x.Subjects, "Тематики").Column("SUBJECTS");
            this.Property(x => x.ExecutantNames, "ФИО исполнителей").Column("EXECUTANT_NAMES");
            this.Property(x => x.AnswerDate, "Дата ответа").Column("ANSWER_DATE");
            this.Reference(x => x.SuretyResolve, "Резолюция").Column("SURETY_RESOLVE_ID").Fetch();
            this.Reference(x => x.Executant, "Исполнитель").Column("EXECUTANT_ID").Fetch();
            this.Reference(x => x.Tester, "Проверяющий").Column("TESTER_ID").Fetch();
            this.Reference(x => x.ZonalInspection, "Зональная жилищная инспекция").Column("ZONAINSP_ID").Fetch();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Reference(x => x.AppealCits, "AppealCits").Column("ID").Fetch();
            this.Property(x => x.SubSubjectsName, "Номер обращения").Column("SubSubjects_name");
            this.Property(x => x.FeaturesName, "Номер обращения").Column("Features_name");
            this.Property(x => x.ControllersFio, "Фио контролёров обращения").Column("controller_fio");
            this.Property(x => x.SSTUExportState, "Статус выгрузки в ССТУ").Column("SSTUExportState");
            this.Property(x => x.QuestionStatus, "Статус обращения в ССТУ").Column("QuestionStatus");
            this.Property(x => x.SOPR, "Обращения СОПР").Column("SOPR");
            this.Reference(x => x.KindStatement, "Вид обращения").Column("GJI_DICT_KIND_ID").Fetch();
            this.Reference(x => x.PreviousAppealCits, "Предыдущее обращение").Column("PREVIOUS_APPEAL_CITIZENS_ID").Fetch();
            this.Reference(x => x.Surety, "Поручитель").Column("SURETY_ID").Fetch();
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION");
            this.Property(x => x.FileId, "Идентификатор файла").Column("FILE_ID");
        }
    }
}
