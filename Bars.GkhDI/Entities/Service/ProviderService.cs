namespace Bars.GkhDi.Entities
{
    using System;
    using Gkh.Entities;

    /// <summary>
    /// Поставщики услуги
    /// </summary>
    public class ProviderService : BaseGkhEntity
    {
        /// <summary>
        /// Базовая услуга
        /// </summary>
        public virtual BaseService BaseService { get; set; }

        /// <summary>
        /// Поставщик услуги
        /// </summary>
        public virtual Contragent Provider { get; set; }

        /// <summary>
        /// Дата заключения договора
        /// </summary>
        public virtual DateTime? DateStartContract { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Активный
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        public virtual string NumberContract { get; set; }
    }
}
