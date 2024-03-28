namespace Bars.GkhCr.Entities
{
    using Bars.B4.Modules.States;
    using Gkh.Entities;
    using System;

    /// <summary>
    /// Стройконтроль выполнения работы
    /// </summary>
    public class BuildControlTypeWorkSmr : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual TypeWorkCr TypeWorkCr { get; set; }

        /// <summary>
        /// Объем выполнения
        /// </summary>
        public virtual DateTime MonitoringDate { get; set; }

        /// <summary>
        /// Объем выполнения
        /// </summary>
        public virtual decimal VolumeOfCompletion { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Стройконтроль
        /// </summary>
        public virtual Contragent Controller { get; set; }

        /// <summary>
        /// Срыв срока
        /// </summary>
        public virtual bool DeadlineMissed { get; set; }

        /// <summary>
        /// Процент выполнения
        /// </summary>
        public virtual decimal PercentOfCompletion { get; set; }

        /// <summary>
        /// Сумма расходов
        /// </summary>
        public virtual decimal CostSum { get; set; }

        /// <summary>
        /// ПРимечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// ПРимечание
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// ПРимечание
        /// </summary>
        public virtual TypeWorkCrAddWork TypeWorkCrAddWork { get; set; }

        /// <summary>
        /// Объем выполнения
        /// </summary>
        public virtual decimal? Latitude { get; set; }

        /// <summary>
        /// Объем выполнения
        /// </summary>
        public virtual decimal? Longitude { get; set; }
    }
}
