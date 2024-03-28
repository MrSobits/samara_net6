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
    public class ChargesAndPaymentsBuilder : AbstractSnapshotBuilder
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id => nameof(ChargesAndPaymentsBuilder);

        /// <summary>
        /// Код источника
        /// </summary>
        public override string Code => ChargesAndPaymentsBuilder.Id;

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Источник оплат и начислений";

        /// <summary>
        /// Описание заполняемых полей полей
        /// </summary>
        public override string Description => "";

        private readonly IBuilderInfo[] builderInfos =
        {
            new BuilderInfo(
                "ChargesAndPayments",
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
            new BuilderInfo("IsFkrPayment", "Уплачено ФКР", "УплаченоФКР"),
            new BuilderInfo("OldVariables", "Источник со старыми переменными",
                    "ДолгНаКонец, ДолгНаНачало, Итого, Начислено, Пени, НачисленоБазовыйПериод, " +
                    "НачисленоПениПериод, НачисленоСверхБазового, НачисленоТарифРешенияПериод, Оплачено, " +
                    "ПереплатаНаКонец, ПереплатаНаНачало, ПереплатаПениНаКонец, ПереплатаПениНаНачало, " +
                    "Перерасчет, ПерерасчетБазовыйПериод, ПерерасчетПениПериод, ПерерасчетТарифРешенияПериод, " +
                    "СальдоБазТарифНачало, СальдоТарифРешНачало, СуммаВсего")
        };

        private Dictionary<long, PersonalAccountCharge> chargesDict;
        private Dictionary<long, PersonalAccountPeriodSummary> periodSummaryDict;
        private Dictionary<long, CancelSums> cancelChargesDict;
        private Dictionary<string, decimal> socialSupportDict;
        private Dictionary<long, decimal> allSummariesDict;
        private ReportFormatter formatter;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ChargesAndPaymentsBuilder()
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
            if (!this.SectionEnabled("ChargesAndPayments"))
            {
                return;
            }

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

            if (this.SectionEnabled("IsFkrPayment"))
            {
                var periods = session.Query<ChargePeriod>()
                    .Where(x => x.StartDate <= docCache.Period.StartDate)
                    .Select(x => x.Id)
                    .ToArray();

                docCache.Cache.RegisterEntity<PersonalAccountPeriodSummary>()
                    .SetQueryBuilder(r => r.GetAll()
                        .Where(t => periods.Contains(t.Period.Id))
                        .Where(t => docCache.AccountIds.Contains(t.PersonalAccount.Id))
                        .Fetch(t => t.Period));
            }
            else
            {
                docCache.Cache.RegisterEntity<PersonalAccountPeriodSummary>()
                    .SetQueryBuilder(r => r.GetAll()
                        .Where(t => t.Period.Id == docCache.Period.Id)
                        .Where(t => docCache.AccountIds.Contains(t.PersonalAccount.Id))
                        .Fetch(t => t.Period));
            }            
        }

        /// <summary>
        /// Получение конкретных данных из кэша для последующей работы
        /// </summary>
        /// <param name="creator">Инициатор</param>
        public override void WarmCache(SnapshotCreator creator)
        {
            if (!this.SectionEnabled("ChargesAndPayments"))
            {
                return;
            }

            var periodSummaryCache = creator.Cache.GetCache<PersonalAccountPeriodSummary>();

            this.periodSummaryDict = periodSummaryCache.GetEntities()
                .Where(x => x.Period.Id == creator.Period.Id)
                .GroupBy(x => x.PersonalAccount.Id)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault());

            this.chargesDict = creator.Cache.GetCache<PersonalAccountCharge>().GetEntities()
                .ToDictionary(x => x.BasePersonalAccount.Id);

            this.cancelChargesDict = creator.Cache.GetCache<CancelCharge>().GetEntities()
                .GroupBy(x => x.PersonalAccount.Id)
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

            this.socialSupportDict = creator.Cache.GetCache<PersonalAccountPaymentTransfer>().GetEntities()
                .Where(x => x.ChargePeriod.Id == creator.Period.Id)
                .Select(x => new
                {
                    WalletGuid = x.TargetGuid,
                    x.Amount
                })
                .AsEnumerable()
                .GroupBy(x => x.WalletGuid)
                .ToDictionary(x => x.Key, y => y.SafeSum(x => x.Amount));

            if (this.SectionEnabled("IsFkrPayment"))
            {
                this.allSummariesDict = periodSummaryCache.GetEntities()
                    .GroupBy(x => x.PersonalAccount.Id)
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
            if (!this.SectionEnabled("ChargesAndPayments"))
            {
                return;
            }

            if (this.periodSummaryDict.ContainsKey(account.Id) && !creator.isZeroPayment)
            {
                var periodSummary = this.periodSummaryDict[account.Id];
                var charge = this.chargesDict.Get(account.Id);
                var cancelCharge = this.cancelChargesDict.Get(account.Id);

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
                record.ПришлоСоСпецсчета = account.DecisionChargeBalance;
                record.ЗачетСредствБазовый = periodSummary.PerformedWorkChargedBase;
                record.ЗачетСредствТарифРешения = periodSummary.PerformedWorkChargedDecision;

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

                record.СоцПоддержка = this.socialSupportDict.Get(account.SsWalletGuid);

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

                record.ДатаЗакрытияПериода = periodSummary.Period.ObjectEditDate.ToShortDateString();


                if (this.SectionEnabled("IsFkrPayment"))
                {
                     record.УплаченоФКР = Math.Round(
                        Math.Abs(this.allSummariesDict.Get(account.Id)),
                        2,
                        MidpointRounding.AwayFromZero);
                }

                if (this.SectionEnabled("OldVariables"))
                {
                    var startDebt = periodSummary.SaldoIn;

                    record.ПереплатаНаНачало = startDebt < 0 ? -startDebt : 0;
                    record.ПереплатаПениНаНачало = startPenaltyDebt < 0 ? -startPenaltyDebt : 0;
                    record.ДолгНаНачало = startDebt > 0 ? startDebt : 0;
                    record.СальдоТарифРешНачало = record.ДолгТарифРешенияНаНачало;
                    record.СальдоБазТарифНачало = record.ДолгБазовыйНаНачало;

                    record.Начислено = periodSummary.ChargeTariff;
                    record.НачисленоБазовыйПериод = record.НачисленоБазовый;
                    record.НачисленоТарифРешенияПериод = record.НачисленоТарифРешения;
                    record.НачисленоПениПериод = record.НачисленоПени;
                    record.НачисленоСверхБазового = periodSummary.GetChargedByDecisionTariff();

                    record.Перерасчет = periodSummary.RecalcByBaseTariff + periodSummary.RecalcByDecisionTariff;

                    record.ПерерасчетБазовыйПериод = periodSummary.RecalcByBaseTariff
                        + periodSummary.BaseTariffChange
                        + periodSummary.ChargedByBaseTariff
                        - record.НачисленоБазовыйПериод;

                    record.ПерерасчетТарифРешенияПериод = periodSummary.RecalcByDecisionTariff
                        + periodSummary.DecisionTariffChange
                        + periodSummary.ChargeTariff
                        - periodSummary.ChargedByBaseTariff
                        - record.НачисленоТарифРешенияПериод;

                    record.ПерерасчетПениПериод = periodSummary.RecalcByPenalty
                        + periodSummary.PenaltyChange
                        + periodSummary.Penalty
                        - record.НачисленоПениПериод;

                    record.Оплачено = periodSummary.TariffPayment + periodSummary.TariffDecisionPayment - record.СоцПоддержка;

                    var endOverPay = startDebt + record.Начислено + record.Перерасчет - record.СоцПоддержка - record.Оплачено;
                    record.ПереплатаНаКонец = endOverPay < 0 ? -endOverPay : 0;
                    record.ДолгНаКонец = endOverPay > 0 ? endOverPay : 0;

                    var penaltyEndDebt = startPenaltyDebt + record.НачисленоПени - record.ОплаченоПени + record.ПерерасчетПени;
                    record.ПереплатаПениНаКонец = penaltyEndDebt < 0 ? -penaltyEndDebt : 0;
                    record.Пени = record.ДолгПениНаКонец;

                    record.Итого = record.ДолгНаКонец;
                    record.СуммаВсего = record.ДолгНаКонец + record.ДолгПениНаКонец;
                }
            }
        }   
    }

    public class CancelSums
    {
        public decimal BaseTariff;
        public decimal DecisionTariff;
        public decimal Penalty;
        public decimal BaseTariffChange;
        public decimal DecisionTariffChange;
        public decimal PenaltyChange;
    }
}
