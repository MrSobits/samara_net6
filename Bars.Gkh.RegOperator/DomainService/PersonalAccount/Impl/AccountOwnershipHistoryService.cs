namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    /// <summary>
    /// Сервис работы с историей принадлежности лс абоненту
    /// </summary>
    public class AccountOwnershipHistoryService : IAccountOwnershipHistoryService
    {
        public IDomainService<AccountOwnershipHistory> OwnershipHistoryDomain { get; set; }

        /// <inheritdoc />
        public PersonalAccountOwner GetOwner(long accountId, IPeriod period)
        {
            return this.OwnershipHistoryDomain.GetAll()
                .Where(x => x.PersonalAccount.Id == accountId)
                .Where(x => x.Date <= (period.EndDate ?? DateTime.Today))
                .OrderByDescending(x => x.Date)
                .FirstOrDefault()?.AccountOwner;            
        }

        /// <inheritdoc />
        public Dictionary<long, long> GetOwners(long[] accountIds, IPeriod period)
        {
            return this.OwnershipHistoryDomain.GetAll()
                .Where(x => accountIds.Contains(x.PersonalAccount.Id))
                .Where(x => x.Date <= (period.EndDate ?? DateTime.Today))
                .GroupBy(x => x.PersonalAccount.Id)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Date).Select(y => y.AccountOwner.Id).First());
        }

        /// <inheritdoc />
        public List<BasePersonalAccount> GetAccounts(long ownerId, IPeriod period)
        {
            return this.OwnershipHistoryDomain.GetAll()
                .Where(x => x.AccountOwner.Id == ownerId)
                .Where(x => x.Date <= (period.EndDate ?? DateTime.Today))
                .GroupBy(x => x.PersonalAccount.Id)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Date).First())
                .Select(x => x.Value.PersonalAccount)
                .ToList();
        }


    }
}
