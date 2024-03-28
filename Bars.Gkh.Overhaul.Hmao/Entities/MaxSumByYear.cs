using Bars.B4.DataAccess;
using Bars.Gkh.Entities;

namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    /// <summary>
    /// предельные стоимости в разрезе МО
    /// </summary>
    public class MaxSumByYear : BaseEntity
    {
        /// <summary>
        /// МО
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Программа
        /// </summary>
        public virtual ProgramVersion Program { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual short Year { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal Sum { get; set; }
    }
}
