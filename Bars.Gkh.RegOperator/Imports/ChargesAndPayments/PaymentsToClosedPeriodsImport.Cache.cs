namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.B4.IoC;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Entities;
    using Entities.ValueObjects;
    using Gkh.Utils;
    using NHibernate.Linq;

    public partial class PaymentsToClosedPeriodsImport
    {
        private void InitCache(Payment[] payments)
        {
            var innerNumbers = payments
                .Select(x => x.InnerNumber)
                .Where(x => !x.IsEmpty())
                .Distinct()
                .ToArray();

            var externalNumbers = payments
                .Select(x => x.ExternalNumber)
                .Where(x => !x.IsEmpty())
                .Distinct()
                .ToArray();

            var accounts = this.Container.ResolveDomain<BasePersonalAccount>().GetAll()
                .Where(x => innerNumbers.Contains(x.PersonalAccountNum)
                    || externalNumbers.Contains(x.PersAccNumExternalSystems))
                .Fetch(x => x.Room)
                .ThenFetch(x => x.RealityObject);

            this.accountsByInnerNumber = accounts
                .Select(x => new
                {
                    x.Id,
                    x.PersonalAccountNum
                })
                .ToArray()
                .ToDictionary(x => x.PersonalAccountNum, x => x.Id);

            this.accountsByExternalNumber = accounts
                .Select(x => new
                {
                    x.Id,
                    x.PersAccNumExternalSystems
                })
                .ToArray()
                .Where(x => !x.PersAccNumExternalSystems.IsEmpty())
                .GroupBy(x => x.PersAccNumExternalSystems)
                .ToDictionary(x => x.Key, x => x.Select(z => z.Id).ToArray());

            this.accountsById = accounts.ToDictionary(x => x.Id);

            var roIds = accounts
                .Select(x => x.Room.RealityObject.Id)
                .ToArray();

            var accountIds = accounts
                .Select(x => x.Id)
                .ToArray();

            var cashCenterIds = payments
                .Select(x => x.InnerRkcId)
                .Where(x => !x.IsEmpty())
                .Select(x => x.ToLong())
                .Distinct()
                .ToArray();

            var cashCenterNumbers = payments
                .Select(x => x.ExternalRkcId)
                .Where(x => !x.IsEmpty())
                .Distinct()
                .ToArray();

            var cachPaymentCenterConnectionType = this.Container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.CachPaymentCenterConnectionType;
            CashPaymentCenterPersAccProxy[] cashCentersAndAccounts;
            
            var periodEndDate = this.period.EndDate ?? this.period.StartDate.AddMonths(1).AddDays(-1); // Выбирать на дату конца (периода). Если вдруг импорт делается по текущему периоду, то конец это последний день месяца.
            if (cachPaymentCenterConnectionType == CachPaymentCenterConnectionType.ByAccount)
            {
                cashCentersAndAccounts = this.Container.ResolveDomain<CashPaymentCenterPersAcc>().GetAll()
                    .Where(x => cashCenterIds.Contains(x.CashPaymentCenter.Id) || cashCenterNumbers.Contains(x.CashPaymentCenter.Identifier))
                    .Where(x => accountIds.Contains(x.PersonalAccount.Id))
                    .Where(x => x.DateStart <= periodEndDate)
                    .Where(x => !x.DateEnd.HasValue || x.DateEnd.Value >= periodEndDate)
                    .Select(x => new CashPaymentCenterPersAccProxy
                    {
                        Id = x.CashPaymentCenter.Id,
                        Identifier = x.CashPaymentCenter.Identifier,
                        PersonalAccountId = x.PersonalAccount.Id
                    })
                    .ToArray();
            }
            else
            {
                var accountRoIds = accounts.ToDictionary(x => x.Id, y => y.Room.RealityObject.Id);
                var cashCentersByRo = this.Container.ResolveDomain<CashPaymentCenterRealObj>().GetAll()
                    .Where(x => cashCenterIds.Contains(x.CashPaymentCenter.Id) || cashCenterNumbers.Contains(x.CashPaymentCenter.Identifier))
                    .Where(x => accountRoIds.Values.Distinct().Contains(x.RealityObject.Id))
                    .Where(x => x.DateStart <= periodEndDate)
                    .Where(x => !x.DateEnd.HasValue || x.DateEnd.Value >= periodEndDate)
                    .ToDictionary(x => x.RealityObject.Id);


                cashCentersAndAccounts = accountRoIds
                    .Select(x => new CashPaymentCenterPersAccProxy
                    {
                        Id = cashCentersByRo.Get(x.Value).ReturnSafe(y => y.CashPaymentCenter.Id),
                        Identifier = cashCentersByRo.Get(x.Value).ReturnSafe(y => y.CashPaymentCenter.Identifier),
                        PersonalAccountId = x.Key
                    })
                    .ToArray();
            }

            this.accountsByCashCenterId = cashCentersAndAccounts
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, z => z.Select(x => x.PersonalAccountId).ToHashSet());

            this.accountsByCashCenterNumber = cashCentersAndAccounts
                .Where(x => !x.Identifier.IsEmpty())
                .GroupBy(x => x.Identifier)
                .ToDictionary(x => x.Key, z => z.Select(x => x.PersonalAccountId).ToHashSet());

            this.accountSummaries = this.Container.ResolveDomain<PersonalAccountPeriodSummary>().GetAll()
                .Where(x => accountIds.Contains(x.PersonalAccount.Id))
                .Where(x => x.Period.Id == this.period.Id)
                .ToArray()
                .ToDictionary(x => x.PersonalAccount.Id);

            this.roAccountSummaries = this.Container.ResolveDomain<RealityObjectChargeAccountOperation>().GetAll()
                .Where(x => roIds.Contains(x.Account.RealityObject.Id))
                .Where(x => x.Period.Id == this.period.Id)
                .Select(x => new { RoId = x.Account.RealityObject.Id, Summary = x })
                .ToArray()
                .ToDictionary(x => x.RoId, x => x.Summary);

            this.roChargeAccounts = this.Container.ResolveDomain<RealityObjectChargeAccount>().GetAll()
                .Where(x => roIds.Contains(x.RealityObject.Id))
                .Select(x => new { RoId = x.RealityObject.Id, Account = x })
                .ToArray()
                .ToDictionary(x => x.RoId, x => x.Account);

            // Для поиска по ФИО не подходит типовая конструкция "(x.AccountOwner as IndividualAccountOwner).Name ?? (x.AccountOwner as LegalAccountOwner).Contragent.Name".
            // Т.к. она даёт AccountOwner.Name. А там вместо ФИО лежит "XXXXX" в случае, если ещё не открывали карточку лицевого счёта.
            // По этому ФИО физ.лица собирается из IndividualAccountOwner.
            this.accountRequisites = this.Container.ResolveDomain<BasePersonalAccount>().GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.PersonalAccountNum,
                    Address = x.Room.RealityObject.Address + ", кв. " +
                        x.Room.RoomNum +
                        (x.Room.ChamberNum != "" && x.Room.ChamberNum != null ? ", ком. " + x.Room.ChamberNum : string.Empty),
                    Name = (x.AccountOwner as LegalAccountOwner).Contragent.Name ??
                                (x.AccountOwner as IndividualAccountOwner).Surname + " " +
                                (x.AccountOwner as IndividualAccountOwner).FirstName + " " +
                                (x.AccountOwner as IndividualAccountOwner).SecondName
                })
                .AsEnumerable()
                .Select(x => new
                {
                    SearchKey = $"{x.Address}#{x.Name}".ToUpper(),
                    x.Id,
                    x.PersonalAccountNum,
                    x.Name,
                    x.Address
                })
                .GroupBy(x => x.SearchKey)        
                .Where(x => x.Count() == 1)
                .ToDictionary(x => x.Key, 
                    y => y.Select(v => new accountRequisitesProxy { Id = v.Id, PersonalAccountNum = v.PersonalAccountNum, Name = v.Name, Address = v.Address })
                        .FirstOrDefault());

            this.InitPaymentCache();

            this.InitWalletCache(roIds);
        }

        private void InitPaymentCache()
        {
            var walletGuids = this.accountsById.Values.SelectMany(x => x.GetMainWalletGuids()).ToArray();

            var domain = this.Container.ResolveDomain<PersonalAccountPaymentTransfer>();
            using (this.Container.Using(domain))
            {
                var accountPayments = this.Container.ResolveDomain<PersonalAccountPaymentTransfer>().GetAll()
                    .Where(x => walletGuids.Contains(x.TargetGuid))
                    .Select(x => new { x.TargetGuid, x.Amount, x.PaymentDate })
                    .ToArray()
                    .GroupBy(x => x.TargetGuid)
                    .ToDictionary(x => x.Key, x => x.ToArray());

                var accountRefunds = this.Container.ResolveDomain<PersonalAccountPaymentTransfer>().GetAll()
                    .Where(x => walletGuids.Contains(x.SourceGuid))
                    .Select(x => new { x.SourceGuid, x.Amount, x.PaymentDate })
                    .ToArray()
                    .GroupBy(x => x.SourceGuid)
                    .ToDictionary(x => x.Key, x => x.ToArray());

                this.accountPayments = this.accountsById
                .Select(x => x.Key)
                .ToDictionary(x => x, x => new HashSet<string>());

                foreach (var account in this.accountsById)
                {
                    foreach (var guid in account.Value.GetMainWalletGuids())
                    {
                        if (accountPayments.ContainsKey(guid))
                        {
                            foreach (var pay in accountPayments.Get(guid))
                            {
                                this.accountPayments[account.Key].Add($"{guid}#{pay.PaymentDate:dd.MM.yyyy}#{pay.Amount.RegopRoundDecimal(2)}");
                            }
                        }

                        if (accountRefunds.ContainsKey(guid))
                        {
                            foreach (var pay in accountRefunds.Get(guid))
                            {
                                this.accountPayments[account.Key].Add($"{guid}#{pay.PaymentDate:dd.MM.yyyy}#{-pay.Amount.RegopRoundDecimal(2)}");
                            }
                        }
                    }
                }
            }
        }

        private void InitWalletCache(long[] roIds)
        {
            this.walletGuidByRoId =
                this.Container.ResolveDomain<RealityObjectPaymentAccount>()
                    .GetAll()
                    .Where(x => roIds.Contains(x.RealityObject.Id))
                    .Select(x => new
                    {
                        x.Id,
                        RoId = x.RealityObject.Id,
                        BaseTariffPaymentWallet = x.BaseTariffPaymentWallet.WalletGuid,
                        DecisionPaymentWallet = x.DecisionPaymentWallet.WalletGuid,
                        PenaltyPaymentWallet = x.PenaltyPaymentWallet.WalletGuid,
                    })
                    .AsEnumerable()
                    .ToDictionary(
                        x => x.RoId,
                        x => Tuple.Create(
                            x.Id,
                            new Dictionary<WalletType, string>
                            {
                                { WalletType.BaseTariffWallet, x.BaseTariffPaymentWallet },
                                { WalletType.DecisionTariffWallet, x.DecisionPaymentWallet },
                                { WalletType.PenaltyWallet, x.PenaltyPaymentWallet }
                            }));
        }

        private ChargePeriod period;

        private Dictionary<long, HashSet<long>> accountsByCashCenterId;
        private Dictionary<string, HashSet<long>> accountsByCashCenterNumber;
        private Dictionary<long, BasePersonalAccount> accountsById;

        private Dictionary<string, long> accountsByInnerNumber;
        private Dictionary<string, long[]> accountsByExternalNumber;

        private Dictionary<long, PersonalAccountPeriodSummary> accountSummaries;

        private Dictionary<long, RealityObjectChargeAccountOperation> roAccountSummaries;
        private Dictionary<long, RealityObjectChargeAccount> roChargeAccounts;

        // гуид кошелька по базовому тарифу счета оплат дома по идентификатору дома
        private Dictionary<long, Tuple<long, Dictionary<WalletType, string>>> walletGuidByRoId;

        // словарь оплат, сгруппированный по идентификатору лс
        // в качестве значения хешсет с ключами оплат вида сумма_оплаты#дата_оплаты
        private Dictionary<long, HashSet<string>> accountPayments;

        // Реквизиты лицевых счетов - ключ: Адрес#ФИО, значение: реквизиты. 
        // Используется для поиска ЛС, которые не были найдены по внешнему номеру и коду РЦ.        
        private Dictionary<string, accountRequisitesProxy> accountRequisites;

        private class CashPaymentCenterPersAccProxy
        {
            /// <summary>
            /// Идентификатор РКЦ в БД
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Идентификатор РКЦ
            /// </summary>
            public string Identifier { get; set; }


            /// <summary>
            /// Идентификатор ЛС
            /// </summary>
            public long PersonalAccountId { get; set; }
        }
        
        private class accountRequisitesProxy
        {
            /// <summary>
            /// Идентификатор ЛС
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Номер ЛС
            /// </summary>
            public string PersonalAccountNum { get; set; }

            /// <summary>
            /// ФИО
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Адрес
            /// </summary>
            public string Address { get; set; }
        }
    }
}
