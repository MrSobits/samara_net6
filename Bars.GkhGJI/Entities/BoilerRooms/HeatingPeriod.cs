namespace Bars.GkhGji.Entities.BoilerRooms
{
    using System;
    using Bars.B4.DataAccess;

    /// <summary>
    /// Период подачи тепла в котельной <see cref="HeatingPeriod"/>
    /// </summary>
    public class HeatingPeriod : BaseEntity
    {
        /// <summary>
        /// Котельная
        /// </summary>
        public virtual BoilerRoom BoilerRoom { get; set; }

        /// <summary>
        /// Начала периода
        /// </summary>
        public virtual DateTime Start { get; set; }

        /// <summary>
        /// Конец периода
        /// </summary>
        public virtual DateTime? End { get; set; }
    }
}
