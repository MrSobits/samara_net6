namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders
{
    using System;
    using System.Linq;
    
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments;

    using NHibernate;

    /// <summary>
    /// Источник даты последней оплаты
    /// </summary>
    public class LastPaymentDateBuilder : AbstractSnapshotBuilder
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id => nameof(LastPaymentDateBuilder);

        /// <summary>
        /// Код источника
        /// </summary>
        public override string Code => LastPaymentDateBuilder.Id;

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Источник даты последней оплаты";

        /// <summary>
        /// Описание заполняемых полей 
        /// </summary>
        public override string Description => "ДатаПоследнейОплаты";

        private DateTime? bankStatementMaxDate;

        /// <summary>
        /// Инициализация кэша
        /// </summary>
        /// <param name="docCache">Кэш, в котором регистрируются сущности</param>
        /// <param name="mainInfo">Основной запрос</param>
        /// <param name="session">Сессия</param>
        public override void InitCache(DocCache docCache, PersonalAccountRecord[] mainInfo, IStatelessSession session)
        {
            docCache.Cache.RegisterEntity<BankAccountStatement>()
                .SetQueryBuilder(
                    r => r.GetAll()
                        .Where(x => x.MoneyDirection == MoneyDirection.Income)
                        .Where(x => x.DistributeState == DistributionState.Distributed)
                        .OrderByDescending(x => x.DocumentDate).Take(1));
        }

        /// <summary>
        /// Получение конкретных данных из кэша для последующей работы
        /// </summary>
        /// <param name="creator">Инициатор</param>
        public override void WarmCache(SnapshotCreator creator)
        {
            this.bankStatementMaxDate = creator.Cache.GetCache<BankAccountStatement>()
               .GetEntities()
               .Max(x => x.DocumentDate as DateTime?);
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
            if (this.bankStatementMaxDate != null)
            {
                record.ДатаПоследнейОплаты = this.bankStatementMaxDate.Value.ToString("dd.MM.yyyy");
            }
        }
    }
}
