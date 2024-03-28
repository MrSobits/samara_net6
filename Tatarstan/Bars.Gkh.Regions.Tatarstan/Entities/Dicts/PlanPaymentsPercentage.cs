namespace Bars.Gkh.Regions.Tatarstan.Entities.Dicts
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>
    /// Процент планируемых оплат
    /// </summary>
    public class PlanPaymentsPercentage : BaseEntity
    {
        /// <summary>
        /// Поставщик ресурсов
        /// </summary>
        public virtual PublicServiceOrg PublicServiceOrg { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public virtual ServiceDictionary Service { get; set; }

        /// <summary>
        /// Ресурс
        /// </summary>
        public virtual CommunalResource Resource { get; set; }

        /// <summary>
        /// Процент оплаты
        /// </summary>
        public virtual decimal Percentage { get; set; }

        /// <summary>
        /// Дата начала действия
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public virtual DateTime DateEnd { get; set; }

        // TODO: добавить интерцептор для проверки пересечения дат + примечание: в ТЗ написано, что множественный выбор РСО, это не так, только 1 РСО
    }
}