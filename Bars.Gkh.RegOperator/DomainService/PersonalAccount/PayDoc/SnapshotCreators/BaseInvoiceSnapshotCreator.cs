namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.ClaimWork.Debtor;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Domain.Cache;
    using Bars.Gkh.Domain.PaymentDocumentNumber;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Domain.Interfaces;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.DomainEvent;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Enums;
    using Castle.Windsor;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders;
    using Bars.Gkh.RegOperator.Domain;

    /// <summary>
    /// Получение данных для печати квитанций по физ лицам
    /// </summary>
    /// <typeparam name="TModel">Класс источник данных для печати</typeparam>activeNotifAccNum
    [System.Obsolete("use class SnapshotCreator")]
    public class BaseInvoiceSnapshotCreator<TModel>
        where TModel : InvoiceInfo, new()
    {
        /* private static object lockObject = new object();
         public readonly GkhCache Cache;
         public readonly IParameterTracker ParamTracker;
         public readonly ChargePeriod Period;
         public readonly ICalculatedParameterCache TraceCache;

         protected Dictionary<long, InvoiceBankProxy> Banks;
         public Gkh.Modules.RegOperator.Entities.RegOperator.RegOperator RegOperator;

         protected HashSet<long> RoWithCustomDecisionType;
         protected IWindsorContainer Container;

         public readonly DebtorTypeConfig indivDebtorConfig;
         public readonly RegOperatorConfig regopConfig;

         public readonly IDomainService<PaymentDocument> paymentDocDmn;

         private Dictionary<long, DebtorClaimWork> debtorClaimWorks;
         private Dictionary<long, string> delAgentByRealObj;
         private Dictionary<long, ManagingOrganization> manOrgByRealObjDict;
         //private List<PaymentDocInfo> paymentDocInfoList;
         private HashSet<long> roWithCrDunDecision;
         private HashSet<long> roWithRegopDecision;
         private Dictionary<string, decimal> socialSupportDict;
         public ISessionProvider sessionProvider;

         public bool isZeroPayment;

         public BaseInvoiceSnapshotCreator(
             IWindsorContainer container,
             GkhCache cache,
             IGkhConfigProvider configProv,
             ChargePeriod period)
         {
             this.Cache = cache;
             this.Period = period;
             this.Container = container;

             this.indivDebtorConfig = configProv.Get<DebtorClaimWorkConfig>().Individual;
             this.regopConfig = configProv.Get<RegOperatorConfig>();
             this.ParamTracker = container.Resolve<IParameterTracker>();
             this.paymentDocDmn = container.ResolveDomain<PaymentDocument>();
             this.TraceCache = container.Resolve<ICalculatedParameterCache>();
             this.sessionProvider = container.Resolve<ISessionProvider>();
         }

         public virtual void CreateSnapshots(bool isZeroPayment, PersonalAccountOwnerType personalAccountOwnerType)
         {
             var indivAccOwners = this.Cache.GetCache<IndividualAccountOwner>();
             var accountOwnersDict = indivAccOwners.GetEntities()
                 .Select(x => new
                 {
                     x.Id,
                     x.Surname,
                     x.FirstName,
                     x.SecondName
                 })
                 .GroupBy(x => x.Id)
                 .ToDictionary(x => x.Key, x => x.First());

             var depersonalized =
                 this.regopConfig.PaymentDocumentConfigContainer.PaymentDocumentConfigIndividual.PaymentDocFormat ==
                 PaymentDocumentFormat.Depersonalized;

             var common = this.GetCommonData(
                 isZeroPayment,
                 record =>
                 {
                     if (!accountOwnersDict.ContainsKey(record.OwnerId) || depersonalized)
                     {
                         return;
                     }

                     var accountOwner = accountOwnersDict[record.OwnerId];

                     record.ФИОСобственника = "{0} {1} {2}".FormatUsing(accountOwner.Surname, accountOwner.FirstName, accountOwner.SecondName);
                     record.ФамилияCобственника = accountOwner.Surname;
                     record.ИмяCобственника = accountOwner.FirstName;
                     record.ОтчествоCобственника = accountOwner.SecondName;
                 });

             this.CreateSnapshots(common, PaymentDocumentData.AccountHolderType, true, personalAccountOwnerType);
         }

         protected void CreateSnapshots(List<TModel> data, string holderType, bool createSubEvent, PersonalAccountOwnerType personalAccountOwnerType)
         {
             using (var paymentDocumentNumberBuilder = new PaymentDocumentNumberBuilder(this.Container))
             {
             foreach (var info in data)
             {
                 var snapshot = new PaymentDocumentSnapshot
                 {
                     Data = JsonConvert.SerializeObject(info),
                     HolderId = personalAccountOwnerType == PersonalAccountOwnerType.Legal ? info.OwnerId : info.AccountId,
                     HolderType = holderType,
                     Address = info.АдресКвартиры,
                     OwnerType = (PersonalAccountOwnerType)info.OwnerType,
                     Municipality = info.Municipality,
                     Settlement = info.Settlement,
                     Payer = personalAccountOwnerType == PersonalAccountOwnerType.Legal
                         ? info.Плательщик
                         : info.ФИОСобственника,
                     PaymentReceiverAccount = info.РсчетПолучателя,
                     Period = this.Period,
                     //TotalCharge = info.Начислено
                 };

                 if (personalAccountOwnerType == PersonalAccountOwnerType.Legal)
                 {
                     snapshot.DocDate = info.ДатаОкончанияПериода.ToDateTime();
                         snapshot.DocNumber = paymentDocumentNumberBuilder.GetDocumentNumber(snapshot);
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
                         //ChargeSum = info.Начислено,
                         PenaltySum = info.НачисленоПени,
                         BaseTariffSum = info.НачисленоБазовый,
                         //DecisionTariffSum = info.НачисленоСверхБазового,
                         RoomAddress = info.АдресКвартиры,
                         RoomType = (RoomType) info.RoomType,
                         Area = (float) info.ОбщаяПлощадь,
                         Services = "Взносы за КР",
                         Tariff = info.Тариф
                     };
                     DomainEvents.Raise(new AccountSnapshotEvent(
                         accountInfo, 
                         snapshot.HolderId,
                         personalAccountOwnerType == PersonalAccountOwnerType.Legal
                             ? PaymentDocumentData.OwnerholderType
                             : PaymentDocumentData.AccountHolderType));
                 }
             }
         }
         }

         protected List<TModel> GetCommonData(bool isZeroPayment, Action<TModel> action = null)
         {
             this.WarmCache();

             var commonData = new List<TModel>();

             var physOwners = this.Cache.GetCache<IndividualAccountOwner>()
                 .GetEntities()
                 .ToDictionary(x => x.Id);

             var legalOwners = this.Cache.GetCache<LegalAccountOwner>()
                 .GetEntities()
                 .ToDictionary(x => x.Id);

             var accounts = this.Cache.GetCache<BasePersonalAccount>().GetEntities()
                 .Select(x =>
                     new PersonalAccountPaymentDocProxy(
                         x))//, 
                         //(PersonalAccountOwner) physOwners.Get(x.AccountOwner.Id) ?? legalOwners.Get(x.AccountOwner.Id)))
                 .ToArray();

             var periodSummaryCache = this.Cache.GetCache<PersonalAccountPeriodSummary>();

             var allSummaries = periodSummaryCache.GetEntities()
                 .GroupBy(x => x.PersonalAccount.Id)
                 .ToDictionary(x => x.Key, x => x.Select(y => y));

             /*var periodSummaryDict = periodSummaryCache.GetEntities()
                 .Where(x => x.Period.Id == this.Period.Id)
                 .GroupBy(x => x.PersonalAccount.Id)
                 .ToDictionary(x => x.Key, x => x.FirstOrDefault());
                 */

        //ПЕРЕМЕННЫЕ НЕ ИСПОЛЬЗУЮТСЯ
        // словарь для получения переменных ОбщийДебет, ОбщийКредит, ОбщийДебетПени, ОбщийКредитПени
        /*var periodSummarySaldoDictByOwner = periodSummaryCache.GetEntities()
            .Where(x => x.Period.Id == this.Period.Id)
            .GroupBy(x => x.PersonalAccount.Id)
            .ToDictionary(x => x.Key, x => new
            {
                PenaltyDebet = x.Where(y => y.Penalty > 0).SafeSum(y => y.Penalty),
                SaldoOutDebet = x.Where(y => y.SaldoOut > 0).SafeSum(y => y.SaldoOut),
                PenaltyKredit = x.Where(y => y.Penalty < 0).SafeSum(y => y.Penalty),
                SaldoOutKredit = x.Where(y => y.SaldoOut < 0).SafeSum(y => y.SaldoOut)
            });
            */
        /*var crFundFormDecisions = this.Cache.GetCache<CrFundFormationDecision>().GetEntities()
            .GroupBy(x => x.Protocol.RealityObject.Id)
            .ToDictionary(x => x.Key, z => z.First());

        var crFundFormGovDecisions = this.Cache.GetCache<GovDecision>().GetEntities()
            .Where(x => x.DateStart <= this.Period.StartDate)
            .OrderByDescending(x => x.DateStart)
            .GroupBy(x => x.RealityObject.Id)
            .ToDictionary(x => x.Key, z => z.First());

        var penaltyDelayDecisions = this.Cache.GetCache<PenaltyDelayDecision>().GetEntities()
            .GroupBy(x => x.Protocol.RealityObject.Id)
            .ToDictionary(x => x.Key, z => z.First());

        var penaltyParams = this.Cache.GetCache<PaymentPenalties>().GetEntities();

        var documentDate = DateTime.Today.ToShortDateString();
        var localCounter = 0;

        var notifPeriodKind = this.indivDebtorConfig.DebtNotification.NotifPeriodKind.GetEnumMeta().Display;
        var notifDelayDaysCount = this.indivDebtorConfig.DebtNotification.NotifDelayDaysCount;
        var notifPrintType = this.indivDebtorConfig.DebtNotification.NotifPrintType.GetEnumMeta().Display;
        var notifDebtSumType = this.indivDebtorConfig.DebtNotification.NotifDebtSumType.GetEnumMeta().Display;

        var paySizeRecordDict = this.Cache.GetCache<PaysizeRecord>().GetEntities()
            .GroupBy(x => x.Municipality.Id)
            .ToDictionary(x => x.Key, y => y.FirstOrDefault());

        var accountPrivCategoryDict = this.Cache.GetCache<PersonalAccountPrivilegedCategory>().GetEntities()
            .GroupBy(x => x.PersonalAccount)
            .ToDictionary(x => x.Key, y => y.FirstOrDefault());

        var chargesDict = this.Cache.GetCache<PersonalAccountCharge>().GetEntities()
            .ToDictionary(x => x.BasePersonalAccount.Id);

        var cancelChargesDict = this.Cache.GetCache<CancelCharge>().GetEntities()
            .GroupBy(x => x.PersonalAccount.Id)
            .ToDictionary(x => x.Key, x => 
                new
                {
                    BaseTariff = x.Where(y => y.CancelType == CancelChargeType.BaseTariffCharge).SafeSum(y => y.CancelSum),
                    DecTariff = x.Where(y => y.CancelType == CancelChargeType.DecisionTariffCharge).SafeSum(y => y.CancelSum),
                    PenaltyTariff = x.Where(y => y.CancelType == CancelChargeType.Penalty).SafeSum(y => y.CancelSum)
                });
            .GroupBy(x => new { x.PersonalAccount.Id, x.CancelType })
            .ToDictionary(x => x.Key, x => x.SafeSum(y => y.CancelSum));

        var bankStatementMaxDate = this.Cache.GetCache<BankAccountStatement>()
            .GetEntities()
            .Max(x => x.DocumentDate as DateTime?);

        Dictionary<long, decimal> crPaymentsDict = null;
        IEntityCache<RealityObjectCrPayments> crPaymentsCache;
        if (this.Cache.TryGetCache<RealityObjectCrPayments>(out crPaymentsCache))
        {
            crPaymentsDict = crPaymentsCache.GetEntities().ToDictionary(x => x.RoId, x => x.Payments);
        }

        foreach (var account in accounts)
        {
            localCounter++;

            //var rec = this.GetRecord<TModel>(account, documentDate, localCounter, isZeroPayment);

            //if (periodSummaryDict.ContainsKey(account.Id) && !isZeroPayment)
            {
                //выносится--------------------

                //доля собственности
                /*decimal areaShare;
                //площадь
                decimal area;
                decimal tariff;
                //площадь*долю собственности
                decimal areaRoom;

                var calcParamTraceData = this.TraceCache.GetParameters(account.Account);

                if (calcParamTraceData != null)
                {
                    areaShare = calcParamTraceData.Get(VersionedParameters.AreaShare).ToDecimal();
                    tariff = calcParamTraceData.Get(VersionedParameters.BaseTariff).ToDecimal();
                    area = calcParamTraceData.Get(VersionedParameters.RoomArea).ToDecimal();
                    areaRoom = areaShare * area;
                }
                else
                {
                    var tarifParam = this.ParamTracker.GetParameter(
                        VersionedParameters.BaseTariff,
                        account.Account, this.Period);

                    tariff = tarifParam.GetActualByDate<decimal>(
                        account.Account, this.Period.EndDate ?? this.Period.StartDate.AddMonths(1).AddDays(-1),
                        false);

                    var areaParam = this.ParamTracker.GetParameter(VersionedParameters.RoomArea, account.Account, this.Period);
                    area = areaParam.GetActualByDate<decimal?>(
                        account.Account, 
                        this.Period.EndDate ?? this.Period.StartDate,
                        true,
                        true) ?? account.Area;

                    var areaShareParam = this.ParamTracker.GetParameter(
                        VersionedParameters.AreaShare,
                        account.Account, this.Period);

                    areaShare = areaShareParam.GetActualByDate<decimal?>(
                        account.Account, 
                        this.Period.EndDate ?? this.Period.StartDate,
                        true) ?? account.AreaShare;

                    areaRoom = area * areaShare;
                }

                rec.ОбщаяПлощадь = areaRoom;
                rec.Тариф = tariff;

                var periodSummary = periodSummaryDict[account.Id];
                var charge = chargesDict.Get(account.Id);
                var cancelCharge = cancelChargesDict.Get(account.Id);                   

                // "Переплата/Задолженность на начало периода" = "Сальдо на начало периода" - "Пени на начало пероида"
                // т.к. в periodSummary уже все агрегировано
                //var startDebt = periodSummary.SaldoIn;
                //агрегированно пени тоже!
                //rec.ПереплатаНаНачало = startDebt < 0 ? -startDebt : 0;

                // т.к. теперь в НачисленоXxxxxПериод сидят только начисления, а в перерасчётах всё остальное
                // нам нужно извлечь следующие операции:
                // 1. Перерасчет                            - сидит в перерасчете
                // 2. Перенос долга при слиянии             - сидит в начислено (или оплачено)
                // 3. Установка и изменение сальдо          - сидит в изменении баланса
                // 4. Зачет средств за выполненные работы   - сидит в изменении баланса
                // 5. Отмена начислений                     - сидит в начислено
                // Операции 2 и 5 можем получить из разницы начислений в PeriodSummary и начислений из PersonalAccountCharge

                rec.НачисленоБазовый = charge.ReturnSafe(z => z.ChargeTariff - z.OverPlus);
                rec.НачисленоТарифРешения = charge.ReturnSafe(z => z.OverPlus);
                rec.НачисленоПени = charge.ReturnSafe(z => z.Penalty);

                rec.ПерерасчетБазовый = periodSummary.RecalcByBaseTariff;
                rec.ПерерасчетТарифРешения = periodSummary.RecalcByDecisionTariff;
                rec.ПерерасчетПени = periodSummary.RecalcByPenalty;

                rec.ОтменыБазовый = cancelCharge.BaseTariff;
                rec.ОтменыТарифРешения = cancelCharge.DecTariff;
                rec.ОтменыПени = cancelCharge.PenaltyTariff;

                rec.КорректировкаБазовый = periodSummary.BaseTariffChange;
                rec.КорректировкаТарифРешения = periodSummary.DecisionTariffChange;
                rec.КорректировкаПени = periodSummary.PenaltyChange;

                rec.СлияниеБазовый = periodSummary.ChargedByBaseTariff - rec.ОтменыБазовый;
                rec.СлияниеТарифРешения = periodSummary.ChargeTariff 
                    - periodSummary.ChargedByBaseTariff 
                    - rec.ОтменыТарифРешения;
                rec.СлияниеПени = periodSummary.Penalty - rec.ОтменыПени;

                //с учетом соц поддержки
                rec.ОплаченоБазовый = periodSummary.TariffPayment;
                rec.ОплаченоТарифРешения = periodSummary.TariffDecisionPayment;
                rec.ОплаченоПени = periodSummary.PenaltyPayment;

                var startBaseTariffDebt = periodSummary.BaseTariffDebt;
                var startDecisionTariffDebt = periodSummary.DecisionTariffDebt;
                var startPenaltyDebt = periodSummary.PenaltyDebt;

                rec.ДолгБазовыйНаНачало = startBaseTariffDebt;
                rec.ДолгТарифРешенияНаНачало = startDecisionTariffDebt;
                rec.ДолгПениНаНачало = startPenaltyDebt;

                //самая полная задолженность на конец, включает в себя все.
                //Долг+Начислено(с отменами)+перерасчет-оплачено+ручные корректировки
                rec.ДолгБазовыйНаКонец = startBaseTariffDebt 
                    + periodSummary.ChargedByBaseTariff 
                    + rec.КорректировкаБазовый
                    + rec.ПерерасчетБазовый
                    - rec.ОплаченоБазовый;

                rec.ДолгТарифРешенияНаКонец = startDecisionTariffDebt
                    + periodSummary.ChargeTariff 
                    - periodSummary.ChargedByBaseTariff
                    + rec.КорректировкаТарифРешения
                    + rec.ПерерасчетТарифРешения
                    - rec.ОплаченоТарифРешения;

                rec.ДолгПениНаКонец = startPenaltyDebt 
                    + periodSummary.Penalty
                    + rec.КорректировкаПени
                    + rec.ПерерасчетПени
                    - rec.ОплаченоПени;

                rec.СоцПоддержка = this.socialSupportDict.Get(account.SsWalletGuid);

                rec.НачисленоБазовыйПериод = charges.ReturnSafe(z => z.ChargeTariff - z.OverPlus);
                rec.НачисленоПениПериод = charges?.Penalty ?? 0;
                rec.НачисленоТарифРешенияПериод = charges?.OverPlus ?? 0;

                rec.ПерерасчетБазовыйПериод = periodSummary.RecalcByBaseTariff 
                                                    + periodSummary.BaseTariffChange 
                                                    + periodSummary.ChargedByBaseTariff 
                                                    - rec.НачисленоБазовыйПериод;

                rec.ПерерасчетПениПериод = periodSummary.RecalcByPenalty 
                                                    + periodSummary.PenaltyChange
                                                    + periodSummary.Penalty
                                                    - rec.НачисленоПениПериод;
                                                    isZeroPayment
                rec.ПерерасчетТарифРешенияПериод = periodSummary.RecalcByDecisionTariff 
                                                    + periodSummary.DecisionTariffChange
                                                    + periodSummary.ChargeTariff 
                                                    - periodSummary.ChargedByBaseTariff
                                                    - rec.НачисленоТарифРешенияПериод;

                rec.ПереплатаНаНачало = startDebt < 0 ? -startDebt : 0;

                rec.ДолгНаНачало = startDebt > 0 ? startDebt : 0;

                rec.ПереплатаПениНаНачало = startPenaltyDebt < 0 ? -startPenaltyDebt : 0;

                rec.ДолгПениНаНачало = startPenaltyDebt > 0 ? startPenaltyDebt : 0;

                rec.Начислено = periodSummary.ChargeTariff;

                rec.НачисленоБазовый = periodSummary.GetChargedByBaseTariff();

                rec.НачисленоСверхБазового = periodSummary.GetChargedByDecisionTariff();

                rec.Перерасчет = periodSummary.RecalcByBaseTariff + periodSummary.RecalcByDecisionTariff;

                rec.ПерерасчетБазовый = periodSummary.RecalcByBaseTariff;

                rec.ПерерасчетТарифРешения = periodSummary.RecalcByDecisionTariff;

                rec.Оплачено = periodSummary.TariffPayment + periodSummary.TariffDecisionPayment
                    - this.socialSupportDict.Get(account.SsWalletGuid);

                rec.ОплаченоБазовый = periodSummary.TariffPayment;

                rec.ОплаченоТарифРешения = periodSummary.TariffDecisionPayment;

                var endOverPay = startDebt + rec.Начислено + rec.Перерасчет
                    - this.socialSupportDict.Get(account.SsWalletGuid) - rec.Оплачено;

                rec.ПереплатаНаКонец = endOverPay < 0 ? -endOverPay : 0;

                rec.ДолгНаКонец = endOverPay > 0 ? endOverPay : 0;

                rec.ОплаченоПени = periodSummary.PenaltyPayment;

                rec.НачисленоПени = periodSummary.Penalty;

                rec.ПерерасчетПени = periodSummary.RecalcByPenalty;

                var penaltyEndDebt = startPenaltyDebt + rec.НачисленоПени - rec.ОплаченоПени + rec.ПерерасчетПени;

                rec.ПереплатаПениНаКонец = penaltyEndDebt < 0 ? -penaltyEndDebt : 0;

                rec.ДолгПениНаКонец = penaltyEndDebt > 0 ? penaltyEndDebt : 0;

                rec.СальдоБазТарифНачало = periodSummary.BaseTariffDebt;

                rec.СальдоТарифРешНачало = periodSummary.DecisionTariffDebt;


                decimal crPaid = crPaymentsDict?.Get(account.RoId) ?? 0m;

                rec.ПотраченоНаКР = crPaid;

                PaysizeRecord minTarif;
                if (paySizeRecordDict.TryGetValue(account.MunicipalityId, out minTarif)
                    || paySizeRecordDict.TryGetValue(account.SettlementId.GetValueOrDefault(), out minTarif))

                {
                    rec.МинимальныйТариф = minTarif != null ? (minTarif.Value ?? 0m) : 0m;
                }

                PersonalAccountPrivilegedCategory accountCategory;
                if (accountPrivCategoryDict.TryGetValue(account.Account, out accountCategory))
                {
                    rec.ПроцентЛьготы = accountCategory
                        .Return(x => x.PrivilegedCategory)
                        .Return(x => x.Percent) ?? 0m;
                }

                rec.НомерПомещения = account.RoomNum;

                rec.УплаченоФКР = Math.Round(
                    Math.Abs(allSummaries.Get(account.Id).ReturnSafe(x => x.Sum(y => y.TariffPayment + y.TariffDecisionPayment))),
                    2,
                    MidpointRounding.AwayFromZero);

                rec.ШтрихКод = rec.ИннПолучателя != null && rec.ИннПолучателя.Length >= 10
                    ? rec.ИннПолучателя.Substring(5, 5) + rec.ЛицевойСчет
                    : string.Empty;

                string payTo = null;
                var rofundDecision = crFundFormDecisions.Get(account.RoId);
                var rofundGovDecision = crFundFormGovDecisions.Get(account.RoId);
                var penaltyDelayDecision = penaltyDelayDecisions.Get(account.RoId);

                if (rofundDecision != null || rofundGovDecision != null)
                {
                    if (penaltyDelayDecision != null)
                    {
                        var penaltyDelay = penaltyDelayDecision.Decision
                            .Where(x => !x.To.HasValue || x.To >= this.Period.StartDate)
                            .FirstOrDefault(x => x.From <= this.Period.StartDate);

                        if (penaltyDelay != null)
                        {
                            payTo = penaltyDelay.MonthDelay
                                ? this.Period.StartDate.AddMonths(2).ToShortDateString()
                                : this.Period.StartDate.AddMonths(1)
                                    .AddDays(penaltyDelay.DaysDelay)
                                    .ToShortDateString();
                        }
                    }

                    if (payTo.IsEmpty())
                    {
                        //если есть только протокол решения дома или этот протокол более актуальный, чем протокол решения гос власти
                        CrFundFormationDecisionType type;
                        if (rofundGovDecision == null ||
                            (rofundDecision != null && rofundDecision.StartDate > rofundGovDecision.DateStart))
                        {
                            type = rofundDecision.Decision;
                        }
                        else
                        {
                            type = CrFundFormationDecisionType.RegOpAccount;
                        }

                        var penaltyParameter = penaltyParams
                            .Where(x => x.DateStart <= this.Period.StartDate)
                            .OrderByDescending(x => x.DateStart)
                            .FirstOrDefault(x => x.DecisionType == type);

                        if (penaltyParameter != null)
                        {
                            payTo = this.Period.StartDate.AddMonths(1)
                                .AddDays(penaltyParameter.Return(x => x.Days))
                                .ToShortDateString();
                        }
                    }
                }

                rec.ОплатитьДо = payTo;
            }

            if (periodSummarySaldoDictByOwner.ContainsKey(account.Id))
            {
                var periodSummarySaldo = periodSummarySaldoDictByOwner[account.Id];

                rec.ОбщийДебет = periodSummarySaldo.SaldoOutDebet;
                rec.ОбщийДебетПени = periodSummarySaldo.PenaltyDebet;
                rec.ОбщийКредит = periodSummarySaldo.SaldoOutKredit;
                rec.ОбщийКредитПени = periodSummarySaldo.PenaltyKredit;
            }

<<<<<<< HEAD
            rec.Итого = rec.ДолгНаКонец;
            rec.Пени = rec.ДолгПениНаКонец;
            rec.СоцПоддержка = this.socialSupportDict.Get(account.SsWalletGuid);
            rec.СуммаВсего = rec.ДолгНаКонец + rec.ДолгПениНаКонец;
            rec.ИтогоКОплате = rec.Итого + rec.Пени;
            rec.ПредоставленнаяМСП = this.socialSupportDict.Get(account.SsWalletGuid);//то же самое, что соц поддержка
=======
                rec.ЕдиницаПериодаПросрочки = notifPeriodKind;
                rec.ПериодПросрочки = notifDelayDaysCount;
                rec.ПечататьУведомление = notifPrintType;
                rec.УчитыватьСумму = notifDebtSumType;
                rec.ДатаЗакрытияПериода = this.Period.ObjectEditDate.ToShortDateString();
>>>>>>> release-1.31.0

            rec.ЕдиницаПериодаПросрочки = notifPeriodKind;
            rec.ПериодПросрочки = notifDelayDaysCount;
            rec.ПечататьУведомление = notifPrintType;
            rec.УчитыватьСумму = notifDebtSumType;

            var debtorClaimWork = this.debtorClaimWorks.Get(account.Id);

            if (debtorClaimWork != null)
            {
                rec.СтатусЗадолженности = debtorClaimWork.State != null
                    ? debtorClaimWork.State.Name
                    : string.Empty;
                rec.КоличествоДнейПросрочки = debtorClaimWork.CountDaysDelay.ToInt();
                rec.КоличествоМесяцевПросрочки = debtorClaimWork.CountMonthDelay;
            }
            
        if (rec.ЛицевойСчет.IsNotEmpty() && bankStatementMaxDate != null)
        {{
            rec.ДатаПоследнейОплаты = bankStatementMaxDate.Value.ToString("dd.MM.yyyy");
        }
    }


                if (action != null)
                {
                    action(rec);
                }

                commonData.Add(rec);
            }

            return commonData;
        }

        protected void WarmCache()
        {

            this.paymentDocInfoList = this.Cache.GetCache<PaymentDocInfo>().GetEntities()
                .Where(x => !this.Period.EndDate.HasValue || x.DateStart <= this.Period.EndDate)
                .Where(x => (!x.DateEnd.HasValue || x.DateEnd >= this.Period.StartDate))
                .ToList();

            this.RegOperator = this.Cache.GetCache<Gkh.Modules.RegOperator.Entities.RegOperator.RegOperator>()
                .GetByKey(ContragentState.Active.ToString());

            
            this.manOrgByRealObjDict = this.Cache.GetCache<ManOrgContractRealityObject>().GetEntities()
                .Select(x => new
                    {
                        x.ManOrgContract.StartDate,
                        x.ManOrgContract.ManagingOrganization,
                        x.RealityObject.IdpaymentDocInfoList
                    })
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key,
                    y => y.OrderByDescending(x => x.StartDate).Select(x => x.ManagingOrganization).First());

            // получаем только те дома у которых тип Решения = Специальный
            this.roWithCrDunDecision = this.Cache.GetCache<CrFundFormationDecision>().GetEntities()
                .Where(x => x.Decision == CrFundFormationDecisionType.SpecialAccount)
                .Select(x => x.Protocol.RealityObject.Id)
                .ToHashSet();
            // получаем только те дома у которых тип Владелеца счета =  Рег. оператор
            this.roWithRegopDecision = this.Cache.GetCache<AccountOwnerDecision>().GetEntities()
                .Where(x => x.DecisionType == AccountOwnerDecisionType.RegOp)
                .Select(x => x.Protocol.RealityObject.Id)
                .ToHashSet();

             var activeNotifAccNum = this.Cache.GetCache<DecisionNotification>().GetEntities()
                .Select(x => new
                {
                    RoId = x.Protocol.RealityObject.Id,
                    x.AccountNum
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.AccountNum).FirstOrDefault());

            this.RoWithCustomDecisionType = this.Cache.GetCache<AccountOwnerDecision>().GetEntities()
                .Where(x => x.DecisionType == AccountOwnerDecisionType.Custom)
                .Select(x => x.Protocol.RealityObject.Id)
                .ToHashSet();

            var contragentBanks = this.Cache.GetCache<InvoiceBankProxy>().GetEntities();

            this.Banks = this.Cache.GetCache<CreditOrgDecision>().GetEntities()
                .Select(x => new
                {
                    RoId = x.Protocol.RealityObject.Id,
                    x.Decision.Name,
                    x.Decision.Bik,
                    x.Decision.CorrAccount,
                    x.Decision.Address
                })
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => new InvoiceBankProxy
                        {
                            RoId = x.RoId,
                            SettlementAccount = activeNotifAccNum.Get(x.RoId),
                            Name = x.Name,
                            Bik = x.Bik,
                            CorrAccount = x.CorrAccount,
                            Address = x.Address
                        })
                        .FirstOrDefault());

            foreach (var bank in contragentBanks)
            {
                this.Banks[bank.RoId] = bank;
            }

            this.socialSupportDict = this.Cache.GetCache<Transfer>().GetEntities()
                .Where(x => x.PaymentDate.Date >= this.Period.StartDate)//ИСПОЛЬЗОВАТЬ ПЕРИОД
                .Where(x => x.PaymentDate.Date <= this.Period.EndDate)
                .Select(x => new
                    {
                    WalletGuid = x.TargetGuid,
                        x.Amount
                    })
                    })
                .AsEnumerable()
                .GroupBy(x => x.WalletGuid)
                .ToDictionary(x => x.Key, y => y.SafeSum(x => x.Amount));

            this.delAgentByRealObj = this.Cache.GetCache<DeliveryAgentRealObj>().GetEntities()
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.DeliveryAgent.Contragent.Name).First());

            this.debtorClaimWorks = this.Cache.GetCache<DebtorClaimWork>().GetEntities()
                .GroupBy(x => x.PersonalAccount.Id)
                .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.ObjectCreateDate).First());
        }

        private T GetRecord<T>(PersonalAccountPaymentDocProxy account,
            string documentDate,
            int accountIndex,
            bool isZeroPayment) where T : InvoiceInfo, new()
        {
            var contragent = this.RoWithCustomDecisionType.Contains(account.RoId)
                ? this.manOrgByRealObjDict.Get(account.RoId).Return(x => x.Contragent)
                : this.RegOperator.Return(x => x.Contragent);
            var bank = this.Banks.Get(account.RoId);

            var crFoundMessage = this.roWithCrDunDecision.Contains(account.RoId)
                                 && this.roWithRegopDecision.Contains(account.RoId)
                ? "Дом формирует фонд капитального ремонта на специальном счёте"
                : string.Empty;
                
            // Если у квартиры выставлена галочка, что дом у квартиры отсутсвует ЛС и при этом указано Примечание
            // То вместо лицевого счета ставим Примечание из карточки квартиры

            var personalaccount = account.PersonalAccountNum;

            var fundFormationType = this.roWithCrDunDecision.Contains(account.RoId)
                ? this.roWithRegopDecision.Contains(account.RoId)
                    ? FundFormationType.SpecRegop
                    : FundFormationType.Special
                : FundFormationType.Regop;

            var info = this.paymentDocInfoList
                .Where(x => ((x.RealityObject == null || x.RealityObject.Id == account.RoId)
                             &&
                             (x.RealityObject != null || x.FundFormationType == FundFormationType.NotSet ||
                              x.FundFormationType == fundFormationType)
                        && (x.Locality == null || x.Locality.AOGuid == account.PlaceGuidId || account.PlaceGuidId == null)
                        && (x.MoSettlement == null || x.MoSettlement.Id == account.SettlementId)
                        && (x.Municipality == null || x.Municipality.Id == account.MunicipalityId))
                    || x.IsForRegion)
                .Select(x => new
                {
                    x.Information,
                    // Частная информация приоритетнее общей
                    Priority = x.RealityObject != null
                        ? 1
                        : (x.Locality != null
                            ? 2
                            : (x.MoSettlement != null ? 3 : 4))
                })
                .GroupBy(x => x.Priority)
                .Select(x => new { Priopity = x.Key, info = x.Select(y => y.Information).ToList() })
                .OrderBy(x => x.Priopity)
                .Select(x => x.info.AggregateWithSeparator("\n"))
                .FirstOrDefault();

            return new T
            {
                AccountId = account.Id,
                ВнешнийЛС = account.ExternalId,
                ФондСпецСчет = crFoundMessage,
                OwnerId = account.OwnerId,
                ЛицевойСчет = personalaccount,
                НаименованиеПериода = this.Period.Name,
                ДатаНачалаПериода = this.Period.StartDate.ToString("dd.MM.yyyy"),
                ДатаОкончанияПериода = this.Period.EndDate.ToDateString("dd.MM.yyyy"),
                МесяцГодНачисления = this.Period.StartDate.ToString("MMMM yyyy"),
                АдресКвартиры = string.Format(
                    "{0}{1}",
                    account.AddressName,
                    account.IsRoomHasNoNumber ? account.Notation : ", кв. " + account.RoomNum),
                ДатаДокумента = documentDate,
                НаименованиеПолучателя = contragent != null ? contragent.Name : string.Empty,
                ИннПолучателя = contragent != null ? contragent.Inn : string.Empty,
                КппПолучателя = contragent != null ? contragent.Kpp : string.Empty,
                ОргнПолучателя = contragent != null ? contragent.Ogrn : string.Empty,
                РсчетПолучателя = bank.Return(x => x.SettlementAccount, string.Empty),
                АдресБанка = bank.Return(x => x.Address, string.Empty),
                НаименованиеБанка = bank.Return(x => x.Name, string.Empty),
                БикБанка = bank.Return(x => x.Bik, string.Empty),
                КсБанка = bank.Return(x => x.CorrAccount, string.Empty),
                ТелефоныПолучателя = contragent != null ? contragent.Phone : string.Empty,
                АдресПолучателя = contragent != null ? contragent.MailingAddress : string.Empty,
                EmailПолучателя = contragent != null ? contragent.Email : string.Empty,
                WebSiteПолучателя = contragent != null ? contragent.OfficialWebsite : string.Empty,
                Информация = info,
                ЗначениеQRКода = isZeroPayment ? string.Empty : account.PersonalAccountNum,
                ШтрихКод = string.Empty,
                Индекс = account.PostCode,
                СпособФормированияФонда = this.roWithCrDunDecision.Contains(account.RoId)
                    ? "Специальный счет"
                    : "Счет регионального оператора",
                АгентДоставки = this.delAgentByRealObj.Get(account.RoId),
                Счетчик = accountIndex,
                Municipality = account.Municipality,
                Settlement = account.Settlement,
                OwnerType = (int)account.OwnerType,
                RoomType = (int)account.RoomType
            };
        }

        protected int GetDocNumber(long accountId)
        {
            var cache = this.Cache.GetCache<PaymentDocument>();

            // Если в рамках периода по ЛС уже печатали счета, то номер выводим уже существующий

            var existingNum = cache.GetByKey("{0}|{1}|{2}".FormatUsing(accountId, this.Period.Id, DateTime.Today.Year));

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
                    PeriodId = this.Period.Id,
                    Year = DateTime.Today.Year,
                    Number = documentNumber
                };

                this.paymentDocDmn.Save(newRecord);
                cache.AddEntity(newRecord);
            }
            else
            {
                documentNumber = existingNum.Number;
            }

            return documentNumber;
        }

        protected int GetNextSerialNumber()
        {
            int serialNumber;
            //будем использовать уже существующую таблицу, которую никто не помнит зачем сделали
            var selectString = "SELECT DOC_NUMBER FROM REGOP_PAYMENT_DOC_PRINT";
            var updateString = "UPDATE REGOP_PAYMENT_DOC_PRINT SET DOC_NUMBER = :docNumber";
            var whereString = " WHERE DOC_YEAR = :year AND ACCOUNT_ID = -1 AND PERIOD_ID = -1";

            // всё таки нужно так делать, номер был уникальным и сквозным
            // лучше последовательностью сделать, но кто будет сбрасывать её каждый год
            lock (PaymentDocument.SyncObject)
            {
                var currentSession = this.sessionProvider.GetCurrentSession();
                
                serialNumber = currentSession.CreateSQLQuery(selectString + whereString)
                    .SetInt32("year", this.Period.StartDate.Year)
                    .List<object>()
                    .FirstOrDefault()
                    .ToInt(-1);

                if (serialNumber == -1)
                {
                    // так как один раз делается в год, то можно и так
                    this.paymentDocDmn.Save(new PaymentDocument
                    {
                        AccountId = -1,
                        PeriodId = -1,
                        Year = this.Period.StartDate.Year,
                        Number = 1
                    });
                    serialNumber = 1;
                }
                else
                {
                    serialNumber++;
                    currentSession.CreateSQLQuery(updateString + whereString)
                        .SetInt32("docNumber", serialNumber)
                        .SetInt32("year", this.Period.StartDate.Year)
                        .ExecuteUpdate();
                }
            }
            
            return serialNumber;
        }
    }

    
    internal class AccountChargeGuidDto
    {
        public long AccountId { get; set; }

        public string Guid { get; set; }*/
    }
}