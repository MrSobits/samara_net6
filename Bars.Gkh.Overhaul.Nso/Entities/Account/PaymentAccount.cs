using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using Bars.Gkh.Entities;

    public class PaymentAccount : BankAccount
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Владелец счета
        /// </summary>
        public virtual Contragent AccountOwner { get; set; }

        /// <summary>
        /// Лимит по овердрафту
        /// </summary>
        public virtual decimal OverdraftLimit { get; set; }
    }
}
