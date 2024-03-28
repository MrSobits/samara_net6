using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.Gkh.RegOperator.Dto
{
    using Bars.Gkh.Decisions.Nso.Entities;

    /// <summary>
    /// Решения по пеням
    /// </summary>
    public class PenaltyDelayDecisionDto
    {
        /// <summary>
        /// Идентификатор reality_object
        /// </summary>
        public long RoId { get; set; }

        /// <summary>
        /// Дата с
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Список решений
        /// </summary>
        public string Decision { get; set; }

        /// <summary>
        /// Список решений
        /// </summary>
        public List<OwnerPenaltyDelay> PenaltyDelays { get; set; }
    }
}
