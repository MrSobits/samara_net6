namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using System;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    public class SMEVValidPassport : BaseEntity
    {
        /// <summary>
        /// ID сообщения в системе СМЭВ
        /// </summary>
        public virtual string MessageId { get; set; }

        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// дата запроса
        /// </summary>
        public virtual DateTime CalcDate { get; set; }

        /// <summary>
        /// Серия паспорта
        /// </summary>
        public virtual string DocSerie { get; set; }

        /// <summary>
        /// Номер паспорта
        /// </summary>
        public virtual string DocNumber { get; set; }

        /// <summary>
        /// Дата выдачи паспорта
        /// </summary>
        public virtual DateTime? DocIssueDate { get; set; }

        /// <summary>
        /// Статус паспорта
        /// </summary>
        public virtual string DocStatus { get; set; }

        /// <summary>
        /// Причина недействительности
        /// </summary>
        public virtual string InvalidityReason { get; set; }

        /// <summary>
        /// Недействительно с
        /// </summary>
        public virtual DateTime? InvaliditySince { get; set; }

        /// <summary>
        /// Текущее состояние запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// TaskId
        /// </summary>
        public virtual string TaskId { get; set; }
    }
}
