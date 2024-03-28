namespace Bars.GkhDi.Entities
{
    using System;
    using Enums;

    /// <summary>
    /// Услуга ремонт
    /// </summary>
    public class RepairService : BaseService
    {
        /// <summary>
        /// Тип оказания услуги
        /// </summary>
        public virtual TypeOfProvisionServiceDi TypeOfProvisionService { get; set; }

        /// <summary>
        /// Сумма работ по ТО
        /// </summary>
        public virtual decimal? SumWorkTo { get; set; }

        /// <summary>
        /// Фактическая сумма
        /// </summary>
        public virtual decimal? SumFact { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Сведения о выполнении 
        /// </summary>
        public virtual string ProgressInfo { get; set; }

        /// <summary>
        /// Причина отклонения
        /// </summary>
        public virtual string RejectCause { get; set; }
    }
}
