namespace Bars.Gkh.Decisions.Nso.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Решение о сроке уплаты взносов
    /// </summary>
    public class PenaltyDelayDecision : UltimateDecision
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PenaltyDelayDecision()
        {
            Decision = new List<OwnerPenaltyDelay>();
        }

        /// <summary>
        /// Приянтое решение
        /// </summary>
        public virtual List<OwnerPenaltyDelay> Decision { get; set; }
    }

    /// <summary>
    /// Параметр учета пени, принятый в решении собственников 
    /// </summary>
    public class OwnerPenaltyDelay
    {
        /// <summary>
        /// Дата с
        /// </summary>
        public DateTime From { get; set; }

        /// <summary>
        /// Дата по
        /// </summary>
        public DateTime? To { get; set; }

        /// <summary>
        /// Количество дней просрочки
        /// </summary>
        public int DaysDelay { get; set; }

        /// <summary>
        /// допустима просрочка в месяц (т.е. оплатить можно в течение всего месяца)
        /// </summary>
        public bool MonthDelay { get; set; }
    }
}