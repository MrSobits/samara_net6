namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders
{
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments;

    using NHibernate;

    /// <summary>
    /// Источник по тарифу и площади
    /// </summary>
    public class TariffAreaBuilder : AbstractSnapshotBuilder
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id => nameof(TariffAreaBuilder);

        /// <summary>
        /// Код источника
        /// </summary>
        public override string Code => TariffAreaBuilder.Id;

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Источник по тарифу и площади";

        /// <summary>
        /// Описание заполняемых полей 
        /// </summary>
        public override string Description => "ОбщаяПлощадь; Тариф; БазовыйТариф";

        /// <summary>
        /// Инициализация кэша
        /// </summary>
        /// <param name="docCache">Кэш, в котором регистрируются сущности</param>
        /// <param name="mainInfo">Основной запрос</param>
        /// <param name="session">Сессия</param>
        public override void InitCache(DocCache docCache, PersonalAccountRecord[] mainInfo, IStatelessSession session)
        {
            docCache.TariffAreaCache.Init(mainInfo, docCache.Period);
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
            if (!creator.isZeroPayment)
            {
                var tariffAreaRecord = creator.TariffAreaCache.GetTariffArea(account.Account, creator.Period);
                record.ОбщаяПлощадь = tariffAreaRecord.AreaShare * tariffAreaRecord.RoomArea;
                record.Тариф = tariffAreaRecord.Tariff;
                record.БазовыйТариф = tariffAreaRecord.BaseTariff;
                //для Смоленска Для правильной печати площади на долевых ЛС
                record.ПлощадьПомещения = tariffAreaRecord.RoomArea;
                record.ДоляСобственности = tariffAreaRecord.AreaShare;
            }
        }

        /// <summary>
        /// Установить в запись ее актуальную площадь и тариф
        /// </summary>
        /// <param name="creator">Инициатор</param>
        /// <param name="record">Запись</param>
        public void SetAreaAndTariff(SnapshotCreator creator, TariffAreaRecord record)
        {
            var tariffAreaRecord = creator.TariffAreaCache.GetTariffArea(record.Account, creator.Period);

            record.AreaShare = tariffAreaRecord.AreaShare;
            record.BaseTariff = tariffAreaRecord.BaseTariff;
            record.RoomArea = tariffAreaRecord.RoomArea;
            record.Tariff = tariffAreaRecord.Tariff;
        }
    }
}