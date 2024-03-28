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
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments;

    using NHibernate;
    using NHibernate.Linq;

    /// <summary>
    /// Источник по задолженностям
    /// </summary>
    public class DebtorInfoBuilder : AbstractSnapshotBuilder
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id => nameof(DebtorInfoBuilder);

        /// <summary>
        /// Код источника
        /// </summary>
        public override string Code => DebtorInfoBuilder.Id;

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Источник по задолженностям";

        /// <summary>
        /// Описание заполняемых полей 
        /// </summary>
        public override string Description => 
            "СтатусЗадолженности; " +
            "КоличествоДнейПросрочки; " +
            "КоличествоМесяцевПросрочки; " +
            "ЕдиницаПериодаПросрочки; " +
            "ПериодПросрочки; " +
            "ПечататьУведомление; " +
            "УчитыватьСумму";

        private Dictionary<long, ClaimWorkAccountDetail> claimWorkAccountDetails;
        private string notifPeriodKind;
        private int notifDelayDaysCount;
        private string notifPrintType;
        private string notifDebtSumType;

        /// <summary>
        /// Инициализация кэша
        /// </summary>
        /// <param name="docCache">Кэш, в котором регистрируются сущности</param>
        /// <param name="mainInfo">Основной запрос</param>
        /// <param name="session">Сессия</param>
        public override void InitCache(DocCache docCache, PersonalAccountRecord[] mainInfo, IStatelessSession session)
        {
            var accountQuery = session.Query<BasePersonalAccount>()
                .WhereContains(x => x.Id, docCache.AccountIds);

            docCache.Cache.RegisterEntity<ClaimWorkAccountDetail>()
                .SetQueryBuilder(r => r.GetAll()
                    .Fetch(x => x.ClaimWork)
                    .ThenFetch(x => x.State)
                    .Fetch(x => x.PersonalAccount)
                    .Where(z => accountQuery.Any(x => x.Id == z.PersonalAccount.Id))
                    .Where(z => !z.ClaimWork.State.FinalState));
        }

        /// <summary>
        /// Получение конкретных данных из кэша для последующей работы
        /// </summary>
        /// <param name="creator">Инициатор</param>
        public override void WarmCache(SnapshotCreator creator)
        {
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
            var claimWorkAccountDetail = this.claimWorkAccountDetails.Get(account.Id);

            if (claimWorkAccountDetail != null)
            {
                var debtor = claimWorkAccountDetail.ClaimWork as DebtorClaimWork;
                if (debtor != null)
                {
                    record.СтатусЗадолженности = debtor.DebtorState.GetDisplayName();
                }
                else
                {
                    record.СтатусЗадолженности = claimWorkAccountDetail.ClaimWork.State != null
                        ? claimWorkAccountDetail.ClaimWork.State.Name
                        : string.Empty;
                }

                record.КоличествоДнейПросрочки = claimWorkAccountDetail.CountDaysDelay.ToInt();
                record.КоличествоМесяцевПросрочки = claimWorkAccountDetail.CountMonthDelay;
            }

            record.ЕдиницаПериодаПросрочки = this.notifPeriodKind;
            record.ПериодПросрочки = this.notifDelayDaysCount;
            record.ПечататьУведомление = this.notifPrintType;
            record.УчитыватьСумму = this.notifDebtSumType;
        }
    }
}