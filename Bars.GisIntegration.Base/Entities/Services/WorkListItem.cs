namespace Bars.GisIntegration.Base.Entities.Services
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Работа/услуга перечня
    /// </summary>
    public class WorkListItem : BaseRisEntity
    {
        /// <summary>
        /// Перечень работ/услуг
        /// </summary>
        public virtual WorkList WorkList { get; set; }

        /// <summary>
        /// Общая стоимость
        /// </summary>
        public virtual decimal TotalCost { get; set; }

        /// <summary>
        /// Код работы/услуги организации (НСИ 59)
        /// </summary>
        public virtual string WorkItemCode { get; set; }

        /// <summary>
        /// Гуид работы/услуги организации (НСИ 59)
        /// </summary>
        public virtual string WorkItemGuid { get; set; }

        /// <summary>
        /// Номер строки в перечне работ и услуг
        /// </summary>
        public virtual int Index { get; set; }
    }
}