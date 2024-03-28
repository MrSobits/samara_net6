namespace Bars.Gkh.RegOperator.Domain.ValueObjects
{
    using System.Collections.Generic;
    using Entities;
    using Entities.ValueObjects;

    /// <summary>Результат оплаты по ЛС</summary>
    public class PersonalAccountPaymentResult
    {
        public PersonalAccountPaymentResult()
        {
            DistributionResult = new AccountDistributionResult();
            Transfers = new List<Transfer>();
        }

        public AccountDistributionResult DistributionResult { get; set; }

        public List<Transfer> Transfers { get; set; }
        
        public void AddTransfer(Transfer transfer)
        {
            if (transfer != null)
            {
                Transfers.Add(transfer);
            }
        }
    }
}