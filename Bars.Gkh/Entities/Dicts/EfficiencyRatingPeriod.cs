namespace Bars.Gkh.Entities.Dicts
{
    using System;
    
    using Bars.Gkh.Entities;
    using Bars.Gkh.MetaValueConstructor.DomainModel;

    /// <summary>
    /// Периоды рейтинга эффективности
    /// </summary>
    public class EfficiencyRatingPeriod : BaseImportableEntity
    {
        /// <summary>
        /// Наименование периода
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime DateEnd { get; set; }

        /// <summary>
        /// Конструктор, к которому относится текущий период
        /// </summary>
        public virtual MetaConstructorGroup Group { get; set; }
    }
}