using Bars.Gkh.Enums;
using Bars.GkhGji.Entities.Dict;

namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Обращение граждан
    /// </summary>
    public partial class AppealCits : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Уникальный номер гражданина
        /// </summary>
        public virtual int? CitizenId { get; set; }

        /// <summary>
        /// Поручитель
        /// </summary>
        public virtual Inspector Surety { get; set; }

        /// <summary>
        /// Резолюция
        /// </summary>
        public virtual ResolveGji SuretyResolve { get; set; }

        /// <summary>
        /// Срок исполнения (Поручитель)
        /// </summary>
        public virtual DateTime? SuretyDate { get; set; }

        /// <summary>
        /// Дата приёма в работу исполнителем
        /// </summary>
        public virtual DateTime? ExecutantTakeDate { get; set; }

        /// <summary>
        /// Отправлена информация о приеме в работу
        /// </summary>
        public virtual bool IsRequestWorkSend { get; set; }

        /// <summary>
        /// Отправлена информация о принятии в работу
        /// </summary>
        public virtual bool IsAcceptedWorkSend { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public virtual Inspector Executant { get; set; }

        /// <summary>
        /// Проверяющий
        /// </summary>
        public virtual Inspector Tester { get; set; }

        /// <summary>
        /// Дата исполнения
        /// </summary>
        public virtual DateTime? ExecuteDate { get; set; }

        /// <summary>
        /// Зональная жилищная инспекция
        /// </summary>
        public virtual ZonalInspection ZonalInspection { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Подномер
        /// </summary>
        //public virtual string SubNumber { get; set; }

        /// <summary>
        /// Номер обращения
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Номер ГЖИ
        /// </summary>
        public virtual string NumberGji { get; set; }

        /// <summary>
        /// Номер ГЖИ для сортировок
        /// </summary>
        public virtual long? SortNumberGji { get; set; }

        /// <summary>
        /// От
        /// </summary>
        public virtual DateTime? DateFrom { get; set; }

        /// <summary>
        /// Контрольный срок
        /// </summary>
        public virtual DateTime? CheckTime { get; set; }

        /// <summary>
        /// Предыдущее Обращение 
        /// </summary>
        public virtual AppealCits PreviousAppealCits { get; set; }

        /// <summary>
        /// Вид обращения
        /// </summary>
        public virtual KindStatementGji KindStatement { get; set; }

        /// <summary>
        /// Описание (Место возникновения проблемы)
        /// </summary>
        public virtual string DescriptionLocationProblem { get; set; }

        /// <summary>
        /// Номер квартиры
        /// </summary>
        public virtual string FlatNum { get; set; }

        /// <summary>
        /// Тематики обращения
        /// </summary>
        public virtual string StatementSubjects { get; set; }

        /// <summary>
        /// Управляющая Организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Статус из Монжф (для хранения)
        /// </summary>
        [Obsolete("Use State")]
        public virtual StatusAppealCitizens Status { get; set; }

        /// <summary>
        /// Корреспондент
        /// </summary>
        public virtual string Correspondent { get; set; }

        /// <summary>
        /// Адрес корреспондента
        /// </summary>
        public virtual string CorrespondentAddress { get; set; }

        /// <summary>
        /// Эл.почта
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Контактный Телефон
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// Количество Вопросов
        /// </summary>
        public virtual int QuestionsCount { get; set; }

        /// <summary>
        /// Признак Волокиты
        /// </summary>
        public virtual RedtapeFlagGji RedtapeFlag { get; set; }

        /// <summary>
        /// ПредыдущееОбращение (ExternalId)
        /// </summary>
        public virtual long? PreviousAppealCitExternalId { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Проведена предварительная проверка
        /// </summary>
        public virtual bool IsPrelimentaryCheck { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Тип корреспондента
        /// </summary>
        public virtual TypeCorrespondent TypeCorrespondent { get; set; }

        /// <summary>
        /// Отдел, принявший обращение
        /// </summary>
        public virtual Accepting Accepting { get; set; }

        /// <summary>
        /// Целая часть номера
        /// </summary>
        public virtual int IntNumber { get; set; }

        /// <summary>
        /// Целая часть подномера
        /// </summary>
        public virtual int IntSubnumber { get; set; }

        /// <summary>
        /// Архивный номер
        /// </summary>
        public virtual string ArchiveNumber { get; set; }

        /// <summary>
        /// Номер дела (Место хранения документа)
        /// </summary>
        public virtual string CaseNumber { get; set; }

        /// <summary>
        /// Дата дела (Место хранения документа)
        /// </summary>
        public virtual DateTime? CaseDate { get; set; }

        /// <summary>
        /// Дата ответа
        /// </summary>
        public virtual DateTime? AnswerDate { get; set; }

        /// <summary>
        /// Социальный статус
        /// </summary>
        public virtual SocialStatus SocialStatus { get; set; }

        /// <summary>
        /// Контрагент (а рсммотрении)
        /// </summary>
        public virtual Contragent ApprovalContragent { get; set; }

        /// <summary>
        /// Особый контроль обращений (Поле нужно для Томска)
        /// </summary>
        public virtual bool SpecialControl { get; set; }

        /// <summary>
        /// Импортировано из АСЭД ДЕЛО
        /// </summary>
        public virtual bool IsImported { get; set; }

        /// <summary>
        /// Продленный контрольный срок
        /// </summary>
        public virtual DateTime? ExtensTime { get; set; }

        public virtual string Comment { get; set; }

        /// <summary>
        /// Количество листов в обращении
        /// </summary>
        public virtual long? AmountPages { get; set; }

        /// <summary>
        /// Гражданство
        /// </summary>
        public virtual Citizenship Citizenship { get; set; }

        /// <summary>
        /// Почтовый адрес заявителя
        /// </summary>
        public virtual string DeclarantMailingAddress { get; set; }

        /// <summary>
        /// Место работы заявителя
        /// </summary>
        public virtual string DeclarantWorkPlace { get; set; }

        /// <summary>
        /// Пол заявителя: 1 – мужской; 2 – женский
        /// </summary>
        public virtual Gender? DeclarantSex { get; set; }

        /// <summary>
        /// Статус заявления
        /// </summary>
        public virtual AppealStatus? AppealStatus { get; set; }

        /// <summary>
        /// Планируемая дата исполнения
        /// </summary>
        public virtual DateTime? PlannedExecDate { get; set; }

        /// <summary>
        /// Регистратор обращения
        /// </summary>
        public virtual Inspector AppealRegistrator { get; set; }

        /// <summary>
        /// UID обращения из ЕАИС
        /// </summary>
        public virtual Guid? AppealUid { get; set; }

        /// <summary>
        /// Контрагент как корреспондент в обращении
        /// </summary>
        public virtual Contragent ContragentCorrespondent { get; set; }

        /// <summary>
        /// Экспортировано в ССТУ
        /// </summary>
        public virtual SSTUExportState SSTUExportState { get; set; }

        /// <summary>
        /// Статус вопроса
        /// </summary>
        public virtual QuestionStatus QuestionStatus { get; set; }

        public virtual SSTUTransferOrg SSTUTransferOrg { get; set; }

        public virtual Contragent OrderContragent { get; set; }

        /// <summary>
        /// Адреса домов
        /// </summary>
        public virtual string RealityAddresses { get; set; }

        /// <summary>
        /// Источники поступления
        /// </summary>
        public virtual string IncomingSources { get; set; }

        /// <summary>
        /// Исполнители
        /// </summary>
        public virtual string Executors { get; set; }

        /// <summary>
        /// Проверяющие
        /// </summary>
        public virtual string Testers { get; set; }

        /// <summary>
        /// Муниципальное образование из первого дома
        /// </summary>
        public virtual string Municipality { get; set; }

        /// <summary>
        /// Муниципальное образование из первого дома
        /// </summary>
        public virtual long? MunicipalityId { get; set; }

        /// <summary>
        /// наименования источников
        /// </summary>
        public virtual string IncomingSourcesName { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID
        /// </summary>
        public virtual string GisGkhGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ Transport GUID
        /// </summary>
        public virtual string GisGkhTransportGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID родительского обращения
        /// </summary>
        public virtual string GisGkhParentGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID контрагента, от которого обращение
        /// </summary>
        public virtual string GisGkhContragentGuid { get; set; }

        /// <summary>
        /// Признак работы с обращением в ГИС ЖКХ
        /// </summary>
        public virtual bool GisWork { get; set; }

        /// <summary>
        /// Возраст заявимтеля
        /// </summary>
        public virtual int? CorrespondentAge { get; set; }

        /// <summary>
        /// Личность подтверждена
        /// </summary>
        public virtual bool IdentityConfirmed { get; set; }

        /// <summary>
        /// Срок ГИС ЖКХ
        /// </summary>
        public virtual DateTime? ControlDateGisGkh { get; set; }
        
        /// <summary>
        /// FastTrach
        /// </summary>
        public virtual bool FastTrack { get; set; }
        
        /// <summary>
        /// Данные личности подтверждены
        /// </summary>
        public virtual bool IsIdentityVerified { get; set; }
    }
}