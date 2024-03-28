using Bars.Gkh.Enums;
using Bars.GkhGji.Entities.Dict;

namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Voronezh.Enums;

    /// <summary>
    /// Обращение граждан
    /// </summary>
    public partial class MKDLicRequest : BaseEntity, IStatefulEntity
    {

        /// <summary>
        /// Статус заявления
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Тип заявителя
        /// </summary>
        public virtual ExecutantDocGji ExecutantDocGji { get; set; }

        /// <summary>
        /// Заявитель-контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Контрагент в заявлении
        /// </summary>
        public virtual Contragent StatmentContragent { get; set; }

        /// <summary>
        /// Заявитель - физлицо
        /// </summary>
        public virtual string PhysicalPerson { get; set; }

        /// <summary>
        /// Дата заявления
        /// </summary>
        public virtual DateTime StatementDate { get; set; }

        /// <summary>
        /// Номер заявления
        /// </summary>
        public virtual string StatementNumber { get; set; }

        /// <summary>
        /// Тип запроса
        /// </summary>
        public virtual MKDLicTypeRequest MKDLicTypeRequest { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Результат рассмотрения
        /// </summary>
        public virtual LicStatementResult LicStatementResult { get; set; }

        /// <summary>
        /// Комментарий к результату рассмотрения
        /// </summary>
        public virtual string LicStatementResultComment { get; set; }

        /// <summary>
        /// Номер заключения
        /// </summary>
        public virtual string ConclusionNumber { get; set; }

        /// <summary>
        /// Дата заключения
        /// </summary>
        public virtual DateTime? ConclusionDate { get; set; }

        /// <summary>
        /// Обжалование
        /// </summary>
        public virtual bool Objection { get; set; }

        /// <summary>
        /// Обжаловано
        /// </summary>
        public virtual DisputeResult ObjectionResult { get; set; }

        /// <summary>
        /// Документ 
        /// </summary>
        public virtual FileInfo RequestFile { get; set; }

        /// <summary>
        /// Документ решения
        /// </summary>
        public virtual FileInfo ConclusionFile { get; set; }

        /// <summary>
        /// Статус вопроса
        /// </summary>
        public virtual QuestionStatus QuestionStatus { get; set; }

        /// <summary>
        /// Экспортировано в ССТУ
        /// </summary>
        public virtual SSTUExportState SSTUExportState { get; set; }

        /// <summary>
        /// Контрольный срок
        /// </summary>
        public virtual DateTime? CheckTime { get; set; }

        /// <summary>
        /// Продленный контрольный срок
        /// </summary>
        public virtual DateTime? ExtensTime { get; set; }

        /// <summary>
        /// Срок ГИС ЖКХ
        /// </summary>
        public virtual DateTime? ControlDateGisGkh { get; set; }

        /// <summary>
        /// Номер ГЖИ
        /// </summary>
        public virtual string NumberGji { get; set; }

        /// <summary>
        /// Зональная жилищная инспекция
        /// </summary>
        public virtual ZonalInspection ZonalInspection { get; set; }

        /// <summary>
        /// Статус заявления
        /// </summary>
        public virtual AppealStatus? RequestStatus { get; set; }

        /// <summary>
        /// Количество листов в обращении
        /// </summary>
        public virtual long? AmountPages { get; set; }

        /// <summary>
        /// Предыдущее Обращение 
        /// </summary>
        public virtual MKDLicRequest PreviousRequest { get; set; }

        /// <summary>
        /// Вид обращения
        /// </summary>
        public virtual KindStatementGji KindStatement { get; set; }

        /// <summary>
        /// Количество Вопросов
        /// </summary>
        public virtual int QuestionsCount { get; set; }

        /// <summary>
        /// Признак Волокиты
        /// </summary>
        public virtual RedtapeFlagGji RedtapeFlag { get; set; }

        /// <summary>
        /// Планируемая дата исполнения
        /// </summary>
        public virtual DateTime? PlannedExecDate { get; set; }

        /// <summary>
        /// Дата приёма в работу исполнителем
        /// </summary>
        public virtual DateTime? ExecutantTakeDate { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public virtual Inspector Executant { get; set; }

        /// <summary>
        /// Поручитель
        /// </summary>
        public virtual Inspector Surety { get; set; }

        /// <summary>
        /// Резолюция
        /// </summary>
        public virtual ResolveGji SuretyResolve { get; set; }

        /// <summary>
        /// Контрагент (в рсммотрении)
        /// </summary>
        public virtual Contragent ApprovalContragent { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Comment { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Дата внесения изменений в реестр лицензий
        /// </summary>
        public virtual DateTime? ChangeDate { get; set; }
    }
}