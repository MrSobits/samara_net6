namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Utils;

    using Bars.Gkh.Domain.ParameterVersioning.Proxy;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Dto;
    using Bars.Gkh.Utils;
    using Domain;
    using Domain.ParametersVersioning;
    using Entities;
    using Entities.PersonalAccount;
    using Enums;
    using Gkh.Domain.CollectionExtensions;

    /// <summary>
    /// Методы класса <see cref="PersonalAccountChargeCalculator"/> для проведения перерасчёта
    /// </summary>
    public partial class PersonalAccountChargeCalculator
    {
        /// <summary>
        /// В случае ручного пересчёта устанавливается в true
        /// </summary>
        private bool needRecalcBeforeOpen;
        private RecalcReasonProxy RecalculationReason;

        private void RecalculateCharge(IPeriod period, BasePersonalAccount account, UnacceptedCharge unaccepted)
        {
            if (account.OpenDate > period.StartDate)
            {
                return;
            }

            var calculatingFirstPeriod = period.StartDate == this.periodRepo.GetFirstPeriod().StartDate;

            /*
             * Если мы считаем перерасчет по первому периоду,
             * то перерасчет не считаем - 0
             */
            if (calculatingFirstPeriod)
            {
                return;
            }

            var needToRecalcPeriods = this.GetRecalcPeriods(account, period);

            if (!needToRecalcPeriods.Any())
            {
                return;
            }

            var shareParam = this.tracker.GetParameter(VersionedParameters.AreaShare, account, period);
            var areaParam = this.tracker.GetParameter(VersionedParameters.RoomArea, account, period);
            var tariffParam = this.tracker.GetParameter(VersionedParameters.BaseTariff, account, period);

            decimal baseRecalc = 0,
                decisionRecalc = 0,
                beforeBaseRecalc = 0,
                beforeDecisionRecalc = 0;

            var wasRecalculated = false;

            // если происходит ручной перерасчёт, то необходимо пересчитать периоды,
            // которые были до фактического открытия ЛС и все начисления посадить с минусом
            if (this.needRecalcBeforeOpen)
            {
                //перерасчёт за периоды до открытия ЛС
                wasRecalculated = this.RecalcBeforeAccountOpen(
                    period,
                    account,
                    unaccepted,
                    needToRecalcPeriods,
                    ref beforeBaseRecalc,
                    ref beforeDecisionRecalc);
            }

            IEnumerable<PersonalAccountChargeDto> accountCharges = this.cache.GetAllCharges(account).ToList();

            foreach (var recalcPeriod in needToRecalcPeriods)
            {
                var tmpPeriod = recalcPeriod;

                var bTariff = tariffParam.GetActualByDate<decimal>(account, tmpPeriod.StartDate, true).Value;

                decimal dTariff = bTariff;

                TariffSource tariffSource;

                var decisionTarif = this.cache.GetDecisionTarif(account.Room.RealityObject, tmpPeriod.StartDate);

                if (decisionTarif != null && decisionTarif.Decision.Value > bTariff)
                {
                    dTariff = decisionTarif.Decision.Value;
                    tariffSource = new TariffSource
                    {
                        TariffSourceType = TariffSourceType.PaySizeByProtocol,
                        ProtocolNum = decisionTarif.Protocol.DocumentNum,
                        ProtocolDate = decisionTarif.Protocol.ProtocolDate
                    };
                }
                else
                {
                    dTariff = bTariff;
                    tariffSource = (TariffSource) tariffParam.GetActualByDate<decimal>(account, tmpPeriod.StartDate, true).Key;
                }

                var begin =
                    tmpPeriod.StartDate.Date >= account.OpenDate.Date
                        ? tmpPeriod.StartDate.Date
                        : account.OpenDate.Date;

                var end = tmpPeriod.GetEndDate();

                var currentParams = new CalculationTraceProxy
                {
                    Area = areaParam.GetLastChangedByDate<decimal>(account, tmpPeriod, begin.Date).Value,
                    DateActualArea = areaParam.GetLastChangedByDate<decimal>(account, tmpPeriod, begin.Date).Key.ToDateTime(),
                    Share = shareParam.GetLastChangedByDate<decimal>(account, tmpPeriod, begin.Date).Value,
                    DateActualShare = shareParam.GetLastChangedByDate<decimal>(account, tmpPeriod, begin.Date).Key.ToDateTime(),
                    Tariff = bTariff,
                    TariffSource = tariffSource,
                    DecisionTariff = dTariff,
                    DateStart = begin,
                    DateEnd = end
                };

                var tempTrace = new List<CalculationTraceProxy> { currentParams };

                for (var currentDate = begin; currentDate <= end; currentDate = currentDate.AddDays(1))
                {
                    var baseTariff = tariffParam.GetActualByDate<decimal>(account, currentDate, true).Value;
                    var currentShare = shareParam.GetActualByDate<decimal>(account, currentDate, true);
                    var currentArea = areaParam.GetActualByDate<decimal>(account, currentDate, true);

                    var decisionTariff = baseTariff;

                    var decision = this.cache.GetDecisionTarif(account.Room.RealityObject, currentDate);

                    if (decision != null && decision.Decision.Value > baseTariff)
                    {
                        decisionTariff = decision.Decision.Value;
                        tariffSource = new TariffSource
                        {
                            TariffSourceType = TariffSourceType.PaySizeByProtocol,
                            ProtocolNum = decision.Protocol.DocumentNum,
                            ProtocolDate = decision.Protocol.ProtocolDate
                        };
                    }
                    else
                    {
                        tariffSource = (TariffSource) tariffParam.GetActualByDate<decimal>(account, currentDate, true).Key;
                    }

                    //если изменился один из параметров, 
                    //то закрываем текущий "период параметров" и создаем новый, который добавляем в темповый список
                    if (baseTariff != currentParams.Tariff
                        || decisionTariff != currentParams.DecisionTariff
                        || currentShare.Value != currentParams.Share
                        || currentArea.Value != currentParams.Area)
                    {
                        currentParams.DateEnd = currentDate.AddDays(-1);

                        currentParams = new CalculationTraceProxy
                        {
                            Area = currentArea.Value,
                            DateActualArea = currentArea.Key.ToDateTime(),
                            Share = currentShare.Value,
                            DateActualShare = currentShare.Key.ToDateTime(),
                            Tariff = baseTariff,
                            TariffSource = tariffSource,
                            DecisionTariff = decisionTariff,
                            DateStart = currentDate,
                            DateEnd = end
                        };

                        tempTrace.Add(currentParams);
                    }
                }

                decimal tmpBaseRecalc = 0,
                    tmpDecisionRecalc = 0,
                    baseDeltaForSave = 0,
                    decDeltaForSave = 0;

                /*Удаляем неактуальные параметры*/
                tempTrace.RemoveAll(x => x.DateStart > x.DateEnd);

                //формируем из темпового списка "периодов параметров"
                //сохраняемые параметры
                foreach (var t in tempTrace)
                {
                    var days = (t.DateEnd - t.DateStart).Days + 1;
                    var oldtmpTariffRecalc = tmpBaseRecalc;
                    tmpBaseRecalc += days != 0
                        ? (t.Tariff * t.Share * t.Area) * days / DateTime.DaysInMonth(t.DateStart.Year, t.DateStart.Month)
                        : 0;

                    var oldtmpDecionRecalc = tmpDecisionRecalc;
                    tmpDecisionRecalc += days != 0
                        ? ((t.DecisionTariff - t.Tariff) * t.Share * t.Area) * days / DateTime.DaysInMonth(t.DateStart.Year, t.DateStart.Month)
                        : 0;

                    var tempCharges = accountCharges
                        .Where(x => x.ChargeDate.Date <= recalcPeriod.EndDate)
                        .Where(x => recalcPeriod.StartDate <= x.ChargeDate.Date)
                        .ToArray();

                    var baseTariffWithRecalc = tempCharges
                        .SafeSum(x => x.ChargeTariff - x.OverPlus);

                    var decionTariffWithRecalc = tempCharges
                        .SafeSum(x => x.OverPlus);

                    //добавляем перерасчеты из истории
                    var recalcHistoryForCurrentPeriod = this.cache.GetRecalcHistory(account)
                        .Where(x => x.RecalcPeriod.Id == recalcPeriod.Id)
                        .Where(x => x.RecalcType != RecalcType.Penalty)
                        .ToList();

                    var baseRecalcSum = recalcHistoryForCurrentPeriod
                        .Where(x => x.RecalcType == RecalcType.BaseTariffCharge)
                        .SafeSum(x => x.RecalcSum);

                    var decRecalcSum = recalcHistoryForCurrentPeriod
                        .Where(x => x.RecalcType == RecalcType.DecisionTariffCharge)
                        .SafeSum(x => x.RecalcSum);

                    baseTariffWithRecalc += baseRecalcSum;
                    decionTariffWithRecalc += decRecalcSum;

                    // получаем количество дней с учетом того, что дата открытия счета может быть позже начала периода.
                    var daysInMonth =
                        tmpPeriod.StartDate.Date < account.OpenDate.Date && !this.needRecalcBeforeOpen
                            ? (DateTime.DaysInMonth(t.DateStart.Year, t.DateStart.Month) - account.OpenDate.Date.Day) + 1
                            : DateTime.DaysInMonth(t.DateStart.Year, t.DateStart.Month);

                    baseTariffWithRecalc = 
                        (baseTariffWithRecalc * days / daysInMonth);

                    decionTariffWithRecalc =
                        (decionTariffWithRecalc * days / daysInMonth);

                    if (tempTrace.Count == 1 && days == daysInMonth)
                    {
                        baseTariffWithRecalc = baseTariffWithRecalc.RegopRoundDecimal(2);
                        decionTariffWithRecalc = decionTariffWithRecalc.RegopRoundDecimal(2);
                        tmpBaseRecalc = tmpBaseRecalc.RegopRoundDecimal(2);
                        tmpDecisionRecalc = tmpDecisionRecalc.RegopRoundDecimal(2);
                    }

                    var baseDelta = tmpBaseRecalc - oldtmpTariffRecalc - baseTariffWithRecalc;
                    var decDelta = tmpDecisionRecalc - oldtmpDecionRecalc - decionTariffWithRecalc;

                    baseRecalc += baseDelta;
                    decisionRecalc += decDelta;

                    baseDeltaForSave += baseDelta;
                    decDeltaForSave += decDelta;

                    this.traces.Add(new CalculationParameterTrace
                    {
                        ParameterValues = new Dictionary<string, object>
                        {
                            {tariffParam.ParameterName, t.Tariff},
                            {"Decision_Tariff", t.DecisionTariff != t.Tariff ? t.DecisionTariff : 0},
                            {"TariffSource", t.TariffSource},
                            {shareParam.ParameterName, t.Share},
                            {"DateActualShare", t.DateActualShare },
                            {areaParam.ParameterName, t.Area},
                            {"DateActualArea", t.DateActualArea },
                            {"Recalc_ByBase", (tmpBaseRecalc - oldtmpTariffRecalc).RegopRoundDecimal(3)},
                            {"Recalc_Decision", (tmpDecisionRecalc - oldtmpDecionRecalc).RegopRoundDecimal(3)},
                            {"Fact_Charge", baseTariffWithRecalc + decionTariffWithRecalc},
                            {"RecalcReason", this.RecalculationReason}
                        },
                        DateStart = t.DateStart,
                        DateEnd = t.DateEnd,
                        CalculationType = CalculationTraceType.Recalc,
                        CalculationGuid = unaccepted.Guid,
                        ChargePeriod = (ChargePeriod)period
                    });
                }

                if (baseDeltaForSave != 0m)
                {
                    this.recalcHistory.Add(new RecalcHistory
                    {
                        CalcPeriod = (ChargePeriod)period,
                        PersonalAccount = account,
                        RecalcSum = baseDeltaForSave.RegopRoundDecimal(2),
                        RecalcPeriod = recalcPeriod,
                        RecalcType = RecalcType.BaseTariffCharge
                    });
                }

                if (decDeltaForSave != 0m)
                {
                    this.recalcHistory.Add(new RecalcHistory
                    {
                        CalcPeriod = (ChargePeriod)period,
                        PersonalAccount = account,
                        RecalcSum = decDeltaForSave.RegopRoundDecimal(2),
                        RecalcPeriod = recalcPeriod,
                        RecalcType = RecalcType.DecisionTariffCharge
                    });
                }

                wasRecalculated = true;
            }

            if (wasRecalculated)
            {
                this.recalc = new TariffRecalc(
                    baseRecalc.RegopRoundDecimal(2) + beforeBaseRecalc.RegopRoundDecimal(2),
                    decisionRecalc.RegopRoundDecimal(2) + beforeDecisionRecalc.RegopRoundDecimal(2));
            }
        }

        private bool RecalcBeforeAccountOpen(IPeriod period,
            BasePersonalAccount account,
            UnacceptedCharge unaccepted,
            List<ChargePeriod> needToRecalcPeriods,
            ref decimal beforeBaseRecalc,
            ref decimal beforeDecisionRecalc)
        {
            bool wasRecalculated = false;

            IEnumerable<PersonalAccountChargeDto> accountCharges = this.cache.GetAllCharges(account).ToList();
            var periodsBeforeAccOpen = needToRecalcPeriods.Where(x => x.StartDate.Date < account.OpenDate.Date).OrderBy(x => x.StartDate);

            var shareParam = this.tracker.GetParameter(VersionedParameters.AreaShare, account, period);
            var areaParam = this.tracker.GetParameter(VersionedParameters.RoomArea, account, period);
            var tariffParam = this.tracker.GetParameter(VersionedParameters.BaseTariff, account, period);

            foreach (var recalcPeriod in periodsBeforeAccOpen)
            {
                var tmpPeriod = recalcPeriod;

                var begin = tmpPeriod.StartDate.Date;

                //крайняя дата - конец периода, если ЛС открыт не в этом периоде
                //или предыдущий день открытия ЛС в противном случае
                var periodEndDate = tmpPeriod.GetEndDate();
                var end = periodEndDate < account.OpenDate.Date
                    ? periodEndDate
                    : account.OpenDate.Date.AddDays(-1);

                var currentParams = new CalculationTraceProxy
                {
                    Area = areaParam.GetLastChangedByDate<decimal>(account, tmpPeriod, end.Date).Value,
                    DateActualArea = areaParam.GetLastChangedByDate<decimal>(account, tmpPeriod, end.Date).Key.ToDateTime(),
                    Share = shareParam.GetLastChangedByDate<decimal>(account, tmpPeriod, end.Date).Value,
                    DateActualShare = shareParam.GetLastChangedByDate<decimal>(account, tmpPeriod, end.Date).Key.ToDateTime(),
                    DateStart = begin,
                    DateEnd = end
                };

                var days = (currentParams.DateEnd - currentParams.DateStart).Days + 1;

                var tempCharges = accountCharges
                    .Where(x => x.ChargePeriodId == recalcPeriod.Id)
                    .ToArray();

                //предыдущие начисления (берём от количества дней, так как у нас может быть открыть ЛС в середине периода)
                var baseTariffWithRecalc = tempCharges
                    .SafeSum(x => x.ChargeTariff - x.OverPlus) * days / DateTime.DaysInMonth(currentParams.DateStart.Year, currentParams.DateStart.Month);

                var decionTariffWithRecalc = tempCharges
                    .SafeSum(x => x.OverPlus) * days / DateTime.DaysInMonth(currentParams.DateStart.Year, currentParams.DateStart.Month);

                //добавляем перерасчеты из истории
                var recalcHistoryForCurrentPeriod = this.cache.GetRecalcHistory(account)
                    .Where(x => x.RecalcPeriod.Id == recalcPeriod.Id)
                    .Where(x => x.RecalcType != RecalcType.Penalty)
                    .ToList();

                var baseRecalcSum = recalcHistoryForCurrentPeriod
                    .Where(x => x.RecalcType == RecalcType.BaseTariffCharge)
                    .SafeSum(x => x.RecalcSum);

                var decRecalcSum = recalcHistoryForCurrentPeriod
                    .Where(x => x.RecalcType == RecalcType.DecisionTariffCharge)
                    .SafeSum(x => x.RecalcSum);

                baseRecalcSum =
                        (baseRecalcSum * days / DateTime.DaysInMonth(currentParams.DateStart.Year, currentParams.DateStart.Month))
                            .RegopRoundDecimal(2);

                decRecalcSum =
                    (decRecalcSum * days / DateTime.DaysInMonth(currentParams.DateStart.Year, currentParams.DateStart.Month))
                        .RegopRoundDecimal(2);

                baseTariffWithRecalc += baseRecalcSum;
                decionTariffWithRecalc += decRecalcSum;

                beforeBaseRecalc -= baseTariffWithRecalc;
                beforeDecisionRecalc -= decionTariffWithRecalc;

                this.traces.Add(new CalculationParameterTrace
                {
                    ParameterValues = new Dictionary<string, object>
                    {
                        {tariffParam.ParameterName, 0},
                        {"Decision_Tariff", 0},
                        {shareParam.ParameterName, currentParams.Share},
                        {"DateActualShare", currentParams.DateActualShare },
                        {areaParam.ParameterName, currentParams.Area},
                        {"DateActualArea", currentParams.DateActualArea },
                        {"Recalc_ByBase", 0},
                        {"Recalc_Decision", 0},
                        {"Fact_Charge", (baseTariffWithRecalc + decionTariffWithRecalc).RegopRoundDecimal(2)},
                        {"RecalcReason", this.RecalculationReason }
                    },
                    DateStart = currentParams.DateStart,
                    DateEnd = currentParams.DateEnd,
                    CalculationType = CalculationTraceType.Recalc,
                    CalculationGuid = unaccepted.Guid,
                    ChargePeriod = (ChargePeriod)period
                });

                if (baseTariffWithRecalc != 0m)
                {
                    this.recalcHistory.Add(new RecalcHistory
                    {
                        CalcPeriod = (ChargePeriod)period,
                        PersonalAccount = account,
                        RecalcSum = -baseTariffWithRecalc.RegopRoundDecimal(2),
                        RecalcPeriod = recalcPeriod,
                        RecalcType = RecalcType.BaseTariffCharge
                    });
                }

                if (decionTariffWithRecalc != 0m)
                {
                    this.recalcHistory.Add(new RecalcHistory
                    {
                        CalcPeriod = (ChargePeriod)period,
                        PersonalAccount = account,
                        RecalcSum = -decionTariffWithRecalc.RegopRoundDecimal(2),
                        RecalcPeriod = recalcPeriod,
                        RecalcType = RecalcType.DecisionTariffCharge
                    });
                }

                wasRecalculated = true;
            }

            return wasRecalculated;
        }

        private List<ChargePeriod> GetRecalcPeriods(BasePersonalAccount account, IPeriod period)
        {
            var allChanges = this.tracker.GetChanges(account, period.StartDate).ToList();

            if (allChanges.Any())
            {
                this.RecalculationReason = allChanges.OrderBy(x => x.DateActualChange)
                    .Select(x => new RecalcReasonProxy
                    {
                        Reason = this.GetRecalcReasonFromChanges(x),
                        Date = x.DateActualChange,
                        Value = x.PropertyValue
                    }).FirstOrDefault();
            }

            IEnumerable<PersonalAccountChargeDto> accountCharges = this.cache.GetAllCharges(account).ToList();

            var closedPeriods = this.cache.GetClosedPeriods().ToList();

            // минимальная дата изменений по параметрам
            var minTrackerDate = allChanges.Count != 0
                ? allChanges.SafeMin(x => x.DateActualChange.Date)
                : DateTime.MaxValue;

            // минимальная дата по ручному перерасчету
            var manuallyRecalcDate = this.cache.GetManuallyRecalcDate(account);

            var recalcEvents = this.cache.GetRecalcEvents(account);

            var recalcOpenDate = recalcEvents
                .Where(x => x.RecalcEventType == RecalcEventType.ChangeOpenDate)
                .OrderBy(x => x.EventDate)
                .FirstOrDefault();

            this.needRecalcBeforeOpen = manuallyRecalcDate.HasValue || (recalcOpenDate?.EventDate.IsValid() ?? false);

            if (manuallyRecalcDate.HasValue && manuallyRecalcDate < minTrackerDate)
            {
                minTrackerDate = manuallyRecalcDate.ToDateTime();
                if (this.RecalculationReason == null || this.RecalculationReason.Date > minTrackerDate)
                {
                    this.RecalculationReason = new RecalcReasonProxy
                    {
                        Reason = RecalcReason.ManuallyRecalc,
                        Date = minTrackerDate
                    };
                }
            }

            if (recalcOpenDate.IsNotNull() && recalcOpenDate.EventDate.IsValid() && recalcOpenDate.EventDate < minTrackerDate)
            {
                minTrackerDate = recalcOpenDate.EventDate;
                this.RecalculationReason = new RecalcReasonProxy
                {
                    Reason = RecalcReason.ChangeOpenDate,
                    Date = minTrackerDate,
                    Value = recalcOpenDate.PersonalAccount.OpenDate.ToShortDateString()
                };
            }

            var recalcCloseDate = recalcEvents
                .Where(x => x.RecalcEventType == RecalcEventType.ChangeCloseDate)
                .OrderBy(x => x.EventDate)
                .FirstOrDefault();

            if (recalcCloseDate.IsNotNull() && recalcCloseDate.EventDate.IsValid() && recalcCloseDate.EventDate <= minTrackerDate)
            {
                minTrackerDate = recalcCloseDate.EventDate;
                this.RecalculationReason = new RecalcReasonProxy
                {
                    Reason = RecalcReason.ChangeCloseDate,
                    Date = minTrackerDate,
                    Value = recalcCloseDate.PersonalAccount.CloseDate.ToShortDateString()
                };
            }

            var minRecalcPeriod =
                closedPeriods
                    // убираем периоды, в которых параметр не начал свое фактическое действие
                    .Where(x => minTrackerDate <= x.EndDate.Value.Date && x.EndDate.Value.Date <= period.StartDate.Date)
                    // если ручной перерасчёт, то нам нужны все периоды, иначе же, обрубаем периоды до открытия ЛС и не трогаем их
                    .WhereIf(!this.needRecalcBeforeOpen, x => x.EndDate >= account.OpenDate.Date)
                    // Если нет ни одного начисления старше начала какого-то периода с перерасчетом > 0
                    .OrderBy(x => x.StartDate)
                    .FirstOrDefault();

            var needToRecalcPeriods = new List<ChargePeriod>();

            if (minRecalcPeriod != null)
            {
                var firstCharge = accountCharges.OrderBy(x => x.ChargeDate).FirstOrDefault();
                var accountOpenDate = account.OpenDate;

                // Если минимальный период для перерасчета попадает в промежуток между датой открытия счета 
                // и датой первого подтвержденного начисления, то начинаем перерасчет с периода, в котором был
                // открыт счет
                if (minRecalcPeriod.EndDate <= firstCharge.Return(x => x.ChargeDate, DateTime.MinValue)
                    && minRecalcPeriod.StartDate >= accountOpenDate)
                {
                    minRecalcPeriod =
                        closedPeriods.FirstOrDefault(x => x.StartDate <= accountOpenDate && x.EndDate >= accountOpenDate);
                }

                if (minRecalcPeriod != null)
                {
                    needToRecalcPeriods = closedPeriods.Where(x => x.StartDate >= minRecalcPeriod.StartDate).ToList();
                }
            }

            var banRecalc = this.cache.GetBanRecalc(account).Where(x => x.Type.HasFlag(BanRecalcType.Charge));

            var exceptPeriods = new List<ChargePeriod>();

            // убираем периоды на которые установлен запрет перерасчета
            foreach (var ban in banRecalc)
            {
                var periods = needToRecalcPeriods.Where(x => x.StartDate.Date >= ban.DateStart.Date && x.StartDate.Date <= ban.DateEnd.Date);

                exceptPeriods.AddRange(periods);
            }

            needToRecalcPeriods = needToRecalcPeriods.Except(exceptPeriods).ToList();

            return needToRecalcPeriods;
        }

        private RecalcReason GetRecalcReasonFromChanges(EntityLogRecord record)
        {
            return this.paramNameToReasonDict.Get(record.ParameterName);
        }

        private readonly IDictionary<string, RecalcReason> paramNameToReasonDict = new Dictionary<string, RecalcReason>
        {
            { "room_area", RecalcReason.Area },
            { "area_share", RecalcReason.AreaShare },
            { "account_open_date", RecalcReason.ChangeOpenDate },
            { "base_tariff", RecalcReason.RecalcCharge }
        };
    }
}