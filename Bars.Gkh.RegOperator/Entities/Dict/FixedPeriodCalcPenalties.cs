namespace Bars.Gkh.RegOperator.Entities.Dict
{
    using System;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Настройка фиксированного периода расчета пени
    /// </summary>
    public class FixedPeriodCalcPenalties : BaseImportableEntity
    {
        /// <summary>
        /// День начала периода
        /// </summary>
        public virtual int StartDay { get; set; }

        /// <summary>
        /// День окончания периода
        /// </summary>
        public virtual int EndDay { get; set; }

        /// <summary>
        /// Действует с
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Действует по
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }
    }
}