namespace Bars.Gkh.Reforma.Entities.Dict
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Reforma.Enums;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Периоды раскрытия информации в Реформе
    /// </summary>
    public class ReportingPeriodDict : BaseEntity
    {
        /// <summary>
        /// Период раскрытия в БАРС.ЖКХ
        /// </summary>
        public virtual PeriodDi PeriodDi { get; set; }

        /// <summary>
        /// Название периода
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Начало периода
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Конец периода
        /// </summary>
        public virtual DateTime DateEnd { get; set; }

        /// <summary>
        /// Состояние периода
        /// </summary>
        public virtual ReportingPeriodStateEnum State { get; set; }

        /// <summary>
        /// Идентификатор периода в Реформе
        /// </summary>
        public virtual int ExternalId { get; set; }

        /// <summary>
        /// Признак синхронизируемости. Синхронизация происходит только по
        /// синхронизируемым периодам
        /// </summary>
        public virtual bool Synchronizing { get; set; }

        /// <summary>
        /// Признак, отчетный период относится к старым формам (Постановление №731) (is_988 is false) 
        /// или к новым формам (Постановление №988) (is_988 is true)
        /// </summary>
        public virtual bool Is_988 { get; set; }
    }
}