using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using System;

    /// <summary>
    /// Счет начислений
    /// </summary>
    public class AccrualsAccount : BankAccount
    {
        public virtual double OpeningBalance { get; set; }
    }
}
