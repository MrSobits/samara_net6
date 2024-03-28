namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using B4.Modules.FileStorage;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Chelyabinsk.Enums;
    using System;

    /// <summary>
    /// Судебная практика
    /// </summary>
    public class CourtPracticeDisputeHistory : BaseEntity
    {
        /// <summary>
        /// CourtPractice
        /// </summary>
        public virtual CourtPractice CourtPractice { get; set; }


        /// <summary>
        /// Суд
        /// </summary>
        public virtual JurInstitution JurInstitution { get; set; }     

        /// <summary>
        /// Дата обращения
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        /// Статус рассмотрения
        /// </summary>
        public virtual CourtPracticeState CourtPracticeState { get; set; }
        

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
    }
}