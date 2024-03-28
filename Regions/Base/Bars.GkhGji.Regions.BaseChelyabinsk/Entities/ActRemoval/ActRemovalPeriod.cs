namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Дата и время проведения проверки предписания
    /// </summary>
    public class ActRemovalPeriod : BaseEntity
    {
        /// <summary>
        /// Акт проверки предписания
        /// </summary>
        public virtual ActRemoval ActRemoval { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime? DateCheck { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания 
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Время начала
        /// </summary>
        public virtual string TimeStart { get; set; }

        /// <summary>
        /// Время окончания 
        /// </summary>
        public virtual string TimeEnd { get; set; }
    }
}