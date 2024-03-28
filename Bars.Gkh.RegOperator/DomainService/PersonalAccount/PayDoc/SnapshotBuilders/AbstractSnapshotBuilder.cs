namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments;

    using NHibernate;
    using NHibernate.Util;

    /// <summary>
    /// Абстрактный источник данных для документа на оплату
    /// </summary>
    public abstract class AbstractSnapshotBuilder : ISnapshotBuilder
    {
        /// <summary>
        /// Код источника
        /// </summary>
        public abstract string Code { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Описание заполняемых полей 
        /// </summary>
        public abstract string Description { get; }

        private readonly IDictionary<string, bool> configs = new Dictionary<string, bool>(); 
        private readonly List<IBuilderInfo> builderInfos = new List<IBuilderInfo>();

        /// <summary>
        /// Получение детализированных дочерних источников
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<IBuilderInfo> GetChildren() => this.builderInfos;

        /// <summary>
        /// Инициализация кэша
        /// </summary>
        /// <param name="docCache">Кэш, в котором регистрируются сущности</param>
        /// <param name="mainInfo">Основной запрос</param>
        /// <param name="session">Сессия</param>
        public abstract void InitCache(DocCache docCache, PersonalAccountRecord[] mainInfo, IStatelessSession session);

        /// <summary>
        /// Получение конкретных данных из кэша для последующей работы
        /// </summary>
        /// <param name="creator">Инициатор</param>
        public abstract void WarmCache(SnapshotCreator creator);

        /// <summary>
        /// Заполнение одной записи модели с использованием данных, полученных в WarmCache 
        /// </summary>
        /// <param name="creator">Инициатор</param>
        /// <param name="record">Запись</param>
        /// <param name="account">Информация о лс</param>
        public abstract void FillRecord(SnapshotCreator creator, InvoiceInfo record, PersonalAccountPaymentDocProxy account);

        /// <summary>
        /// Установить настройки
        /// </summary>
        /// <param name="enabledModules"></param>
        public void SetConfigs(IList<string> enabledModules)
        {
            EnumerableExtensions.ForEach(this.builderInfos.Select(x => x.Code), x => this.configs.Add(x, enabledModules.Contains(x)));
        }

        /// <summary>
        /// Включена ли секция
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool SectionEnabled(string key = null)
        {
            var fullKey = key.IsNotEmpty() ? key : this.Code;
            return this.configs.Get(fullKey);
        }

        /// <summary>
        /// Добавить дочерний источник
        /// </summary>
        /// <param name="info">Источник</param>
        protected virtual void AddChildSource(IBuilderInfo info)
        {
            if (this.builderInfos.Any(x => x.Code == info.Code))
            {
                throw new InvalidOperationException();
            }
            this.builderInfos.Add(info);
        }
    }
}