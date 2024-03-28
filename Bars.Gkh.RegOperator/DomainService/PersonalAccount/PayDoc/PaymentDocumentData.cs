namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc
{
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Хранилище информации по платежным документам
    /// </summary>
    internal class PaymentDocumentData
    {
        public const string AccountHolderType = "PersonalAccount";
        public const string OwnerholderType = "AccountOwner";

        private readonly IWindsorContainer container;
        private readonly ChargePeriod period;
        private readonly IEnumerable<PayDocPrimarySource> primarySources;

        private readonly List<Tuple<AccountPaymentInfoSnapshot, long, string>> subItems;
        private List<PaymentDocumentSnapshot> storedAccountsData;
        private List<OwnerInfo> storedOwnerData;

        public PaymentDocumentData(IWindsorContainer container, ChargePeriod period, IEnumerable<PayDocPrimarySource> primarySources)
        {
            this.container = container;
            this.period = period;
            this.primarySources = primarySources;

            this.subItems = new List<Tuple<AccountPaymentInfoSnapshot, long, string>>();
            
            this.PrepareData();
        }

        private void PrepareData()
        {
            this.storedAccountsData = new List<PaymentDocumentSnapshot>();

            var paymentDocumentSnapshotDomain = this.container.ResolveDomain<PaymentDocumentSnapshot>();
            var accountPaymentInfoSnapshotDomain = this.container.ResolveDomain<AccountPaymentInfoSnapshot>();

            using (this.container.Using(paymentDocumentSnapshotDomain, accountPaymentInfoSnapshotDomain))
            {
                if (this.primarySources.IsNotEmpty())
                {
                    this.storedAccountsData = paymentDocumentSnapshotDomain.GetAll()
                        .Where(x => x.HolderType == PaymentDocumentData.AccountHolderType)
                        .Where(x => x.Period.Id == this.period.Id)
                        .WhereContains(x => x.HolderId, this.primarySources.Select(x => x.AccountId))
                        .ToList();

                    this.storedOwnerData = accountPaymentInfoSnapshotDomain.GetAll()
                        .Where(a => a.Snapshot.HolderType == PaymentDocumentData.OwnerholderType)
                        .Where(a => a.Snapshot.Period.Id == this.period.Id)
                        .Where(a => a.Snapshot.IsBase)
                        .WhereContains(a => a.Snapshot.HolderId, this.primarySources.Select(x => x.OwnerId).Distinct())
                        .WhereContains(a => a.AccountId, this.primarySources.Select(x => x.AccountId))
                        .Select(x => new
                        {
                            x.Snapshot,
                            AccountInfo = x
                        })
                        .ToList()
                        .GroupBy(x => x.Snapshot, FnEqualityComparer<PaymentDocumentSnapshot>.Member(x => x.Id))
                        .Select(x => new OwnerInfo
                        {
                            Owner = x.Key,
                            Accounts = x.Select(a => a.AccountInfo).ToList()
                        })
                        .ToList();
                }
            }
        }
        
        /// <summary>
        /// Получить слепки, относящиеся к лс
        /// </summary>
        /// <returns>Список слепков</returns>
        public List<PaymentDocumentSnapshot> GetAccountData()
        {
            return this.storedAccountsData;
        }

        /// <summary>
        /// Получить слепки всех типов
        /// </summary>
        /// <returns>Список слепков</returns>
        public List<PaymentDocumentSnapshot> GetAllAccountData()
        {
            return this.storedAccountsData.Union(this.storedOwnerData.SelectMany(o => o.Accounts).Select(o => o.Snapshot)).ToList();
        }

        /// <summary>
        /// Получить информацию по абонентам (реестр юр лиц)
        /// </summary>
        /// <returns>Список объектов с информацией об абоненте</returns>
        public List<OwnerInfo> GetOwnerData()
        {
            return this.storedOwnerData;
        }
        
        /// <summary>
        /// Добавить слепок в хранилище
        /// </summary>
        /// <param name="snapshot">Слепок</param>
        public void AddItem(PaymentDocumentSnapshot snapshot)
        {
            if (snapshot.HolderType == PaymentDocumentData.AccountHolderType)
            {
                if (this.storedAccountsData.All(x => x.HolderId != snapshot.HolderId))
                {
                    this.storedAccountsData.Add(snapshot);
                }
            }
            else
            {
                if (this.storedOwnerData.All(x => x.Owner.HolderId != snapshot.HolderId || !snapshot.IsBase))
                {
                    var ownerInfo = new OwnerInfo(snapshot.IsBase) {Owner = snapshot};
                    this.storedOwnerData.Add(ownerInfo);

                    var subItems = this.subItems
                        .Where(x => x.Item3 == PaymentDocumentData.OwnerholderType && x.Item2 == ownerInfo.Owner.HolderId)
                        .ToList();
                    if (subItems.Any())
                    {
                        subItems.Select(x => x.Item1).AddTo(ownerInfo.Accounts);
                        subItems.ForEach(x => this.subItems.Remove(x));
                    }
                }
            }
        }

        /// <summary>
        /// Добавить детализацию по лс
        /// </summary>
        /// <param name="accountInfo">Слепок детализации</param>
        /// <param name="parentItemId">Родительский элемент</param>
        /// <param name="holderType">Тип плательщика</param>
        /// <param name="isBase">Базовый слепок</param>
        public void AddSubItem(AccountPaymentInfoSnapshot accountInfo, long parentItemId, string holderType, bool isBase)
        {
            if (holderType == AccountHolderType)
            {
                this.AddToSubItems(accountInfo, parentItemId, holderType);
            }
            else
            {
                var ownerInfo = this.storedOwnerData.FirstOrDefault(x => x.Owner.HolderId == parentItemId && x.IsBase == isBase);
                if (ownerInfo != null)
                {
                    if (ownerInfo.Accounts.All(x => x.AccountId != accountInfo.AccountId || x.Services != accountInfo.Services || !isBase))
                    {
                        if (accountInfo.Snapshot == null)
                        {
                            accountInfo.Snapshot = ownerInfo.Owner;
                        }
                        ownerInfo.Accounts.Add(accountInfo);
                        this.subItems.Remove(Tuple.Create(accountInfo, parentItemId, holderType));
                    }
                }
                else
                {
                    this.AddToSubItems(accountInfo, parentItemId, holderType);
                }
            }
        }
        
        /// <summary>
        /// Сохранение слепков данных для платежных документов
        /// </summary>
        public void SaveChanges()
        {
            var paymentDocumentSnapshotDomain = this.container.ResolveDomain<PaymentDocumentSnapshot>();
            var accountPaymentInfoSnapshotDomain = this.container.ResolveDomain<AccountPaymentInfoSnapshot>();

            using (this.container.Using(paymentDocumentSnapshotDomain, accountPaymentInfoSnapshotDomain))
            {
                using (var transaction = this.container.Resolve<IDataTransaction>())
                {
                    foreach (var source in this.storedAccountsData.Where(x => x.Id == 0))
                    {
                        paymentDocumentSnapshotDomain.Save(source);
                    }

                    foreach (var subItem in this.subItems)
                    {
                        var snapshot = this.storedAccountsData.FirstOrDefault(x => x.HolderId == subItem.Item2);
                        if (snapshot != null)
                        {
                            var accountInfo = subItem.Item1;
                            accountInfo.Snapshot = snapshot;

                            accountPaymentInfoSnapshotDomain.Save(accountInfo);
                        }
                    }

                    foreach (var ownerInfo in this.storedOwnerData)
                    {
                        if (ownerInfo.Owner.Id == 0)
                        {
                            paymentDocumentSnapshotDomain.Save(ownerInfo.Owner);
                        }

                        foreach (var source in ownerInfo.Accounts.Where(x => x.Id == 0))
                        {
                            source.Snapshot = ownerInfo.Owner;
                            accountPaymentInfoSnapshotDomain.Save(source);
                        }
                    }

                    transaction.Commit();
                }
            }
        }

        private void AddToSubItems(AccountPaymentInfoSnapshot accountInfo, long parentItemId, string holderType)
        {
            if (this.subItems.All(
                x => x.Item1.AccountId != accountInfo.AccountId
                     || x.Item2 != parentItemId
                     || x.Item3 != holderType))
            {
                this.subItems.Add(Tuple.Create(accountInfo, parentItemId, holderType));
            }
        }
    }

    internal class OwnerInfo
    {
        public OwnerInfo(bool isBase = true)
        {
            this.Accounts = new List<AccountPaymentInfoSnapshot>();
            this.IsBase = isBase;
        }

        public bool IsBase { get; set; }

        public PaymentDocumentSnapshot Owner { get; set; }

        public List<AccountPaymentInfoSnapshot> Accounts { get; set; }
    }
}