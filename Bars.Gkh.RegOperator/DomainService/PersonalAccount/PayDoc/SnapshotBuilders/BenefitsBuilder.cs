using Bars.Gkh.RegOperator.Tasks.PaymentDocuments;

namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    using NHibernate;
    using NHibernate.Linq;

    /// <summary>
    /// Источник информации о льготах
    /// </summary>
    public class BenefitsBuilder : AbstractSnapshotBuilder
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id => nameof(BenefitsBuilder);

        /// <summary>
        /// Код источника
        /// </summary>
        public override string Code => BenefitsBuilder.Id;

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Источник информации о льготах";

        /// <summary>
        /// Описание заполняемых полей 
        /// </summary>
        public override string Description => "Льгота; КодЛьготы";

        private Dictionary<long, decimal> benefitsDict;
        private Dictionary<long, string> сodeDict;

        /// <summary>
        /// Инициализация кэша
        /// </summary>
        /// <param name="docCache">Кэш, в котором регистрируются сущности</param>
        /// <param name="mainInfo">Основной запрос</param>
        /// <param name="session">Сессия</param>
        public override void InitCache(DocCache docCache, PersonalAccountRecord[] mainInfo, IStatelessSession session)
        {
            docCache.Cache.RegisterEntity<PersonalAccountBenefits>()
                .SetQueryBuilder(r => r.GetAll()
                    .Fetch(x => x.Period)
                    .Where(x => x.Period.Id == docCache.Period.Id)
                    .Where(z => docCache.AccountIds.Contains(z.PersonalAccount.Id)));

            docCache.Cache.RegisterEntity<PersonalAccountPrivilegedCategory>()
                .SetQueryBuilder(r => r.GetAll()
                    .Fetch(x => x.PrivilegedCategory)
                    .Where(x => docCache.AccountIds.Contains(x.PersonalAccount.Id))
                    .Where(x => x.DateFrom <= docCache.Period.StartDate && x.DateTo >= docCache.Period.EndDate));
        }

        /// <summary>
        /// Получение конкретных данных из кэша для последующей работы
        /// </summary>
        /// <param name="creator">Инициатор</param>
        public override void WarmCache(SnapshotCreator creator)
        {
            this.benefitsDict = creator.Cache.GetCache<PersonalAccountBenefits>().GetEntities()
                .GroupBy(x => x.PersonalAccount.Id)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.Sum));

            this.сodeDict = creator.Cache.GetCache<PersonalAccountPrivilegedCategory>().GetEntities()
                .GroupBy(x => x.PersonalAccount.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.PrivilegedCategory.Code).First());
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
            record.Льгота = this.benefitsDict.Get(account.Id);
            record.КодЛьготы = this.сodeDict.Get(account.Id);
        }
    }
}