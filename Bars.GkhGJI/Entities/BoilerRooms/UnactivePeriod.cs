namespace Bars.GkhGji.Entities.BoilerRooms
{
    using System;
    using Bars.B4.DataAccess;

    /// <summary>
    /// Период неактивности котельной <see cref="BoilerRoom"/>
    /// </summary>
    public class UnactivePeriod : BaseEntity
    {
        /// <summary>
        /// Котельная
        /// </summary>
        public virtual BoilerRoom BoilerRoom { get; set; }

        /// <summary>
        /// Начало периода
        /// </summary>
        public virtual DateTime? Start { get; set; }

        /// <summary>
        /// Конец периода
        /// </summary>
        public virtual DateTime? End { get; set; }
    }
}