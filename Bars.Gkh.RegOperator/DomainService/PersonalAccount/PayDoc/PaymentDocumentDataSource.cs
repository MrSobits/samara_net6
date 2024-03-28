namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc
{
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainEvent;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.Events;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.DomainEvent.Infrastructure;

    /// <summary>
    /// Источник данных для документов оплаты
    /// </summary>
    internal class PaymentDocumentDataSource : IDisposable
    {
        private readonly IWindsorContainer container;
        private readonly ChargePeriod period;
        private readonly IEnumerable<PayDocPrimarySource> primarySources;
        private DocCache docCache;
        private PaymentDocumentData paymentData;
        private IEnumerable<PayDocPrimarySource> primarySourcesToCache;
        private readonly PaymentDocumentType paymentDocumentType;
        private readonly List<ISnapshotBuilder> builders = new List<ISnapshotBuilder>();
        private readonly bool IsPartially;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="period">Период за который собираются данные</param>
        /// <param name="primarySources">Информация по счетам и держателям</param>
        /// <param name="paymentDocumentType">Тип квитанции</param>
        public PaymentDocumentDataSource(
            IWindsorContainer container,
            ChargePeriod period,
            IEnumerable<PayDocPrimarySource> primarySources,
            PaymentDocumentType paymentDocumentType,
            bool isPartially = false)
        {
            this.container = container;
            this.period = period;
            this.primarySources = primarySources;
            this.paymentDocumentType = paymentDocumentType;
            this.IsPartially = isPartially;

            this.InitBuilders();//заполнение источников согласно настройкам

            this.PrepareData();

            DomainEvents.Register<SnapshotEvent>(this.StoreItem);

            DomainEvents.Register<AccountSnapshotEvent>(this.StoreSubItem);
        }

        /// <summary>
        /// Данные
        /// </summary>
        public PaymentDocumentData DocumentData
        {
            get { return this.paymentData; }
        }

        /// <summary>
        /// Формирование для юридических лиц (множ.)
        /// </summary>
        /// <param name="isZeroPayment">Разрешить нулевые платежи</param>
        public void CreateRegistrySnapshots(bool isZeroPayment = false)
        {
            if (this.primarySourcesToCache.IsNotEmpty())
            {
                var gkhConfigProvider = this.container.Resolve<IGkhConfigProvider>();
                using (this.container.Using(gkhConfigProvider))
                {
                    var creator = new SnapshotCreator(
                        this.container,
                        this.docCache.GetCache(),
                        gkhConfigProvider,
                        this.period,
                        isZeroPayment);
                    creator.CreateRegistrySnapshots(this.paymentDocumentType, this.builders);
                }
            }
        }

        /// <summary>
        /// Формирование для юридических лиц по отдельным помещениям
        /// </summary>
        /// <param name="isZeroPayment">Разрешить нулевые платежи</param>
        public void CreatePartiallyRegistrySnapshots(bool isZeroPayment = false)
        {
            var gkhConfigProvider = this.container.Resolve<IGkhConfigProvider>();
            using (this.container.Using(gkhConfigProvider))
            {
                var creator = new SnapshotCreator(
                    this.container,
                    this.docCache.GetCache(),
                    gkhConfigProvider,
                    this.period,
                    isZeroPayment);
                creator.CreatePartiallyRegistrySnapshots(this.paymentDocumentType, this.builders);
            }

        }

        /// <summary>
        /// Создать слепки
        /// </summary>
        public void CreateSnapshots(bool isZeroPayment = false)
        {
            if (this.primarySourcesToCache.IsNotEmpty())
            {
                var gkhConfigProvider = this.container.Resolve<IGkhConfigProvider>();
                using (this.container.Using(gkhConfigProvider))
                {
                    var creator = new SnapshotCreator(
                        this.container,
                        this.docCache.GetCache(),
                        gkhConfigProvider,
                        this.period,
                        isZeroPayment);

                    creator.CreateSnapshots(this.paymentDocumentType, this.builders);
                }
            }
        }

        /// <summary>
        /// Сохранение данных
        /// </summary>
        public void Save()
        {
            this.paymentData.SaveChanges();
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            if (this.docCache != null)
            {
                this.docCache.Dispose();
            }

            DomainEvents.ClearCallbacks();
        }

        private void PrepareData()
        {
            this.paymentData = new PaymentDocumentData(this.container, this.period, this.primarySources);

            var physicalInfo = this.paymentData.GetAccountData();
            var ownerInfo = this.paymentData.GetOwnerData();
            
            var snapshotedAccounts = physicalInfo.Select(x => x.HolderId)
                .Union(ownerInfo.SelectMany(x => x.Accounts).Select(x => x.AccountId))
                .Distinct();

            this.primarySourcesToCache = this.primarySources.Where(
                x => !snapshotedAccounts.Contains(x.AccountId) || this.IsPartially);

            this.docCache = new DocCache(this.container, this.period, this.primarySourcesToCache, this.builders);
        }

        private void InitBuilders()
        {
            var configService = this.container.Resolve<ISourceConfigService>();

            try
            {
                var configList = configService.GetConfigList(this.paymentDocumentType);

                foreach (var builder in configService.GetSourceList(this.paymentDocumentType))
                {
                    if (configList.Contains(builder.Code) || configList.Any(y => builder.GetChildren().Select(x => x.Code).Contains(y)))
                    {
                        this.builders.Add(builder);
                        builder.SetConfigs(configList);
                    }
                }
            }
            finally
            {
                this.container.Release(configService);
            }
        }

        private void StoreItem(SnapshotEvent evt)
        {
            this.paymentData.AddItem(evt.Snapshot);
        }

        private void StoreSubItem(AccountSnapshotEvent evt)
        {
            this.paymentData.AddSubItem(evt.Snapshot, evt.SnapshotParentId, evt.SnapshotType, evt.IsBase);
        }
    }

    /// <summary>
    /// По каким данным печатать квитанцию
    /// </summary>
    [Serializable]
    public class PayDocPrimarySource
    {
        public long AccountId;
        public long OwnerId;

        public PayDocPrimarySource(long accountId, long ownerId)
        {
            this.AccountId = accountId;
            this.OwnerId = ownerId;
        }
    }
}