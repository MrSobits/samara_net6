namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Entities;
    using Entities.ValueObjects;

    public interface IWalletBalanceService
    {
        Dictionary<string, WalletBalance> GetDebetBalance(
            IEnumerable<RealityObjectPaymentAccount> accounts,
            params Expression<Func<Transfer, bool>>[] filters);

        Dictionary<string, WalletBalance> GetCreditBalance(
            IEnumerable<RealityObjectPaymentAccount> accounts,
            params Expression<Func<Transfer, bool>>[] filters);
    }

    public class WalletBalance
    {
        public WalletBalance(string guid)
        {
            WalletGuid = guid;
        }

        /// <summary>
        /// Гуид кошелька
        /// </summary>
        public string WalletGuid { get; set; }

        /// <summary>
        /// Баланс кошелька
        /// </summary>
        public decimal Amount { get; set; }
    }
}