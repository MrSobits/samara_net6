namespace Bars.GkhGji.Regions.Habarovsk.Entities
{
    using B4.Modules.FileStorage;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Habarovsk.Enums;
    using System;

    /// <summary>
    /// Судебная практика
    /// </summary>
    public class CourtPractice : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Суд
        /// </summary>
        public virtual JurInstitution JurInstitution { get; set; }

        /// <summary>
        /// Номер дела
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Истец юр лицо
        /// </summary>
        public virtual Contragent ContragentPlaintiff { get; set; }

        /// <summary>
        /// ответчик юр лицо
        /// </summary>
        public virtual Contragent ContragentDefendant { get; set; }

        /// <summary>
        /// Истец ФИО
        /// </summary>
        public virtual string PlaintiffFio { get; set; }
        /// <summary>
        /// Истец адрес
        /// </summary>
        public virtual string PlaintiffAddress { get; set; }

        /// <summary>
        /// Ответчик ФИО
        /// </summary>
        public virtual string DefendantFio { get; set; }

        /// <summary>
        /// Ответчик адрес
        /// </summary>
        public virtual string DefendantAddress { get; set; }

        /// <summary>
        /// Третье лицо ФИО
        /// </summary>
        public virtual string DifferentFIo { get; set; }

        /// <summary>
        /// Третье лицо адрес
        /// </summary>
        public virtual string DifferentAddress { get; set; }

        /// <summary>
        ///Третье юр лицо
        /// </summary>
        public virtual Contragent DifferentContragent { get; set; }

        /// <summary>
        /// Дата обращения
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        /// Статус рассмотрения
        /// </summary>
        public virtual CourtPracticeState CourtPracticeState { get; set; }

        /// <summary>
        /// Вид спора
        /// </summary>
        public virtual DisputeType DisputeType { get; set; }

        /// <summary>
        /// Категория спора 
        /// </summary>
        public virtual DisputeCategory DisputeCategory { get; set; }

        /// <summary>
        /// Предмет спора 
        /// </summary>
        public virtual TypeFactViolation TypeFactViolation { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Discription { get; set; }

        /// <summary>
        /// Дата и время судебного заседания
        /// </summary>
        public virtual DateTime DateCourtMeeting { get; set; }

        /// <summary>
        /// Время судебного заседания
        /// </summary>
        public virtual DateTime? CourtMeetingTime { get; set; }

        /// <summary>
        /// Обеспечительные меры
        /// </summary>
        public virtual bool InterimMeasures { get; set; }


        /// <summary>
        /// Дата обеспечительных мер
        /// </summary>
        public virtual DateTime? InterimMeasuresDate { get; set; }

        /// <summary>
        /// Результат рассмотрения
        /// </summary>
        public virtual CourtMeetingResult CourtMeetingResult { get; set; }

        /// <summary>
        /// Вступило в законную силу
        /// </summary>
        public virtual bool InLaw { get; set; }

        /// <summary>
        /// Дата вступления в законную силу
        /// </summary>
        public virtual DateTime? InLawDate { get; set; }

        /// <summary>
        /// Судебные расходы
        /// </summary>
        public virtual bool CourtCosts { get; set; }

        /// <summary>
        /// Судебные расходы заявлено
        /// </summary>
        public virtual decimal CourtCostsPlan { get; set; }

        /// <summary>
        /// Судебные расходы взыскано
        /// </summary>
        public virtual decimal CourtCostsFact { get; set; }

        /// <summary>
        /// Исполнительный лист
        /// </summary>
        public virtual string PerformanceList { get; set; }

        /// <summary>
        /// Исполнительное производство
        /// </summary>
        public virtual string PerformanceProceeding { get; set; }

        //
        /// <summary>
        /// Исполнительное производство
        /// </summary>
        public virtual InstanceGji InstanceGji { get; set; }

        /// <summary>
        /// Обжалование
        /// </summary>
        public virtual bool Dispute { get; set; }

        /// <summary>
        /// Комментарий к статусу рассмотрения Приостановлено
        /// </summary>
        public virtual string PausedComment { get; set; }

        /// <summary>
		/// Не хранимое Часы времени составления
		/// </summary>
		public virtual int? FormatHour { get; set; }

        /// <summary>
        /// Не хранимое Минуты времени составления
        /// </summary>
        public virtual int? FormatMinute { get; set; }

        /// <summary>
        /// DocumentGji
        /// </summary>
        public virtual DocumentGji DocumentGji { get; set; }

        /// <summary>
        /// DocumentGji
        /// </summary>
        public virtual MKDLicRequest MKDLicRequest { get; set; }

    }
}