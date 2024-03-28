namespace Bars.GkhDi.Entities
{
    using System;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Работы по плану работ по содержанию и ремонту
    /// </summary>
    public class PlanWorkServiceRepairWorks : BaseGkhEntity
    {
        /// <summary>
        /// План работ по содержанию и ремонту
        /// </summary>
        public virtual PlanWorkServiceRepair PlanWorkServiceRepair { get; set; }

        /// <summary>
        /// Работа ремонта ППР
        /// </summary>
        public virtual WorkRepairList WorkRepairList { get; set; }

        /// <summary>
        /// Сведение о выполнение
        /// </summary>
        public virtual string DataComplete { get; set; }

        /// <summary>
        /// Периодичность
        /// </summary>
        public virtual PeriodicityTemplateService PeriodicityTemplateService { get; set; }

        /// <summary>
        /// Срок выполнения
        /// </summary>
        public virtual DateTime? DateComplete { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        public virtual decimal? Cost { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Фактическая стоимость
        /// </summary>
        public virtual decimal? FactCost { get; set; }

        /// <summary>
        /// Причина отклонения
        /// </summary>
        public virtual string ReasonRejection { get; set; }
    }
}
