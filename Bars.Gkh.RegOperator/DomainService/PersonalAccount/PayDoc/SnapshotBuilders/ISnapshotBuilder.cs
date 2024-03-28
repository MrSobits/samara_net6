namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders
{
    using System.Collections.Generic;

    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments;

    using NHibernate;

    /// <summary>
    /// Интерфейс источника данных документа на оплату
    /// </summary>
    public interface ISnapshotBuilder : IBuilderInfo
    {
        /// <summary>
        /// Инициализация кэша
        /// </summary>
        /// <param name="docCache">Кэш, в котором регистрируются сущности</param>
        /// <param name="mainInfo">Основной запрос</param>
        /// <param name="session">Сессия</param>
        void InitCache(DocCache docCache, PersonalAccountRecord[] mainInfo, IStatelessSession session);

        /// <summary>
        /// Получение конкретных данных из кэша для последующей работы
        /// </summary>
        /// <param name="creator">Инициатор</param>
        void WarmCache(SnapshotCreator creator);

        /// <summary>
        /// Заполнение одной записи модели с использованием данных, полученных в WarmCache 
        /// </summary>
        /// <param name="creator">Инициатор</param>
        /// <param name="record">Запись</param>
        /// <param name="account">Информация о лс</param>
        void FillRecord(
            SnapshotCreator creator,
            InvoiceInfo record, 
            PersonalAccountPaymentDocProxy account);

        /// <summary>
        /// Установить поднастройки
        /// </summary>
        /// <param name="enabledModules">Имена настроек</param>
        void SetConfigs(IList<string> enabledModules);

        /// <summary>
        /// Включена ли секция
        /// </summary>
        /// <param name="key">Ключ секции</param>
        bool SectionEnabled(string key = null);
    } 
}