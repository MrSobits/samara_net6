using System.Collections.Generic;
using System.Linq;

namespace Bars.Gkh.RegOperator.DomainService.RealityObjectAccount.Impl
{
    using System;

    using Gkh.Entities;

    public class EmptyBankDataProvider : IBankAccountDataProvider
    {
        public virtual string GetBankAccountNumber(RealityObject ro)
        {
            return string.Empty;
        }

        public virtual AccInfoProxy GetBankAccountInfo(long roId)
        {
            return new AccInfoProxy();
        }

        public virtual Dictionary<long, string> GetBankNumbersForCollection(IEnumerable<RealityObject> ros)
        {
            return ros.Distinct().ToDictionary(x => x.Id, x => string.Empty);
        }
    }

    public class AccInfoProxy
    {
        public long CalcAccId { get; set; }
        public string BankAccountNum { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
    }
}