namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments;

    using NHibernate;
    using NHibernate.Linq;

    /// <summary>
    /// Источник с информацией физ. лиц
    /// </summary>
    public class IndividualInfoBuilder : AbstractSnapshotBuilder
    {
        public static string Id => nameof(IndividualInfoBuilder);

        /// <summary>
        /// Код источника
        /// </summary>
        public override string Code => IndividualInfoBuilder.Id;

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Источник с информацией физ. лиц";

        /// <summary>
        /// Описание заполняемых полей 
        /// </summary>
        public override string Description => "OwnerId; OwnerType; ФИОСобственника; ФамилияCобственника; ИмяCобственника; ОтчествоCобственника; ФактическийАдрес;";

        private Dictionary<long, long> accountOwnershipDict;
        private Dictionary<long, IndividualAccountInfo> accountOwnersDict;
        private bool depersonalized;

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

            var ownerQuery = session.Query<PersonalAccountOwner>().WhereContains(x => x.Id, ownerIds);

            docCache.Cache.RegisterEntity<IndividualAccountOwner>()
                .SetQueryBuilder(r => r.GetAll()
                    .Fetch(y => y.FiasFactAddress)
                    .Where(y => ownerQuery.Any(x => x == y)));
        }

        /// <summary>
        /// Получение конкретных данных из кэша для последующей работы
        /// </summary>
        /// <param name="creator">Инициатор</param>
        public override void WarmCache(SnapshotCreator creator)
        {
            var indivAccOwners = creator.Cache.GetCache<IndividualAccountOwner>();

            this.accountOwnersDict = indivAccOwners.GetEntities()
                .Select(x => new IndividualAccountInfo
                {
                    Id = x.Id,
                    Surname = x.Surname,
                    FirstName = x.FirstName,
                    SecondName = x.SecondName,
                    FiasFactAddressName = x.FiasFactAddress?.AddressName
                })
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.First());

            this.depersonalized =
                creator.RegopConfig.PaymentDocumentConfigContainer.PaymentDocumentConfigIndividual.PaymentDocFormat ==
                PaymentDocumentFormat.Depersonalized;
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
            var ownerId = this.accountOwnershipDict.Get(account.Id);

            if (!this.accountOwnersDict.ContainsKey(ownerId) || this.depersonalized)
            {
                return;
            }

            var accountOwner = this.accountOwnersDict[ownerId];

            record.OwnerId = ownerId;
            record.ФамилияCобственника = accountOwner.Surname;
            record.ИмяCобственника = accountOwner.FirstName;
            record.ОтчествоCобственника = accountOwner.SecondName;
            record.ФИОСобственника = "{0} {1} {2}".FormatUsing(accountOwner.Surname, accountOwner.FirstName, accountOwner.SecondName);

            record.OwnerType = (int)PersonalAccountOwnerType.Individual;
            record.ФактическийАдрес = accountOwner.FiasFactAddressName;
        }
    }

    public class IndividualAccountInfo
    {
        public long Id;
        public string Surname;
        public string FirstName;
        public string SecondName;
        public string FiasFactAddressName;
    }
}
