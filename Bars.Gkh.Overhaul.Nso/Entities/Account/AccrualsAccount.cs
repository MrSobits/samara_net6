using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using Gkh.Entities;

    /// <summary>
    /// Счет начислений
    /// </summary>
    public class AccrualsAccount : BankAccount
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Баланс
        /// </summary>
        public virtual decimal OpeningBalance { get; set; }
    }
}
