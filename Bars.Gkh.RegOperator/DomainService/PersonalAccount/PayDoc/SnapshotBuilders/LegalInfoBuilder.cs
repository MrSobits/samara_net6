namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders
{
    using System;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments;

    using NHibernate;
    using NHibernate.Linq;
    using System.Collections.Generic;
    using System.Text;

    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.B4;

    /// <summary>
    /// Источник информации юр.лиц
    /// </summary>
    public class LegalInfoBuilder : AbstractSnapshotBuilder
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id => nameof(LegalInfoBuilder);

        /// <summary>
        /// Код источника
        /// </summary>
        public override string Code => LegalInfoBuilder.Id;

        /// <summary>
        /// НаименованиеРуководитель
        /// </summary>
        public override string Name => "Источник информации юр.лиц";

        /// <summary>
        /// Описание заполняемых полей 
        /// </summary>
        public override string Description => "OwnerId; ПечататьАкт; Плательщик; ИННСобственника; КППСобственника; ContragentId; " +
            "РуководительПолучателя; ГлБухПолучателя; ПериодНачислений; Наименование; РСчетБанка; ОКПО; ОКОНХ; БИК; КорреспондентскийСчет; " +
            "Руководитель; ГлавБух; OwnerType; АдресКонтрагента; АдресКонтрагентаСИндексом; ПочтовыйАдресКонтрагента; ПочтовыйИндексКонтрагента";

        private Dictionary<long, long> accountOwnershipDict;
        private Dictionary<long, LegalAccOwnerProxy> accountOwnersDict;
        private Dictionary<long, Dictionary<string, string>> contragentContactsDict;
        private Dictionary<string, string> contactsDict;
        private Dictionary<long, ContragentBankCreditOrgProxy> contragentBankCreditOrgsDict;

        /// <summary>
        /// Инициализация кэша
        /// </summary>
        /// <param name="docCache">Кэш, в котором регистрируются сущности</param>
        /// <param name="mainInfo">Основной запрос</param>
        /// <param name="session">Сессия</param>
        public override void InitCache(DocCache docCache, PersonalAccountRecord[] mainInfo, IStatelessSession session)
        {
            this.accountOwnershipDict = docCache.AccountOwnershipDict;

            var ownerIds = this.accountOwnershipDict.Values.Distinct().ToArray();

            var legalOwnerQuery = session.Query<LegalAccountOwner>()
                .WhereContains(x => x.Id, ownerIds);

            docCache.Cache.RegisterEntity<LegalAccountOwner>()
                .SetQueryBuilder(r => r.GetAll()
                    .Fetch(y => y.Contragent)
                    .ThenFetch(y => y.FiasJuridicalAddress)
                    .Fetch(y => y.Contragent)
                    .ThenFetch(y => y.FiasMailingAddress)
                    .Fetch(y => y.Contragent)
                    .ThenFetch(y => y.FiasFactAddress)
                    .Where(y => ownerIds.Contains(y.Id)));

            docCache.Cache.RegisterEntity<ContragentContact>()
                .SetQueryBuilder(r => r.GetAll()
                    .Fetch(x => x.Contragent)
                    .Fetch(x => x.Position)
                    .Where(x => legalOwnerQuery.Any(o => o.Contragent == x.Contragent)
                        || session.Query<RegOperator>().Any(o => o.Contragent == x.Contragent))
                    .WhereIf(docCache.Period.EndDate.HasValue, x => x.DateStartWork == null || x.DateStartWork <= docCache.Period.EndDate)
                    .Where(x => x.DateEndWork == null || x.DateEndWork >= docCache.Period.StartDate));

            docCache.Cache.RegisterEntity<ContragentBankCreditOrg>()
                .SetQueryBuilder(r => r.GetAll()
                    .Fetch(x => x.Contragent)
                    .Fetch(x => x.CreditOrg)
                    .Where(x => legalOwnerQuery.Any(o => o.Contragent == x.Contragent)));
        }

        /// <summary>
        /// Получение конкретных данных из кэша для последующей работы
        /// </summary>
        /// <param name="creator">Инициатор</param>
        public override void WarmCache(SnapshotCreator creator)
        {
            this.accountOwnersDict = creator.Cache.GetCache<LegalAccountOwner>().GetEntities()
                .Select(x => new LegalAccOwnerProxy
                {
                    Id = x.Id,
                    ContragentName = x.Contragent.With(c => c.Name),
                    Inn = x.Contragent.Inn,
                    Kpp = x.Contragent.Kpp,

                    AddressOutSubjectName = x.Contragent.Return(c => c.AddressOutsideSubject),
                    FiasJuridicalAddress = x.Contragent.With(c => c.FiasJuridicalAddress),
                    FiasFactAddress = x.Contragent.With(c => c.FiasFactAddress),
                    FiasMailingAddress = x.Contragent.With(c => c.FiasMailingAddress),

                    ContragentId = x.Contragent.Return(z => z.Id),
                    PrintAct = x.PrintAct
                })
                .ToDictionary(x => x.Id);

            this.contragentContactsDict = creator.Cache.GetCache<ContragentContact>().GetEntities()
                .Where(x => x.Position != null)
                .Select(x => new { x.Contragent.Id, x.FullName, PositionName = x.Position.Name })
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key,
                    x => x.GroupBy(y => y.PositionName.ToUpper())
                        .ToDictionary(y => y.Key, y => y.First().FullName));

            this.contactsDict = creator.Cache.GetCache<ContragentContact>().GetEntities()
                .Where(x => x.Position != null)
                .Where(x => creator.RegOperator.Contragent.Id == x.Contragent.Id)
                .GroupBy(x => x.Position.Name.Trim().ToUpper())
                .ToDictionary(x => x.Key,
                    y => y.Return(x =>
                    {
                        var rec = y.First();
                        return string.Format(
                            "{0} {1}. {2}.",
                            rec.Surname,
                            rec.Name.ElementAtOrDefault(0),
                            rec.Patronymic.ElementAtOrDefault(0));
                    }));

            var contragentIdList = this.accountOwnersDict.Select(x => x.Value.ContragentId).ToArray();

            var contragentBankCreditOrgDomain = creator.Cache.GetCache<ContragentBankCreditOrg>();

            this.contragentBankCreditOrgsDict = contragentBankCreditOrgDomain.GetEntities()
                .Where(x => contragentIdList.Contains(x.Contragent.Id))
                .Select(
                    x => new ContragentBankCreditOrgProxy
                    {
                        ContragentId = x.Contragent.Id,
                        Name = x.Name,
                        Okpo = x.Okpo,
                        Okonh = x.Okonh,
                        Bik = x.Bik,
                        CorrAccount = x.CorrAccount,
                        SettlementAccount = x.SettlementAccount
                    })
                .AsEnumerable()
                .GroupBy(x => x.ContragentId)
                .ToDictionary(x => x.Key, x => x.First());
        }

        /// <summary>
        /// Заполнение одной записи модели с использованием данных, полученных в WarmCache 
        /// </summary>
        /// <param name="creator">Инициатор</param>
        /// <param name="record">Запись</param>
        /// <param name="account">Информация о лс</param>
        public override void FillRecord(
            SnapshotCreator creator,
            InvoiceInfo record,
            PersonalAccountPaymentDocProxy account)
        {
            //получаем владельца, который передан фильтром
            var ownerId = this.accountOwnershipDict.Get(account.Id);

            if (!this.accountOwnersDict.ContainsKey(ownerId))
            {
                return;
            }

            var accountOwner = this.accountOwnersDict[ownerId];

            record.OwnerId = ownerId;
            record.ПечататьАкт = accountOwner.PrintAct;

            record.Плательщик = accountOwner.ContragentName;
            record.ИННСобственника = accountOwner.Inn;
            record.КППСобственника = accountOwner.Kpp;
            record.ContragentId = accountOwner.ContragentId;

            //идиотская логика. 
            //АдресКонтрагента заполняется по порядку юр адрес-факт адрес-почтовый адрес-адрес за пределами
            //АдресКонтрагентаСИндексом наоборот: адрес за пределами-почтовый-факт-юр
            var address = accountOwner.FiasMailingAddress ?? accountOwner.FiasFactAddress ?? accountOwner.FiasJuridicalAddress;  
            
            record.АдресКонтрагентаСИндексом = !string.IsNullOrEmpty(accountOwner.AddressOutSubjectName)
                ? accountOwner.AddressOutSubjectName
                : (address != null)
                    ? string.Format("{0}, {1}", address.PostCode, address.AddressName)
                    : account.AddressName;

            address = accountOwner.FiasJuridicalAddress ?? accountOwner.FiasFactAddress ?? accountOwner.FiasMailingAddress;

            record.АдресКонтрагента = (address != null)
                ? address.AddressName
                : accountOwner.AddressOutSubjectName;

            record.ИндексАдресаКонтрагента = address?.PostCode ?? "";

            record.РуководительПолучателя = this.contactsDict.Get("РУКОВОДИТЕЛЬ");
            record.ГлБухПолучателя = this.contactsDict.Get("ГЛАВНЫЙ БУХГАЛТЕР");

            record.ПериодНачислений = creator.isZeroPayment
                ? string.Empty
                : "за период с {0} года по {1} года".FormatUsing(
                    record.ДатаНачалаПериода,
                    record.ДатаОкончанияПериода);

            var contragentCreditOrg = this.contragentBankCreditOrgsDict.Get(accountOwner.ContragentId);
            if (contragentCreditOrg != null)
            {
                record.Наименование = contragentCreditOrg.Name;
                record.РСчетБанка = contragentCreditOrg.SettlementAccount;
                record.ОКПО = contragentCreditOrg.Okpo;
                record.ОКОНХ = contragentCreditOrg.Okonh;
                record.БИК = contragentCreditOrg.Bik;
                record.КорреспондентскийСчет = contragentCreditOrg.CorrAccount;
            }

            
            if (accountOwner.FiasMailingAddress != null)
            {
                this.ApplyAddressInfo(record, accountOwner.FiasMailingAddress);
            }
            else if (accountOwner.FiasJuridicalAddress != null)
            {
                this.ApplyAddressInfo(record, accountOwner.FiasJuridicalAddress);
            } 
            else
            {
                this.ApplyAddressInfo(record, accountOwner.FiasMailingAddress);
            }

            if (this.contragentContactsDict.ContainsKey(accountOwner.ContragentId))
            {
                var regopContact = this.contragentContactsDict[accountOwner.ContragentId];

                record.Руководитель = regopContact.Get("РУКОВОДИТЕЛЬ") ?? string.Empty;
                record.ГлавБух = regopContact.Get("ГЛАВНЫЙ БУХГАЛТЕР") ?? string.Empty;
            }

            record.OwnerType = (int)PersonalAccountOwnerType.Legal;
        }

        private void ApplyAddressInfo(InvoiceInfo record, FiasAddress address)
        {
            if (address == null) return;
            record.ПочтовыйИндексКонтрагента = address.PostCode;

            // По задаче 44880 адрес должен выводится не так: Камчатский край, Карагинский р-н, с. Карага, ул. Лукашевского, д. 14
            // а вот так: ул. Лукашевского, д. 14, с. Карага, Карагинский р-н, Камчатский край
            var mailingAdress = address.AddressName;

            var streetAddress = string.Empty;

            if (address.StreetName != null)
            {
                var streetNameIndex = mailingAdress.IndexOf(address.StreetName, StringComparison.Ordinal);

                if (streetNameIndex < 0)
                {
                    throw new Exception($"Адрес улицы не совпадает с почтовым адресом: '{mailingAdress}' и '{address.StreetName}'");
                }

                streetAddress = address.StreetName.IsNotEmpty()
                    ? mailingAdress.Substring(streetNameIndex)
                    : string.Empty;
            }

            var other = streetAddress.IsNotEmpty()
                ? mailingAdress.Replace(streetAddress, string.Empty)
                : mailingAdress;

            var splitOther = other.Split(',');

            var idx = splitOther.Length - 1;

            // теперь оставшуюся част ьадреса пишем в обратнубю сторону чтобы получилось так
            // ул. Лукашевского, д. 14, с. Карага, Карагинский р-н, Камчатский край

            var addressBuilder = new StringBuilder();
            addressBuilder.Append(streetAddress);
            while (idx >= 0)
            {
                var item = splitOther[idx];
                if (item.IsNotEmpty())
                {
                    addressBuilder.AppendFormat(", {0}", item.Trim());
                }
                idx--;
            }
            
            record.ПочтовыйАдресКонтрагента = addressBuilder.ToString();
        }
    }

    public class LegalAccOwnerProxy
    {
        public long Id;
        public string ContragentName;
        public string Inn;
        public string Kpp;
        public string AddressOutSubjectName;
        public FiasAddress FiasJuridicalAddress;
        public FiasAddress FiasFactAddress;
        public FiasAddress FiasMailingAddress;
        public long ContragentId;
        public bool PrintAct;
    }

    public class ContragentBankCreditOrgProxy
    {
        public long ContragentId;
        public string Name;
        public string Okpo;
        public string Okonh;
        public string SettlementAccount;
        public string CorrAccount;
        public string Bik;
    }
}