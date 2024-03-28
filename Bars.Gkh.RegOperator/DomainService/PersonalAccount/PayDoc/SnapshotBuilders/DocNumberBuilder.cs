namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders
{
    using System;
    using System.Linq;
    
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments;

    using NHibernate;

    /// <summary>
    /// Источник номеров квитанций
    /// </summary>
    public class DocNumberBuilder : AbstractSnapshotBuilder
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id => nameof(DocNumberBuilder);

        /// <summary>
        /// Код источника
        /// </summary>
        public override string Code => DocNumberBuilder.Id;

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Источник номеров квитанций";

        /// <summary>
        /// Описание заполняемых полей 
        /// </summary>
        public override string Description => "Номер; ПорядковыйНомерВГоду";

        /// <summary>
        /// Инициализация кэша
        /// </summary>
        /// <param name="docCache">Кэш, в котором регистрируются сущности</param>
        /// <param name="mainInfo">Основной запрос</param>
        /// <param name="session">Сессия</param>
        public override void InitCache(DocCache docCache, PersonalAccountRecord[] mainInfo, IStatelessSession session)
        {
            docCache.Cache.RegisterEntity<PaymentDocument>()
                .MakeDictionary("ReceiptAndActAccthis.periodYear",
                    x => "{0}|{1}|{2}".FormatUsing(x.AccountId, x.PeriodId, x.Year));
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
            record.Номер = this.GetDocNumber(creator, account.Id);
            record.ПорядковыйНомерВГоду = this.GetNextSerialNumber(creator);
        }

        private int GetDocNumber(SnapshotCreator creator, long accountId)
        {
            var cache = creator.Cache.GetCache<PaymentDocument>();

            // Если в рамках периода по ЛС уже печатали счета, то номер выводим уже существующий

            var existingNum = cache.GetByKey("{0}|{1}|{2}".FormatUsing(accountId, creator.Period.Id, DateTime.Today.Year));

            int documentNumber;

            if (existingNum.IsNull())
            {
                var maxExistingNumber = cache.GetEntities()
                    .Where(x => x.Year == DateTime.Today.Year)
                    .Max(x => (int?)x.Number) ?? 0;

                documentNumber = maxExistingNumber + 1;

                var newRecord = new PaymentDocument
                {
                    AccountId = accountId,
                    PeriodId = creator.Period.Id,
                    Year = DateTime.Today.Year,
                    Number = documentNumber
                };

                creator.PaymentDocDmn.Save(newRecord);
                cache.AddEntity(newRecord);
            }
            else
            {
                documentNumber = existingNum.Number;
            }
            return documentNumber;
        }

        private int GetNextSerialNumber(SnapshotCreator creator)
        {
            int serialNumber;
            // будем использовать уже существующую таблицу, которую никто не помнит зачем сделали
            var selectString = "SELECT DOC_NUMBER FROM REGOP_PAYMENT_DOC_PRINT";
            var updateString = "UPDATE REGOP_PAYMENT_DOC_PRINT SET DOC_NUMBER = :docNumber";
            var whereString = " WHERE DOC_YEAR = :year AND ACCOUNT_ID = -1 AND PERIOD_ID = -1";

            // всё таки нужно так делать, номер был уникальным и сквозным
            // лучше последовательностью сделать, но кто будет сбрасывать её каждый год
            lock (PaymentDocument.SyncObject)
            {
                var currentSession = creator.SessionProvider.GetCurrentSession();

                serialNumber = currentSession.CreateSQLQuery(selectString + whereString)
                    .SetInt32("year", creator.Period.StartDate.Year)
                    .List<object>()
                    .FirstOrDefault()
                    .ToInt(-1);

                if (serialNumber == -1)
                {
                    // так как один раз делается в год, то можно и так
                    creator.PaymentDocDmn.Save(new PaymentDocument
                    {
                        AccountId = -1,
                        PeriodId = -1,
                        Year = creator.Period.StartDate.Year,
                        Number = 1
                    });
                    serialNumber = 1;
                }
                else
                {
                    serialNumber++;
                    currentSession.CreateSQLQuery(updateString + whereString)
                        .SetInt32("docNumber", serialNumber)
                        .SetInt32("year", creator.Period.StartDate.Year)
                        .ExecuteUpdate();
                }
            }

            return serialNumber;
        }
    }
}