namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments;

    using NHibernate;
    using NHibernate.Linq;

    /// <summary>
    /// Источник оплат и начислений
    /// </summary>
    public class GroupedChargesAndPaymentsBuilder : AbstractSnapshotBuilder
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id => nameof(GroupedChargesAndPaymentsBuilder);

        /// <summary>
        /// Код источника
        /// </summary>
        public override string Code => GroupedChargesAndPaymentsBuilder.Id;

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Источник оплат и начислений для реестра юр.лиц";

        /// <summary>
        /// Описание заполняемых полей полей
        /// </summary>
        public override string Description => "";

        private readonly IBuilderInfo[] builderInfos =
        {
            new BuilderInfo(
                "GroupedChargesAndPayments",
                "Оплаты и начисления",
                "НачисленоБазовый; НачисленоТарифРешения; НачисленоПени; " +
                    "ПерерасчетБазовый; ПерерасчетТарифРешения; ПерерасчетПени; " +
                    "ОтменыБазовый; ОтменыТарифРешения; ОтменыПени; " +
                    "ОтменыКорректировкаБазовый; ОтменыКорректировкаТарифРешения; ОтменыКорректировкаПени; " +
                    "КорректировкаБазовый; КорректировкаТарифРешения; КорректировкаПени; " +
                    "СлияниеБазовый; СлияниеТарифРешения; СлияниеПени; " +
                    "ОплаченоБазовый; ОплаченоТарифРешения; ОплаченоПени; " +
                    "ЗачетСредствБазовый; ЗачетСредствТарифРешения; " +
                    "ДолгБазовыйНаНачало; ДолгТарифРешенияНаНачало; ДолгПениНаНачало; " +
                    "ДолгБазовыйНаКонец; ДолгТарифРешенияНаКонец; ДолгПениНаКонец; СоцПоддержка; " +
                    "ИтогоПоТарифу; ИтогоПоПени; ИтогоКОплате; СуммаСтрокой"),
            new BuilderInfo("GroupedIsFkrPayment", "Уплачено ФКР", "УплаченоФКР"),
            new BuilderInfo("OldVariables", "Источник со старыми переменными",
                    "ДолгНаКонец, Итого, Пени, НачисленоБазовыйПериод, НачисленоВсего, НачисленоПениВсего, " +
                    "НачисленоПениПериод, НачисленоТарифРешенияПериод, ОбщийДебет, ОбщийДебетПени, ОбщийКредит, " +
                    "ОбщийКредитПени, Перерасчет, ПерерасчетБазовыйПериод, ПерерасчетВсего, ПерерасчетПениПериод, " +
                    "ПерерасчетТарифРешенияПериод, СальдоБазТарифНачало, СальдоТарифРешНачало, СуммаВсего")
        };

        private Dictionary<long, long> accountOwnershipDict;
        private Dictionary<long, ChargeProxy> chargesDict;
        private Dictionary<long, PeriodSummaryProxy> periodSummaryDict;
        private Dictionary<long, CancelSums> cancelChargesDict;
        private Dictionary<long, decimal> socialSupportDict;
        private Dictionary<long, decimal> allSummariesDict;
        private ReportFormatter formatter;

        public GroupedChargesAndPaymentsBuilder()
        {
            this.builderInfos.ForEach(this.AddChildSource);
        }

        /// <summary>
        /// Инициализация кэша
        /// </summary>
        /// <param name="docCache">Кэш, в котором регистрируются сущности</param>
        /// <param name="mainInfo">Основной запрос</param>
        /// <param name="session">Сессия</param>
        public override void InitCache(DocCache docCache, PersonalAccountRecord[] mainInfo, IStatelessSession session)
        {
            if (!this.SectionEnabled("GroupedChargesAndPayments"))
            {
                return;
            }

            this.accountOwnershipDict = docCache.AccountOwnershipDict;

            docCache.Cache.RegisterEntity<PersonalAccountCharge>()
                .SetQueryBuilder(r => r.GetAll()
                    .Fetch(x => x.ChargePeriod)
                    .Where(x => x.ChargePeriod.Id == docCache.Period.Id && x.IsActive) // т.к. печать квитанций возможна и по открытому периоду, то смотрим на этот параметр
                    .Where(z => docCache.AccountIds.Contains(z.BasePersonalAccount.Id)));

            docCache.Cache.RegisterEntity<CancelCharge>()
                .SetQueryBuilder(r => r.GetAll()
                    .Fetch(z => z.ChargeOperation)
                    .Fetch(z => z.PersonalAccount)
                    .Where(z => z.ChargeOperation.Period.Id == docCache.Period.Id)
                    .Where(z => docCache.AccountIds.Contains(z.PersonalAccount.Id)));

            //получаем трансферы соц поддержки
            var walletQuery = session.Query<BasePersonalAccount>()
                .WhereContains(x => x.Id, docCache.AccountIds)
                .Select(x => x.SocialSupportWallet.WalletGuid);

            docCache.Cache.RegisterEntity<PersonalAccountPaymentTransfer>()
                .SetQueryBuilder(r => r.GetAll()
                    .WhereContains(z => z.Owner.Id, docCache.AccountIds)
                    .Where(z => walletQuery.Any(x => x == z.TargetGuid))
                    // todo проверить условия такого типа
                    // .Where(z => z.Owner.SocialSupportWallet.WalletGuid == z.TargetGuid)
                    .Where(z => !z.Operation.IsCancelled)
                    .Where(z => z.ChargePeriod.Id == docCache.Period.Id));

            if (this.SectionEnabled("GroupedIsFkrPayment"))
            {
                var periods = session.Query<ChargePeriod>()
                    .Where(x => x.StartDate <= docCache.Period.StartDate)
                    .Select(x => x.Id)
                    .ToArray();

                docCache.Cache.RegisterEntity<PersonalAccountPeriodSummary>()
                    .SetQueryBuilder(r => r.GetAll()
                        .Where(t => periods.Contains(t.Period.Id))
                        .Where(t => docCache.AccountIds.Contains(t.PersonalAccount.Id)));
            }
            else
            {
                docCache.Cache.RegisterEntity<PersonalAccountPeriodSummary>()
                    .SetQueryBuilder(r => r.GetAll()
                        .Where(t => t.Period.Id == docCache.Period.Id)
                        .Where(t => docCache.AccountIds.Contains(t.PersonalAccount.Id)));
            }            
        }

        /// <summary>
        /// Получение конкретных данных из кэша для последующей работы
        /// </summary>
        /// <param name="creator">Инициатор</param>
        public override void WarmCache(SnapshotCreator creator)
        {
            if (!this.SectionEnabled("GroupedChargesAndPayments"))
            {
                return;
            }
            
            this.chargesDict = creator.Cache.GetCache<PersonalAccountCharge>().GetEntities()
                .GroupBy(x => this.accountOwnershipDict[x.BasePersonalAccount.Id])
                .ToDictionary(
                    x => x.Key,
                    y => new ChargeProxy
                    {
                        ChargeTariff = y.Sum(x => x.ChargeTariff),
                        OverPlus = y.Sum(x => x.OverPlus),
                        Penalty = y.Sum(x => x.Penalty)
                    });

            var periodSummaryCache = creator.Cache.GetCache<PersonalAccountPeriodSummary>();

            this.periodSummaryDict = periodSummaryCache.GetEntities()
                .Where(x => x.Period.Id == creator.Period.Id)
                .GroupBy(x => this.accountOwnershipDict[x.PersonalAccount.Id])
                .ToDictionary(
                    x => x.Key, 
                    y => new PeriodSummaryProxy
                    {
                        ChargeTariff = y.Sum(x => x.ChargeTariff),
                        ChargedByBaseTariff = y.Sum(x => x.ChargedByBaseTariff),
                        Penalty = y.Sum(x => x.Penalty),
                        RecalcByBaseTariff = y.Sum(x => x.RecalcByBaseTariff),
                        RecalcByDecisionTariff = y.Sum(x => x.RecalcByDecisionTariff),
                        RecalcByPenalty = y.Sum(x => x.RecalcByPenalty),
                        BaseTariffChange = y.Sum(x => x.BaseTariffChange),
                        DecisionTariffChange = y.Sum(x => x.DecisionTariffChange),
                        PenaltyChange = y.Sum(x => x.PenaltyChange),
                        TariffPayment = y.Sum(x => x.TariffPayment),
                        TariffDecisionPayment = y.Sum(x => x.TariffDecisionPayment),
                        PenaltyPayment = y.Sum(x => x.PenaltyPayment),
                        BaseTariffDebt = y.Sum(x => x.BaseTariffDebt),
                        DecisionTariffDebt = y.Sum(x => x.DecisionTariffDebt),
                        PenaltyDebt = y.Sum(x => x.PenaltyDebt),
                        PerformedWorkBaseTariff = y.Sum(x => x.PerformedWorkChargedBase),
                        PerformedWorkDecisionTariff = y.Sum(x => x.PerformedWorkChargedDecision),
                        PenaltyDebet = y.Where(x => x.Penalty > 0).Sum(x => x.Penalty),
                        SaldoOutDebet = y.Where(x => x.SaldoOut > 0).Sum(x => x.SaldoOut),
                        PenaltyKredit = y.Where(x => x.Penalty < 0).Sum(x => x.Penalty),
                        SaldoOutKredit = y.Where(x => x.SaldoOut < 0).Sum(x => x.SaldoOut)
                    });

            this.cancelChargesDict = creator.Cache.GetCache<CancelCharge>().GetEntities()
                .GroupBy(x => this.accountOwnershipDict[x.PersonalAccount.Id])
                .ToDictionary(x => x.Key, x =>
                    new CancelSums
                    {
                        BaseTariff = x.Where(y => y.CancelType == CancelType.BaseTariffCharge).SafeSum(y => y.CancelSum),
                        DecisionTariff = x.Where(y => y.CancelType == CancelType.DecisionTariffCharge).SafeSum(y => y.CancelSum),
                        Penalty = x.Where(y => y.CancelType == CancelType.Penalty).SafeSum(y => y.CancelSum),
                        BaseTariffChange = x.Where(y => y.CancelType == CancelType.BaseTariffChange).SafeSum(y => y.CancelSum),
                        DecisionTariffChange = x.Where(y => y.CancelType == CancelType.DecisionTariffChange).SafeSum(y => y.CancelSum),
                        PenaltyChange = x.Where(y => y.CancelType == CancelType.PenaltyChange).SafeSum(y => y.CancelSum)
                    });

            var walletData = creator.Cache.GetCache<BasePersonalAccount>()
                .GetEntities()
                .Select(x => new
                {
                    OwnerId = this.accountOwnershipDict[x.Id],
                    SocSupportWallet = x.SocialSupportWallet.WalletGuid
                })
                .ToDictionary(x => x.SocSupportWallet);

            this.socialSupportDict = creator.Cache.GetCache<PersonalAccountPaymentTransfer>().GetEntities()
                .Where(x => x.ChargePeriod.Id == creator.Period.Id)
                .Select(x => new
                {
                    WalletGuid = x.TargetGuid,
                    x.Amount
                })
                .AsEnumerable()
                .GroupBy(x => walletData[x.WalletGuid].OwnerId)
                .ToDictionary(x => x.Key, y => y.SafeSum(x => x.Amount));

            if (this.SectionEnabled("GroupedIsFkrPayment"))
            {
                this.allSummariesDict = periodSummaryCache.GetEntities()
                    .GroupBy(x => this.accountOwnershipDict[x.PersonalAccount.Id])
                    .ToDictionary(x => x.Key, y => y.Sum(z => z.TariffPayment + z.TariffDecisionPayment));
            }

            this.formatter = new ReportFormatter();
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
            if (!this.SectionEnabled("GroupedChargesAndPayments"))
            {
                return;
            }

            var ownerId = this.accountOwnershipDict.Get(account.Id);

            if (this.periodSummaryDict.ContainsKey(ownerId) && !creator.isZeroPayment)
            {
                var periodSummary = this.periodSummaryDict[ownerId];
                var charge = this.chargesDict.Get(ownerId);
                var cancelCharge = this.cancelChargesDict.Get(ownerId);

                // т.к. теперь в НачисленоXxxxxПериод сидят только начисления, а в перерасчётах всё остальное
                // нам нужно извлечь следующие операции:
                // 1. Перерасчет                            - сидит в перерасчете
                // 2. Перенос долга при слиянии             - сидит в начислено (или оплачено)
                // 3. Установка и изменение сальдо          - сидит в изменении баланса
                // 4. Зачет средств за выполненные работы   - сидит в изменении баланса
                // 5. Отмена начислений                     - сидит в начислено

                var chargeDecision = periodSummary.ChargeTariff - periodSummary.ChargedByBaseTariff;

                record.НачисленоБазовый = charge.ReturnSafe(z => z.ChargeTariff - z.OverPlus);
                record.НачисленоТарифРешения = charge.ReturnSafe(z => z.OverPlus);
                record.НачисленоПени = charge.ReturnSafe(z => z.Penalty);

                record.ПерерасчетБазовый = periodSummary.RecalcByBaseTariff;
                record.ПерерасчетТарифРешения = periodSummary.RecalcByDecisionTariff;
                record.ПерерасчетПени = periodSummary.RecalcByPenalty;

                record.ЗачетСредствБазовый = periodSummary.PerformedWorkBaseTariff;
                record.ЗачетСредствТарифРешения = periodSummary.PerformedWorkDecisionTariff;

                record.ОтменыБазовый = cancelCharge.ReturnSafe(z => z.BaseTariff);
                record.ОтменыТарифРешения = cancelCharge.ReturnSafe(z => z.DecisionTariff);
                record.ОтменыПени = cancelCharge.ReturnSafe(z => z.Penalty);

                record.ОтменыКорректировкаБазовый = cancelCharge.ReturnSafe(z => z.BaseTariffChange);
                record.ОтменыКорректировкаТарифРешения = cancelCharge.ReturnSafe(z => z.DecisionTariffChange);
                record.ОтменыКорректировкаПени = cancelCharge.ReturnSafe(z => z.PenaltyChange);

                //BaseTariffChange = корректировки - отмены корректировок
                record.КорректировкаБазовый = periodSummary.BaseTariffChange + record.ОтменыКорректировкаБазовый;
                record.КорректировкаТарифРешения = periodSummary.DecisionTariffChange + record.ОтменыКорректировкаТарифРешения;
                record.КорректировкаПени = periodSummary.PenaltyChange + record.ОтменыКорректировкаПени;

                //ChargedByBaseTariff = чистые начисления - отмены + перенос долга при слиянии
                record.СлияниеБазовый = periodSummary.ChargedByBaseTariff - record.НачисленоБазовый + record.ОтменыБазовый;
                record.СлияниеТарифРешения = chargeDecision - record.НачисленоТарифРешения + record.ОтменыТарифРешения;
                record.СлияниеПени = periodSummary.Penalty - record.НачисленоПени + record.ОтменыПени;

                //с учетом соц поддержки
                record.ОплаченоБазовый = periodSummary.TariffPayment;
                record.ОплаченоТарифРешения = periodSummary.TariffDecisionPayment;
                record.ОплаченоПени = periodSummary.PenaltyPayment;

                var startBaseTariffDebt = periodSummary.BaseTariffDebt;
                var startDecisionTariffDebt = periodSummary.DecisionTariffDebt;
                var startPenaltyDebt = periodSummary.PenaltyDebt;

                record.ДолгБазовыйНаНачало = startBaseTariffDebt;
                record.ДолгТарифРешенияНаНачало = startDecisionTariffDebt;
                record.ДолгПениНаНачало = startPenaltyDebt;

                //самая полная задолженность на конец, включает в себя все.
                //Долг+Начислено(с отменами)+перерасчет-оплачено+ручные корректировки(c отменами)
                record.ДолгБазовыйНаКонец = startBaseTariffDebt
                    + periodSummary.ChargedByBaseTariff
                    + periodSummary.BaseTariffChange
                    + record.ПерерасчетБазовый
                    - record.ОплаченоБазовый;

                record.ДолгТарифРешенияНаКонец = startDecisionTariffDebt
                    + chargeDecision
                    + periodSummary.DecisionTariffChange
                    + record.ПерерасчетТарифРешения
                    - record.ОплаченоТарифРешения;

                record.ДолгПениНаКонец = startPenaltyDebt
                    + periodSummary.Penalty
                    + periodSummary.PenaltyChange
                    + record.ПерерасчетПени
                    - record.ОплаченоПени;

                record.СоцПоддержка = this.socialSupportDict.Get(ownerId);

                record.ИтогоПоТарифу = record.НачисленоБазовый +
                    record.НачисленоТарифРешения +
                    record.ПерерасчетБазовый +
                    record.ПерерасчетТарифРешения;

                record.ИтогоПоПени = record.НачисленоПени + record.ПерерасчетПени;

                record.ИтогоКОплате = record.ИтогоПоТарифу + record.ИтогоПоПени;

                record.СуммаСтрокой = this.formatter.Format(
                    "СуммаСтрокой",
                    record.ИтогоКОплате,
                    CultureInfo.InvariantCulture.NumberFormat);

                if (this.SectionEnabled("GroupedIsFkrPayment"))
                {
                     record.УплаченоФКР = Math.Round(
                        Math.Abs(this.allSummariesDict.Get(ownerId)),
                        2,
                        MidpointRounding.AwayFromZero);
                }

                if (this.SectionEnabled("OldVariables"))
                {
                    record.ДолгНаКонец = periodSummary.SaldoOutDebet + periodSummary.SaldoOutKredit;
                    record.Итого = periodSummary.ChargeTariff;
                    record.Пени = periodSummary.Penalty;
                    record.Перерасчет = record.ПерерасчетБазовый;

                    record.СуммаВсего = record.Итого + record.Пени + record.Перерасчет;
                    record.СальдоБазТарифНачало = periodSummary.BaseTariffDebt;
                    record.СальдоТарифРешНачало = periodSummary.DecisionTariffDebt;

                    record.НачисленоБазовыйПериод = record.НачисленоБазовый;
                    record.НачисленоПениПериод = record.НачисленоПени;
                    record.НачисленоТарифРешенияПериод = record.НачисленоТарифРешения;

                    record.ПерерасчетБазовыйПериод = periodSummary.RecalcByBaseTariff
                                    + periodSummary.BaseTariffChange
                                    + periodSummary.ChargedByBaseTariff
                                    - record.НачисленоБазовыйПериод;

                    record.ПерерасчетПениПериод = periodSummary.RecalcByPenalty
                                                        + periodSummary.PenaltyChange
                                                        + periodSummary.Penalty
                                                        - record.НачисленоПениПериод;

                    record.ПерерасчетТарифРешенияПериод = periodSummary.RecalcByDecisionTariff
                        + periodSummary.DecisionTariffChange
                        + periodSummary.ChargeTariff
                        - periodSummary.ChargedByBaseTariff
                        - record.НачисленоТарифРешенияПериод;
            
                    record.НачисленоВсего = periodSummary.ChargeTariff;
                    record.НачисленоПениВсего = periodSummary.Penalty;
                    record.ПерерасчетВсего = record.ПерерасчетБазовый + record.ПерерасчетТарифРешения + record.ПерерасчетПени;

                    record.ОбщийДебет = periodSummary.SaldoOutDebet;
                    record.ОбщийДебетПени = periodSummary.PenaltyDebet;
                    record.ОбщийКредит = periodSummary.SaldoOutKredit;
                    record.ОбщийКредитПени = periodSummary.PenaltyKredit;
                }
            }
        }   
    }
    public class ChargeProxy
    {
        public decimal ChargeTariff;
        public decimal OverPlus;
        public decimal Penalty;
    }

    public class PeriodSummaryProxy
    {
        public decimal ChargeTariff;
        public decimal ChargedByBaseTariff;
        public decimal Penalty;

        public decimal RecalcByBaseTariff;
        public decimal RecalcByDecisionTariff;
        public decimal RecalcByPenalty;
        public decimal BaseTariffChange;
        public decimal DecisionTariffChange;
        public decimal PenaltyChange;

        public decimal TariffPayment;
        public decimal TariffDecisionPayment;
        public decimal PenaltyPayment;

        public decimal BaseTariffDebt ;
        public decimal DecisionTariffDebt;
        public decimal PenaltyDebt;

        public decimal PerformedWorkBaseTariff;
        public decimal PerformedWorkDecisionTariff;

        public decimal PenaltyDebet;
        public decimal SaldoOutDebet;
        public decimal PenaltyKredit;
        public decimal SaldoOutKredit;
    }
}