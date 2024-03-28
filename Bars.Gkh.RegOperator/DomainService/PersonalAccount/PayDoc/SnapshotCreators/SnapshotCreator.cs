namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.ClaimWork.Debtor;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Domain.Cache;
    using Bars.Gkh.Domain.PaymentDocumentNumber;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.Events;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Enums;
    using Castle.Windsor;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.DomainEvent.Infrastructure;

    /// <summary>
    /// Получение данных для печати квитанций
    /// </summary>
    public class SnapshotCreator
    {
        private static object lockObject = new object();

        /// <summary>
        /// Кэш
        /// </summary>
        internal readonly GkhCache Cache;

        /// <summary>
        /// Кэш для получения актуальных тарифа и площади 
        /// </summary>
        internal readonly ITariffAreaCache TariffAreaCache;

        /// <summary>
        /// Период начислений
        /// </summary>
        internal readonly ChargePeriod Period;

        /// <summary>
        /// Рег оператор
        /// </summary>
        internal Gkh.Modules.RegOperator.Entities.RegOperator.RegOperator RegOperator;

        /// <summary>
        /// Контейнер
        /// </summary>
        protected IWindsorContainer Container;

        /// <summary>
        /// Настройки должников
        /// </summary>
        internal readonly DebtorTypeConfig IndivDebtorConfig;

        /// <summary>
        /// Настройки регопа
        /// </summary>
        internal readonly RegOperatorConfig RegopConfig;

        /// <summary>
        /// Домен квитанций
        /// </summary>
        internal readonly IDomainService<PaymentDocument> PaymentDocDmn;

        /// <summary>
        /// Домен абонентов юрлиц
        /// </summary>
        internal readonly IDomainService<LegalAccountOwner> LegalOwnerDomain;

        /// <summary>
        /// Провайдер сессии
        /// </summary>
        internal ISessionProvider SessionProvider;

        /// <summary>
        /// Непонятная настройка непонятной кнопки, которую давно пора выпилить
        /// </summary>
        internal bool isZeroPayment { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="cache">Кэш</param>
        /// <param name="configProv">Провайдер конфигураций</param>
        /// <param name="period">Период</param>
        /// <param name="isZeroPayment">Бесполезная настройка</param>
        public SnapshotCreator(
            IWindsorContainer container,
            GkhCache cache,
            IGkhConfigProvider configProv,
            ChargePeriod period,
            bool isZeroPayment)
        {
            this.Cache = cache;
            this.Period = period;
            this.Container = container;
            this.isZeroPayment = isZeroPayment;

            this.IndivDebtorConfig = configProv.Get<DebtorClaimWorkConfig>().Individual;
            this.RegopConfig = configProv.Get<RegOperatorConfig>();
            this.PaymentDocDmn = container.ResolveDomain<PaymentDocument>();
            this.LegalOwnerDomain = container.ResolveDomain<LegalAccountOwner>();
            this.TariffAreaCache = container.Resolve<ITariffAreaCache>();
            this.SessionProvider = container.Resolve<ISessionProvider>();
        }

        /// <summary>
        /// Создать слепки
        /// </summary>
        /// <param name="paymentDocumentType">Тип квитанции</param>
        /// <param name="builders">Список источников</param>
        public virtual void CreateSnapshots(
            PaymentDocumentType paymentDocumentType,
            List<ISnapshotBuilder> builders)
        {
            var common = this.GetData(builders);
            this.CreateSnapshots(common, paymentDocumentType, true);
        }

        /// <summary>
        /// Создать слепки
        /// </summary>
        /// <param name="paymentDocumentType">Тип собственника</param>
        /// <param name="builders">Список источников</param>
        public virtual void CreateRegistrySnapshots(
            PaymentDocumentType paymentDocumentType,
            List<ISnapshotBuilder> builders)
        {
            if (paymentDocumentType != PaymentDocumentType.RegistryLegal)
            {
                throw new ArgumentException();
            }

            var ownerHistory = this.Cache.GetCache<AccountOwnershipHistory>()
                .GetEntities()
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.Id)
                .GroupBy(x => x.PersonalAccount.Id)
                .ToDictionary(x => x.Key, y => y.OrderByDescending(z => z.Date).ThenByDescending(x => x.Id).First().AccountOwner.Id);

            //подготавливаем данные для реестра и сгруппированной информации
            var accountsInfo = this.Cache.GetCache<BasePersonalAccount>()
                    .GetEntities()
                    .OrderBy(x => x.Room.RealityObject.Municipality.Name)
                    .ThenBy(x => x.Room.RealityObject.MoSettlement.Return(z => z.Name))
                    .ThenBy(x => x.Room.RealityObject.FiasAddress.PlaceName)
                    .ThenBy(x => x.Room.RealityObject.FiasAddress.StreetName)
                .ThenBy(x => x.Room.RealityObject.FiasAddress.House + x.Room.RealityObject.FiasAddress.Letter, new NumericComparer())
                .Select(
                    x => new TariffAreaRecord
                    {
                        Account = x,
                        OwnerId = ownerHistory.Get(x.Id)
                    })
                .ToList();

            //Нужно получить площадь по каждому лс юр лица, а потом сложить ее. 
            var tariffAreaBuilder = (TariffAreaBuilder)builders.FirstOrDefault(x => x.Code.Equals(TariffAreaBuilder.Id));

            if (tariffAreaBuilder != null)
            {
                builders.Remove(tariffAreaBuilder);

                foreach (var account in accountsInfo)
                {
                    tariffAreaBuilder.SetAreaAndTariff(this, account);
                }
            }

            var common = this.GetRegistryData(builders, accountsInfo);
            this.CreateSnapshots(common, paymentDocumentType, false);

            var accountInfoCreator = new InvoiceAccountInfoSnapshotCreator(this.Container, this.Period, builders);
            accountInfoCreator.CreateSnapshots(accountsInfo);
        }

        /// <summary>
        /// Создать слепки
        /// </summary>
        /// <param name="paymentDocumentType">Тип собственника</param>
        /// <param name="builders">Список источников</param>
        public virtual void CreatePartiallyRegistrySnapshots(
            PaymentDocumentType paymentDocumentType,
            List<ISnapshotBuilder> builders)
        {
            if (paymentDocumentType != PaymentDocumentType.RegistryLegal)
            {
                throw new ArgumentException();
            }

            var invoiceInfo = this.Cache.GetCache<AccountPaymentInfoSnapshot>()
                .GetEntities()
                .Select(x => x.ConvertTo<InvoiceInfo>())
                .FirstOrDefault();

            var accountssnapshotInfo = this.Cache.GetCache<AccountPaymentInfoSnapshot>()
                .GetEntities()
                .Select(x => x.ConvertTo<AccountInfo>())
                .ToList();

            var accountsInfo = this.Cache.GetCache<BasePersonalAccount>()
                .GetEntities()
                .OrderBy(x => x.Room.RealityObject.Municipality.Name)
                .ThenBy(x => x.Room.RealityObject.MoSettlement.Return(z => z.Name))
                .ThenBy(x => x.Room.RealityObject.FiasAddress.PlaceName)
                .ThenBy(x => x.Room.RealityObject.FiasAddress.StreetName)
                .ThenBy(x => x.Room.RealityObject.FiasAddress.House + x.Room.RealityObject.FiasAddress.Letter, new NumericComparer())
                .Select(
                    x => new TariffAreaRecord
                    {
                        Account = x
                    })
                .ToList();

            List<ISnapshotBuilder> mainBuilders = builders
                .Where(x => x.Code.Equals(LegalInfoBuilder.Id) || x.Code.Equals(MainInfoBuilder.Id) || x.Code.Equals(DocNumberBuilder.Id))
                .ToList();

            var common = this.GetPartiallyRegistryData(mainBuilders, accountsInfo, accountssnapshotInfo, invoiceInfo);
            this.CreateSnapshots(common, paymentDocumentType, false, false);

            var accountInfoCreator = new InvoiceAccountInfoSnapshotCreator(this.Container, this.Period, builders);
            accountInfoCreator.CreateSnapshots(accountssnapshotInfo, false);
        }

        /// <summary>
        /// Создание слепков
        /// </summary>
        /// <param name="data">Данные</param>
        /// <param name="paymentDocumentType">Тип документа на оплату</param>
        /// <param name="createSubEvent">createSubEvent</param>
        /// <param name="isBase">Базовый слепок</param>
        protected void CreateSnapshots(
            List<InvoiceInfo> data,
            PaymentDocumentType paymentDocumentType,
            bool createSubEvent,
            bool isBase = true)
        {
            string holderType = paymentDocumentType == PaymentDocumentType.Individual
                ? PaymentDocumentData.AccountHolderType
                : PaymentDocumentData.OwnerholderType;

            using (var paymentDocumentNumberBuilder = new PaymentDocumentNumberBuilder(this.Container))
            {
                foreach (var info in data)
                {
                    var snapshot = new PaymentDocumentSnapshot
                    {
                        Data = JsonConvert.SerializeObject(info, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        }),
                        HolderId = paymentDocumentType == PaymentDocumentType.Individual ? info.AccountId : info.OwnerId,
                        HolderType = holderType,
                        PaymentDocumentType = paymentDocumentType,
                        Address = paymentDocumentType == PaymentDocumentType.RegistryLegal ? info.АдресКонтрагента : info.АдресКвартиры,
                        OwnerType = paymentDocumentType > 0 ? PersonalAccountOwnerType.Legal : PersonalAccountOwnerType.Individual,
                        Municipality = info.Municipality,
                        Settlement = info.Settlement,
                        Payer = paymentDocumentType == PaymentDocumentType.Individual
                            ? info.ФИОСобственника
                            : info.Плательщик,
                        PaymentReceiverAccount = info.РсчетПолучателя,
                        DeliveryAgent = info.АгентДоставки,
                        Period = this.Period,
                        TotalCharge = info.ИтогоКОплате ?? 0,
                        AccountCount = info.КоличествоЛС,
                        IsBase = isBase,
                        HasEmail = YesNo.No
                    };

                    if (paymentDocumentType != PaymentDocumentType.Individual)
                    {
                        snapshot.DocDate = info.ДатаОкончанияПериода.ToDateTime();
                        snapshot.DocNumber = paymentDocumentNumberBuilder.GetDocumentNumber(snapshot);
                        snapshot.OwnerInn = LegalOwnerDomain.Get(info.OwnerId)?.Contragent?.Inn;
                        info.НомерДокумента = snapshot.DocNumber;
                        snapshot.Data = JsonConvert.SerializeObject(info);
                    }

                    DomainEvents.Raise(new SnapshotEvent(snapshot));

                    if (createSubEvent)
                    {
                        // Если зашли сюда, значит сохраняем инфу по ЛС
                        // И добавляем в подчиненный грид информацию по тому же лс

                        var accountInfo = new AccountPaymentInfoSnapshot
                        {
                            AccountId = info.AccountId,
                            AccountNumber = info.ЛицевойСчет,
                            ChargeSum = (info.НачисленоБазовый + info.НачисленоТарифРешения + info.НачисленоПени) ?? 0,
                            PenaltySum = info.НачисленоПени ?? 0,
                            BaseTariffSum = info.НачисленоБазовый ?? 0,
                            DecisionTariffSum = info.НачисленоТарифРешения ?? 0,
                            RoomAddress = info.АдресКвартиры,
                            RoomType = (RoomType)info.RoomType,
                            Area = (float)(info.ОбщаяПлощадь ?? 0),
                            Services = "Взносы за КР",
                            Tariff = info.Тариф ?? 0
                        };
                        DomainEvents.Raise(new AccountSnapshotEvent(
                            accountInfo,
                            snapshot.HolderId,
                            paymentDocumentType == PaymentDocumentType.Individual
                                ? PaymentDocumentData.AccountHolderType
                                : PaymentDocumentData.OwnerholderType));
                    }
                }
            }
        }

        private List<InvoiceInfo> GetData(List<ISnapshotBuilder> builders)
        {
            this.RegOperator = this.Cache.GetCache<Gkh.Modules.RegOperator.Entities.RegOperator.RegOperator>()
                .GetByKey(ContragentState.Active.ToString());

            var accounts = this.Cache.GetCache<BasePersonalAccount>().GetEntities()
                .Select(x => new PersonalAccountPaymentDocProxy(x))
                .ToArray();

            foreach (var builder in builders)
            {
                builder.WarmCache(this);
            }

            var commonData = new List<InvoiceInfo>();

            foreach (var account in accounts)
            {
                var rec = new InvoiceInfo();

                rec.КоличествоЛС = 1;

                foreach (var builder in builders)
                {
                    builder.FillRecord(this, rec, account);
                }

                commonData.Add(rec);
            }

            return commonData;
        }

        private List<InvoiceInfo> GetRegistryData(List<ISnapshotBuilder> builders, List<TariffAreaRecord> accountList)
        {
            this.RegOperator = this.Cache.GetCache<Gkh.Modules.RegOperator.Entities.RegOperator.RegOperator>()
                .GetByKey(ContragentState.Active.ToString());

            var ownerList = accountList
                .Select(x => new
                {
                    x.OwnerId,
                    x.Account,
                    Area = x.RoomArea * x.AreaShare,
                    x.Tariff,
                    x.BaseTariff
                })
                .GroupBy(x => x.OwnerId)
                .ToDictionary(
                    x => x.Key,
                    x => new PersonalAccountPaymentDocProxy(x.First().Account)
                    {              
                        OwnerId = x.First().OwnerId,
                        Area = x.Sum(y => y.Area),
                        Tariff = x.First().Tariff,
                        BaseTariff = x.First().BaseTariff
                    })
                .Select(x => x.Value)
                .ToList();

            foreach (var builder in builders)
            {
                builder.WarmCache(this);
            }

            var commonData = new List<InvoiceInfo>();
            var sumArea = ownerList.Sum(x => x.Area);
            var count = accountList.Count;

            foreach (var owner in ownerList)
            {
                var rec = new InvoiceInfo();
                
                    rec.ОбщаяПлощадь = sumArea;
                    rec.КоличествоЛС = count;
                    rec.Тариф = owner.Tariff;
                    rec.БазовыйТариф = owner.BaseTariff;
              
                foreach (var builder in builders)
                {
                    builder.FillRecord(this, rec, owner);
                }

                commonData.Add(rec);
            }

            return commonData;
        }

        private List<InvoiceInfo> GetPartiallyRegistryData(List<ISnapshotBuilder> builders, List<TariffAreaRecord> accountList, List<AccountInfo> accountInfoList, InvoiceInfo invoiceInfo)
        {
            this.RegOperator = this.Cache.GetCache<Gkh.Modules.RegOperator.Entities.RegOperator.RegOperator>()
                .GetByKey(ContragentState.Active.ToString());

            var ownerInfo = accountList
                .Select(
                    x => new
                    {
                        OwnerId = x.Account.AccountOwner.Id,
                        x.Account
                    })
                .GroupBy(x => x.OwnerId)
                .ToDictionary(
                    x => x.Key,
                    x => new PersonalAccountPaymentDocProxy(x.First().Account))
                .Select(x => x.Value)
                .FirstOrDefault();

            foreach (var builder in builders)
            {
                builder.WarmCache(this);
            }

            var commonData = new List<InvoiceInfo>();

            var rec = invoiceInfo;

            var docNumber = rec.НомерДокумента;

            foreach (var builder in builders)
            {
                builder.FillRecord(this, rec, ownerInfo);
            }

            rec.НомерДокумента = docNumber;
            rec.Тариф = accountInfoList.First().Тариф;
            rec.БазовыйТариф = accountInfoList.First().БазовыйТариф;
            rec.ОбщаяПлощадь = accountInfoList.Sum(x => x.ПлощадьПомещения);
            rec.АдресКвартиры = accountInfoList.First().АдресПомещения;

            rec.НачисленоБазовый = accountInfoList.Sum(x => x.НачисленоБазовый);
            rec.НачисленоТарифРешения = accountInfoList.Sum(x => x.НачисленоТарифРешения);
            rec.НачисленоПени = accountInfoList.Sum(x => x.НачисленоПени);
            rec.ПерерасчетБазовый = accountInfoList.Sum(x => x.ПерерасчетБазовый);
            rec.ПерерасчетТарифРешения = accountInfoList.Sum(x => x.ПерерасчетТарифРешения);
            rec.ПерерасчетПени = accountInfoList.Sum(x => x.ПерерасчетПени);
            rec.ОплаченоБазовый = accountInfoList.Sum(x => x.ОплаченоБазовый);
            rec.ОплаченоТарифРешения = accountInfoList.Sum(x => x.ОплаченоТарифРешения);
            rec.ОплаченоПени = accountInfoList.Sum(x => x.ОплаченоПени);
            rec.ДолгБазовыйНаНачало = accountInfoList.Sum(x => x.ДолгБазовыйНаНачало);
            rec.ДолгБазовыйНаКонец = accountInfoList.Sum(x => x.ДолгБазовыйНаКонец);
            rec.ДолгТарифРешенияНаНачало = accountInfoList.Sum(x => x.ДолгТарифРешенияНаНачало);
            rec.ДолгТарифРешенияНаКонец = accountInfoList.Sum(x => x.ДолгТарифРешенияНаКонец);
            rec.ДолгПениНаНачало = accountInfoList.Sum(x => x.ДолгПениНаНачало);
            rec.ДолгПениНаКонец = accountInfoList.Sum(x => x.ДолгПениНаКонец);
            rec.СоцПоддержка = accountInfoList.Sum(x => x.СоцПоддержка);
            rec.ОтменыБазовый = accountInfoList.Sum(x => x.ОтменыБазовый);
            rec.ОтменыТарифРешения = accountInfoList.Sum(x => x.ОтменыТарифРешения);
            rec.ОтменыПени = accountInfoList.Sum(x => x.ОтменыПени);
            rec.ОтменыКорректировкаБазовый = accountInfoList.Sum(x => x.ОтменыКорректировкаБазовый);
            rec.ОтменыКорректировкаТарифРешения = accountInfoList.Sum(x => x.ОтменыКорректировкаТарифРешения);
            rec.ОтменыКорректировкаПени = accountInfoList.Sum(x => x.ОтменыКорректировкаПени);
            rec.КорректировкаБазовый = accountInfoList.Sum(x => x.КорректировкаБазовый);
            rec.КорректировкаТарифРешения = accountInfoList.Sum(x => x.КорректировкаТарифРешения);
            rec.КорректировкаПени = accountInfoList.Sum(x => x.КорректировкаПени);
            rec.СлияниеБазовый = accountInfoList.Sum(x => x.СлияниеБазовый);
            rec.СлияниеТарифРешения = accountInfoList.Sum(x => x.СлияниеТарифРешения);
            rec.СлияниеПени = accountInfoList.Sum(x => x.СлияниеПени);
            rec.ИтогоПоПени = accountInfoList.Sum(x => x.ИтогоПоПени);
            rec.ИтогоКОплате = accountInfoList.Sum(x => x.ИтогоКОплате);

            rec.КоличествоЛС = accountInfoList.Count;

            commonData.Add(rec);

            return commonData;
        }
    }
}