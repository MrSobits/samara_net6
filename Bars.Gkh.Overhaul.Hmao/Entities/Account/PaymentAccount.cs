using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;

    public class PaymentAccount : BankAccount
    {
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
