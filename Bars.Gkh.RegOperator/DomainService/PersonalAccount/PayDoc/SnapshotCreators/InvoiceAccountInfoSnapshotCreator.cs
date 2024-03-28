namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators
{
    using System.Collections.Generic;
    using System.Linq;

    using B4.Utils;

    using Bars.Gkh.Domain.Cache;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.Events;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    using Newtonsoft.Json;

    /// <summary>
    /// Создатель слепков данных
    /// </summary>
    internal class InvoiceAccountInfoSnapshotCreator
    {
        private readonly IPeriod period;
        private readonly GkhCache cache;
        private readonly List<ISnapshotBuilder> builders;

        private Dictionary<long, PersonalAccountCharge> chargesDict;
        private Dictionary<long, PersonalAccountPeriodSummary> periodSummaryDict;
        private Dictionary<long, CancelSums> cancelChargesDict;
        private Dictionary<string, decimal> socialSupportDict;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="period">Период сборки</param>
        /// <param name="builders">Используемые источники</param>
        public InvoiceAccountInfoSnapshotCreator(IWindsorContainer container, IPeriod period, List<ISnapshotBuilder> builders)
        {
            this.period = period;
            this.cache = container.Resolve<GkhCache>();
            this.builders = builders;
        }

        /// <summary>
        /// Создание слепка данных
        /// </summary>
        public void CreateSnapshots(List<TariffAreaRecord> accountsInfo)
        {
            if (this.builders.Select(x => x.Code).Contains(GroupedChargesAndPaymentsBuilder.Id))
            {
                this.periodSummaryDict = this.cache.GetCache<PersonalAccountPeriodSummary>().GetEntities()
                    .Where(x => x.Period.Id == this.period.Id)
                    .GroupBy(x => x.PersonalAccount.Id)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                this.chargesDict = this.cache.GetCache<PersonalAccountCharge>().GetEntities()
                    .ToDictionary(x => x.BasePersonalAccount.Id);

                this.cancelChargesDict = this.cache.GetCache<CancelCharge>().GetEntities()
                    .GroupBy(x => x.PersonalAccount.Id)
                    .ToDictionary(
                        x => x.Key,
                        x =>
                            new CancelSums
                            {
                                BaseTariff = x.Where(y => y.CancelType == CancelType.BaseTariffCharge).SafeSum(y => y.CancelSum),
                                DecisionTariff = x.Where(y => y.CancelType == CancelType.DecisionTariffCharge).SafeSum(y => y.CancelSum),
                                Penalty = x.Where(y => y.CancelType == CancelType.Penalty).SafeSum(y => y.CancelSum),
                                BaseTariffChange = x.Where(y => y.CancelType == CancelType.BaseTariffChange).SafeSum(y => y.CancelSum),
                                DecisionTariffChange = x.Where(y => y.CancelType == CancelType.DecisionTariffChange).SafeSum(y => y.CancelSum),
                                PenaltyChange = x.Where(y => y.CancelType == CancelType.PenaltyChange).SafeSum(y => y.CancelSum)
                            });

                this.socialSupportDict = this.cache.GetCache<PersonalAccountPaymentTransfer>().GetEntities()
                    .Where(x => x.ChargePeriod.Id == this.period.Id)
                    .Select(
                        x => new
                        {
                            WalletGuid = x.TargetGuid,
                            x.Amount
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.WalletGuid)
                    .ToDictionary(x => x.Key, y => y.SafeSum(x => x.Amount));
            }

            var accountsData = accountsInfo
                .Select(x => this.GetAccountInfo(x))
                .ToArray();

            this.CreateSnapshots(accountsData);
        }

        private AccountInfo GetAccountInfo(TariffAreaRecord record)
        {
            var roomAddress = string.Format("{0} {1}",
                record.Account.Room.RealityObject.FiasAddress.AddressName,
                record.Account.Room.IsRoomHasNoNumber
                    ? record.Account.Room.Notation
                    : ", кв. " + record.Account.Room.RoomNum);

            var fiasAddress = record.Account.Room.RealityObject.FiasAddress;

            var result = new AccountInfo
            {
                AccountId = record.Account.Id,
                OwnerId = record.OwnerId,
                НомерЛС = record.Account.PersonalAccountNum,
                АдресПомещения = roomAddress,
                Примечание = record.Account.Room.Notation,
                Тариф = record.Tariff,
                БазовыйТариф = record.BaseTariff,
                ПлощадьПомещения = record.RoomArea * record.AreaShare,
                ТипПомещения = record.Account.Room.Type.GetEnumMeta().Display,
                НаименованиеРабот = "Взносы на капитальный ремонт",
                RoId = record.Account.Room.RealityObject.Id,
                AreaLivingNotLivingMkd = record.Account.Room.RealityObject.AreaLivingNotLivingMkd,
                АдресДома = record.Account.Room.RealityObject.Address,
                RoomType = (int)record.Account.Room.Type,
                НаселенныйПункт = fiasAddress.PlaceName,
                Улица = fiasAddress.StreetName,
                Дом = fiasAddress.House,
                Литер = fiasAddress.Letter,
                Корпус = fiasAddress.Building,
                Секция = fiasAddress.Housing,
                СтатусЛС = record.Account.State.Name,
                Квартира = record.Account.Room.RoomNum,
                НомерКомнаты = record.Account.Room.ChamberNum,
                МрЛицевойСчет = record.Account.Room.RealityObject.Municipality.Name,
                ПришлоСоСпецсчета = record.Account.DecisionChargeBalance

            };

            var chargesAndPaymentsBuilder = this.builders.FirstOrDefault(x => x.Code == GroupedChargesAndPaymentsBuilder.Id);

            if (chargesAndPaymentsBuilder != null)
            {
                var periodSummary = this.periodSummaryDict.Get(record.Account.Id);
                var charge = this.chargesDict.Get(record.Account.Id);
                var cancelCharge = this.cancelChargesDict.Get(record.Account.Id);

                var chargeDecision = periodSummary.ChargeTariff - periodSummary.ChargedByBaseTariff;

                result.НачисленоБазовый = charge.ReturnSafe(z => z.ChargeTariff - z.OverPlus);
                result.НачисленоТарифРешения = charge.ReturnSafe(z => z.OverPlus);
                result.НачисленоПени = charge.ReturnSafe(z => z.Penalty);

                result.ПерерасчетБазовый = periodSummary.RecalcByBaseTariff;
                result.ПерерасчетТарифРешения = periodSummary.RecalcByDecisionTariff;
                result.ПерерасчетПени = periodSummary.RecalcByPenalty;
                result.ЗачетБазовый = periodSummary.PerformedWorkChargedBase;
                result.ЗачетРешения = periodSummary.PerformedWorkChargedDecision;

                result.ОтменыБазовый = cancelCharge.ReturnSafe(z => z.BaseTariff);
                result.ОтменыТарифРешения = cancelCharge.ReturnSafe(z => z.DecisionTariff);
                result.ОтменыПени = cancelCharge.ReturnSafe(z => z.Penalty);

                result.ОтменыКорректировкаБазовый = cancelCharge.ReturnSafe(z => z.BaseTariffChange);
                result.ОтменыКорректировкаТарифРешения = cancelCharge.ReturnSafe(z => z.DecisionTariffChange);
                result.ОтменыКорректировкаПени = cancelCharge.ReturnSafe(z => z.PenaltyChange);

                //BaseTariffChange = корректировки - отмены корректировок
                result.КорректировкаБазовый = periodSummary.BaseTariffChange + result.ОтменыКорректировкаБазовый;
                result.КорректировкаТарифРешения = periodSummary.DecisionTariffChange + result.ОтменыКорректировкаТарифРешения;
                result.КорректировкаПени = periodSummary.PenaltyChange + result.ОтменыКорректировкаПени;

                //ChargedByBaseTariff = чистые начисления - отмены + перенос долга при слиянии
                result.СлияниеБазовый = periodSummary.ChargedByBaseTariff - result.НачисленоБазовый + result.ОтменыБазовый;
                result.СлияниеТарифРешения = chargeDecision - result.НачисленоТарифРешения + result.ОтменыТарифРешения;
                result.СлияниеПени = periodSummary.Penalty - result.НачисленоПени + result.ОтменыПени;

                //с учетом соц поддержки
                result.ОплаченоБазовый = periodSummary.TariffPayment;
                result.ОплаченоТарифРешения = periodSummary.TariffDecisionPayment;
                result.ОплаченоПени = periodSummary.PenaltyPayment;

                var startBaseTariffDebt = periodSummary.BaseTariffDebt;
                var startDecisionTariffDebt = periodSummary.DecisionTariffDebt;
                var startPenaltyDebt = periodSummary.PenaltyDebt;

                result.ДолгБазовыйНаНачало = startBaseTariffDebt;
                result.ДолгТарифРешенияНаНачало = startDecisionTariffDebt;
                result.ДолгПениНаНачало = startPenaltyDebt;

                //самая полная задолженность на конец, включает в себя все.
                //Долг+Начислено(с отменами)+перерасчет-оплачено+ручные корректировки(c отменами)
                result.ДолгБазовыйНаКонец = startBaseTariffDebt
                    + periodSummary.ChargedByBaseTariff
                    + periodSummary.BaseTariffChange
                    + result.ПерерасчетБазовый
                    - result.ОплаченоБазовый;

                result.ДолгТарифРешенияНаКонец = startDecisionTariffDebt
                    + chargeDecision
                    + periodSummary.DecisionTariffChange
                    + result.ПерерасчетТарифРешения
                    - result.ОплаченоТарифРешения;

                result.ДолгПениНаКонец = startPenaltyDebt
                    + periodSummary.Penalty
                    + periodSummary.PenaltyChange
                    + result.ПерерасчетПени
                    - result.ОплаченоПени;

                result.СоцПоддержка = this.socialSupportDict.Get(record.Account.SocialSupportWallet.WalletGuid);

                result.ИтогоПоБазовомуТарифу = result.НачисленоБазовый + result.ПерерасчетБазовый;

                result.ИтогоПоТарифуРешений = result.НачисленоТарифРешения + result.ПерерасчетТарифРешения;

                result.ИтогоПоПени = result.НачисленоПени + result.ПерерасчетПени;

                result.ИтогоКОплате = result.ИтогоПоБазовомуТарифу + result.ИтогоПоТарифуРешений + result.ИтогоПоПени;

                if (chargesAndPaymentsBuilder.SectionEnabled("OldVariables"))
                {
                    result.ДолгНаНачало = periodSummary.SaldoIn;
                    result.НачисленоБазовыйПериод = result.НачисленоБазовый;
                    result.НачисленоПениПериод = result.НачисленоПени;
                    result.НачисленоТарифРешенияПериод = result.НачисленоТарифРешения;
                    result.Оплачено = periodSummary.GetTotalPayment();
                    result.Перерасчёт = result.ПерерасчетБазовый + result.ПерерасчетТарифРешения + result.ПерерасчетПени;

                    result.ПерерасчетБазовыйПериод = periodSummary.ReturnSafe(z => z.RecalcByBaseTariff + z.BaseTariffChange + z.ChargedByBaseTariff)
                        - result.НачисленоБазовый;

                    result.ПерерасчетПениПериод = periodSummary.ReturnSafe(z => z.RecalcByPenalty + z.PenaltyChange + z.Penalty)
                        - result.НачисленоПени;

                    result.ПерерасчетТарифРешенияПериод = periodSummary.ReturnSafe(z => z.RecalcByDecisionTariff + z.DecisionTariffChange
                            + z.ChargeTariff - z.ChargedByBaseTariff)
                        - result.НачисленоТарифРешения;

                    result.Сумма = periodSummary.ChargeTariff;
                    result.СуммаБазовый = periodSummary.Return(z => z.GetChargedByBaseTariff());
                    result.СуммаПени = periodSummary.Penalty;
                    result.СуммаСверхБазового = periodSummary.Return(z => z.GetChargedByDecisionTariff());
                }
            }

            return result;
        }

        public void CreateSnapshots(IEnumerable<AccountInfo> accountsData, bool isBase = true)
        {
            foreach (var accountInfo in accountsData)
            {
                var accountData = new AccountPaymentInfoSnapshot
                {
                    AccountId = accountInfo.AccountId,
                    AccountNumber = accountInfo.НомерЛС,
                    Data = JsonConvert.SerializeObject(accountInfo, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    }),
                    ChargeSum = accountInfo.ИтогоКОплате,
                    RoomAddress = accountInfo.АдресПомещения,
                    RoomType = (RoomType)accountInfo.RoomType,
                    Services = accountInfo.НаименованиеРабот,
                    Tariff = accountInfo.Тариф,
                    Area = (float)accountInfo.ПлощадьПомещения,
                    BaseTariffSum = accountInfo.ИтогоПоБазовомуТарифу,
                    DecisionTariffSum = accountInfo.ИтогоПоТарифуРешений,
                    PenaltySum = accountInfo.ИтогоПоПени
                };

                DomainEvents.Raise(new AccountSnapshotEvent(accountData, accountInfo.OwnerId, PaymentDocumentData.OwnerholderType, isBase));
            }
        }
    }
}