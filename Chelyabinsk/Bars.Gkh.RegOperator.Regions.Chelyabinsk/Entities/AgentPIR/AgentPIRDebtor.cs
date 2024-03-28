using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.Gkh.Enums;
using Bars.Gkh.RegOperator.Entities;
using System;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities
{
    public class AgentPIRDebtor : BaseEntity
    {
        /// <summary>
        /// Агент ПИР
        /// </summary>
        public virtual AgentPIR AgentPIR { get; set; }

        /// <summary>
        /// Статус задолжности
        /// </summary>
        public virtual AgentPIRDebtorStatus Status { get; set; }

        /// <summary>
        /// ЛС
        /// </summary>
        public virtual BasePersonalAccount BasePersonalAccount { get; set; }

        /// <summary>
        /// Задолженность по БТ
        /// </summary>
        public virtual decimal DebtBaseTariff { get; set; }

        /// <summary>
        /// Пени
        /// </summary>
        public virtual decimal PenaltyDebt { get; set; }

        /// <summary>
        /// Дата возникновения задолженности
        /// </summary>
        public virtual DateTime? DebtStartDate { get; set; }

        /// <summary>
        /// Дата окончания задолженности
        /// </summary>
        public virtual DateTime? DebtEndDate { get; set; }

        /// <summary>
        /// Использовать спецдату
        /// </summary>
        public virtual bool UseCustomDate { get; set; }

        /// <summary>
        /// Спецдата
        /// </summary>
        public virtual DateTime? CustomDate { get; set; }

        /// <summary>
        /// Способ погашения задолженности
        /// </summary>
        public virtual DebtCalc? DebtCalc { get; set; }

        /// <summary>
        /// Способ начисления пени
        /// </summary>
        public virtual PenaltyCharge? PenaltyCharge { get; set; }

        /// <summary>
        /// Выписка
        /// </summary>
        public virtual FileInfo Ordering { get; set; }
    }
}
