namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    ///  Данная вьюха предназначена для реестра Обращения граждан, чтобы вернуть Муниципальное образование и количество домов у обращения
    /// </summary>
    public class ViewAppealCitizens : PersistentObject
    {
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Строка идентификаторов жилых домов вида /1/2/4/ 
        /// </summary>
        public virtual string RealityObjectIds { get; set; }

        /// <summary>
        /// Муниципальное образование из первого дома
        /// </summary>
        public virtual string Municipality { get; set; }

        /// <summary>
        /// Муниципальное образование из первого дома
        /// </summary>
        public virtual long? MunicipalityId { get; set; }

        /// <summary>
        /// количество домов во вкладке "Место возникновения"
        /// </summary>
        public virtual int? CountRealtyObj { get; set; }

        /// <summary>
        /// Количество вопросов
        /// </summary>
        public virtual int? QuestionsCount { get; set; }

        /// <summary>
        /// Адреса домов
        /// </summary>
        public virtual string RealObjAddresses { get; set; }

        /// <summary>
        /// Зональная жилищная инспекция
        /// </summary>
        public virtual ZonalInspection ZonalInspection { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public virtual Inspector Executant { get; set; }

         /// <summary>
        /// Проверяющий
        /// </summary>
        public virtual Inspector Tester { get; set; }

         /// <summary>
         /// Поручитель
         /// </summary>
        public virtual Inspector Surety { get; set; }
 
         /// <summary>
        /// Резолюция
        /// </summary>
        public virtual ResolveGji SuretyResolve { get; set; }

        /// <summary>
        /// Срок исполнения (Исполнитель)
        /// </summary>
        public virtual DateTime? ExecuteDate { get; set; }

        /// <summary>
        /// Срок исполнения (Поручитель)
        /// </summary>
        public virtual DateTime? SuretyDate { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }


        /// <summary>
        /// Дата ответа
        /// </summary>
        public virtual DateTime? AnswerDate { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Номер ГЖИ
        /// </summary>
        public virtual string NumberGji { get; set; }

        /// <summary>
        /// Номер ГЖИ для сортировки
        /// </summary>
        public virtual long? SortableNumberGji { get; set; }

        /// <summary>
        /// Дата обращения
        /// </summary>
        public virtual DateTime? DateFrom { get; set; }

        /// <summary>
        /// Контрольный срок
        /// </summary>
        public virtual DateTime? CheckTime { get; set; }

        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Корреспондент
        /// </summary>
        public virtual string Correspondent { get; set; }

        /// <summary>
        /// Адрес корреспондента
        /// </summary>
        public virtual string CorrespondentAddress { get; set; }

        /// <summary>
        /// Муниципальное образование из первого дома
        /// </summary>
        public virtual string MoSettlement { get; set; }

        /// <summary>
        /// Населенный пункт из первого дома
        /// </summary>
        public virtual string PlaceName { get; set; }

        /// <summary>
        /// ФИО исполнителей (если поддерживается регионом)
        /// </summary>
        public virtual string ExecutantNames { get; set; }

        /// <summary>
        /// Тематики
        /// </summary>
        public virtual string Subjects { get; set; }

        /// <summary>
        /// Источники обращения
        /// </summary>
        public virtual string RevenueSourceNames { get; set; }

        /// <summary>
        /// Дата источника обращения
        /// </summary>
        public virtual string RevenueSourceDates { get; set; }

        /// <summary>
        /// Номер источника обращения
        /// </summary>
        public virtual string RevenueSourceNumbers { get; set; }

        /// <summary>
        /// Продленный контрольный срок
        /// </summary>
        public virtual DateTime? ExtensTime { get; set; }

        /// <summary>
        /// Наименование тематики 
        /// </summary>
        public virtual string SubSubjectsName { get; set; }

        /// <summary>
        /// Наименование характеристики
        /// </summary>
        public virtual string FeaturesName { get; set; }

        /// <summary>
        /// ФИО контролёров обращения
        /// </summary>
        public virtual string ControllersFio { get; set; }

        /// <summary>
        /// Экспортировано в ССТУ
        /// </summary>
        public virtual SSTUExportState SSTUExportState { get; set; }

        /// <summary>
        /// Статус вопроса
        /// </summary>
        public virtual QuestionStatus QuestionStatus { get; set; }

        /// <summary>
        /// Строка идентификаторов жилых домов вида /1/2/4/ 
        /// </summary>
        public virtual string SOPR { get; set; }
        
        /// Идентификатор файла
        /// </summary>
        public virtual long? FileId { get; set; }

        /// <summary>
        /// Вид обращения
        /// </summary>
        public virtual KindStatementGji KindStatement { get; set; }

        /// <summary>
        /// Предыдущее обращение
        /// </summary>
        public virtual AppealCits PreviousAppealCits { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
    }
}