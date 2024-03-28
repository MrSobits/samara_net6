using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using System;

    using Bars.Gkh.Entities;

    public class RealAccount : BankAccount
    {
        /// <summary>
        /// Владелец счета
        /// </summary>
        public virtual Contragent AccountOwner { get; set; }

        /// <summary>
        /// Лимит по овердрафту
        /// </summary>
        public virtual Double OverdraftLimit { get; set; }
    }
}
